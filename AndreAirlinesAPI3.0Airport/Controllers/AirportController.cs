using AndreAirlinesAPI3._0Airport.Service;
using AndreAirlinesAPI3._0ErrorMessages;
using AndreAirlinesAPI3._0Models;
using AndreAirlinesAPI3._0SearchZipcode;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public ActionResult<List<Airport>> Get()
        {
            var airports = _airportService.Get();

            if (airports[0].ErrorCode != null)
                return BadRequest("Aeroporto - " + ErrorMessage.ReturnMessage(airports[0].ErrorCode));

            return airports;
        }

        [HttpGet("{id}", Name = "GetAirport")]
        public ActionResult<Airport> Get(string id)
        {
            var airport = _airportService.Get(id);

            if (airport.ErrorCode != null)
                return BadRequest("Aeroporto - " + ErrorMessage.ReturnMessage(airport.ErrorCode));
            else if (airport == null)
                return NotFound();

            return airport;
        }

        [HttpPost]
        public async Task<ActionResult<Airport>> Create(Airport airport)
        {
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

            var airportData = await SearchAiportData.ReturnAirportData(airport.IataCode);

            if (airportData.ErrorCode != null)
                return BadRequest("Código IATA - " + ErrorMessage.ReturnMessage(airportData.ErrorCode));
            else
            {
                airport.IataCode = airportData.Code;
                airport.Address.City = airportData.City;
                airport.Address.Country = airportData.Country;
                airport.Address.Continent = airportData.Continent;                
            }

            var airportInsertion = await _airportService.Create(airport);

            if (airportInsertion.ErrorCode == "noLog")
                return BadRequest("Log - " + ErrorMessage.ReturnMessage("noLog"));
            else if (airportInsertion.ErrorCode != null)
                return BadRequest("Aeroporto - " + ErrorMessage.ReturnMessage(airportInsertion.ErrorCode));

            return CreatedAtRoute("GetAirport", new { id = airport.Id }, airport);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Airport airportIn)
        {
            Airport airport = new();
            string returnMsg;

            var user = await SearchUser.ReturnUser(airportIn.LoginUser);

            if (user.LoginUser == null)
                return BadRequest("Aeroporto - " + ErrorMessage.ReturnMessage("noBlank"));
            if (user.ErrorCode != null)
                return BadRequest("Aeroporto - " + ErrorMessage.ReturnMessage(user.ErrorCode));
            else if (user.Sector != "ADM")
                return BadRequest("Aeroporto - " + ErrorMessage.ReturnMessage("noPermited"));
            else
                airport = _airportService.Get(id);

            if (airport.ErrorCode != null)
                return BadRequest("Aeroporto - " + ErrorMessage.ReturnMessage(airport.ErrorCode));
            else if (airport == null)
                return NotFound();
            else
                returnMsg = await _airportService.Update(id, airportIn, user);

            if (returnMsg != "ok")
                return BadRequest("Log - " + ErrorMessage.ReturnMessage("noLog"));

            return Ok("Aeroporto atualizado com sucesso. Log gravado com sucesso.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, Airport airportIn)
        {
            Airport airport = new();
            string returnMsg;

            var user = await SearchUser.ReturnUser(airportIn.LoginUser);

            if (user.LoginUser == null)
                return BadRequest("Aeroporto - " + ErrorMessage.ReturnMessage("noBlank"));
            if (user.ErrorCode != null)
                return BadRequest("Aeroporto - " + ErrorMessage.ReturnMessage(user.ErrorCode));
            else if (user.Sector != "ADM")
                return BadRequest("Aeroporto - " + ErrorMessage.ReturnMessage("noPermited"));
            else
                airport = _airportService.Get(id);

            if (airport.ErrorCode != null)
                return BadRequest("Aeroporto - " + ErrorMessage.ReturnMessage(airport.ErrorCode));
            else if (airport == null)
                return NotFound();
            else
                returnMsg = await _airportService.Remove(airport.Id, airport, user);

            return Ok("Aeroporto excluído com sucesso. Log gravado com sucesso.");
        }
    }
}
