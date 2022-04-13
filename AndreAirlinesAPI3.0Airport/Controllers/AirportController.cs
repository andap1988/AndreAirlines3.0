using AndreAirlinesAPI3._0Airport.Service;
using AndreAirlinesAPI3._0ErrorMessages;
using AndreAirlinesAPI3._0Models;
using AndreAirlinesAPI3._0SearchZipcode;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0Airport.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirportController : ControllerBase
    {
        private readonly AirportService _airportService;
        public bool utilizationSearchZipcode = true;

        public AirportController(AirportService airportService)
        {
            _airportService = airportService;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] User userIn)
        {
            User searchUser = await SearchUser.ReturnUserLogin(userIn);

            if (searchUser == null || searchUser.ErrorCode != null)
                return NotFound("Usuário - " + ErrorMessage.ReturnMessage("noUser"));

            var token = TokenService.GenerateToken(searchUser);

            searchUser.Password = "";

            return new
            {
                user = searchUser,
                token = token
            };
        }

        [HttpGet]
        [Authorize]
        public ActionResult<List<Airport>> Get()
        {
            var airports = _airportService.Get();

            if (airports[0].ErrorCode != null)
                return BadRequest("Aeroporto - " + ErrorMessage.ReturnMessage(airports[0].ErrorCode));

            return airports;
        }

        [HttpGet("{id}", Name = "GetAirport")]
        [Authorize]
        public ActionResult<Airport> Get(string id)
        {
            var airport = _airportService.Get(id);

            if (airport == null)
                return NotFound();
            else if (airport.ErrorCode != null)
                return BadRequest("Aeroporto - " + ErrorMessage.ReturnMessage(airport.ErrorCode));

            return airport;
        }

        [HttpGet("code/{iataCode}")]
        [Authorize(Roles = "adm")]
        public ActionResult<Airport> GetIataCode(string iataCode)
        {
            var airport = _airportService.GetIataCode(iataCode.ToUpper());

            if (airport == null)
                return NotFound();
            else if (airport.ErrorCode != null)
                return BadRequest("Aeroporto - " + ErrorMessage.ReturnMessage(airport.ErrorCode));

            return airport;
        }

        [HttpPost]
        [Authorize(Roles = "adm")]
        public async Task<ActionResult<Airport>> Create(Airport airport)
        {
            var user = User.Identity.Name;
            var token = Request.Headers[HeaderNames.Authorization].ToString().Split(" ")[1];

            if (airport.IataCode.Length != 3)
                return BadRequest("Codigo Iata - " + ErrorMessage.ReturnMessage("noLengthIataCode"));

            if (utilizationSearchZipcode)
            {
                Address address = await SearchZipcode.ReturnAddress(airport.Address);

                if (address.ErrorCode != null)
                    return BadRequest("Endereço - " + ErrorMessage.ReturnMessage(address.ErrorCode));

                airport.Address = address;
            }

            if (airport.IataCode == null)
                return BadRequest("Codigo IATA - " + ErrorMessage.ReturnMessage("noIataCode"));

            var airportData = await SearchAirportData.ReturnAirportData(airport.IataCode, token);

            if (airportData.ErrorCode != null)
                return BadRequest("Código IATA - " + ErrorMessage.ReturnMessage(airportData.ErrorCode));
            else
            {
                airport.IataCode = airportData.Code;
                airport.Address.City = airportData.City;
                airport.Address.Country = airportData.Country;
                airport.Address.Continent = airportData.Continent;
            }

            var airportInsertion = await _airportService.Create(airport, user, token);

            if (airportInsertion.ErrorCode == "noLog")
                return BadRequest("Log - " + ErrorMessage.ReturnMessage("noLog"));
            else if (airportInsertion.ErrorCode != null)
                return BadRequest("Aeroporto - " + ErrorMessage.ReturnMessage(airportInsertion.ErrorCode));

            return CreatedAtRoute("GetAirport", new { id = airport.Id }, airport);

        }

        [HttpPut("{id}")]
        [Authorize(Roles = "adm")]
        public async Task<IActionResult> Update(string id, Airport airportIn)
        {
            Airport airport = new();
            string returnMsg;
            var user = User.Identity.Name;
            var token = Request.Headers[HeaderNames.Authorization].ToString().Split(" ")[1];

            airport = _airportService.Get(id);

            if (airport.ErrorCode != null)
                return BadRequest("Aeroporto - " + ErrorMessage.ReturnMessage(airport.ErrorCode));
            else if (airport == null)
                return NotFound();
            else
                returnMsg = await _airportService.Update(id, airportIn, user, token);

            if (returnMsg != "ok")
                return BadRequest("Log - " + ErrorMessage.ReturnMessage("noLog"));

            return Ok("Aeroporto atualizado com sucesso. Log gravado com sucesso.");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "adm")]
        public async Task<IActionResult> Delete(string id, Airport airportIn)
        {
            Airport airport = new();
            string returnMsg;
            var user = User.Identity.Name;
            var token = Request.Headers[HeaderNames.Authorization].ToString().Split(" ")[1];

            airport = _airportService.Get(id);

            if (airport == null)
                return NotFound();
            else if (airport.ErrorCode != null)
                return BadRequest("Aeroporto - " + ErrorMessage.ReturnMessage(airport.ErrorCode));
            else
                returnMsg = await _airportService.Remove(airport.Id, airport, user, token);

            return Ok("Aeroporto excluído com sucesso. Log gravado com sucesso.");
        }
    }
}
