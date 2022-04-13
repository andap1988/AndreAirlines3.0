using AndreAirlinesAPI3._0ErrorMessages;
using AndreAirlinesAPI3._0Flight.Service;
using AndreAirlinesAPI3._0Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0Flight.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly FlightService _flightService;

        public FlightsController(FlightService flightService)
        {
            _flightService = flightService;
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
        public ActionResult<List<Flight>> Get()
        {
            var flights = _flightService.Get();

            if (flights[0].ErrorCode != null)
                return BadRequest("Voo - " + ErrorMessage.ReturnMessage(flights[0].ErrorCode));

            return flights;
        }

        [HttpGet("{id}", Name = "GetFlight")]
        [Authorize]
        public ActionResult<Flight> Get(string id)
        {
            var flight = _flightService.Get(id);

            if (flight == null)
                return NotFound();
            else if (flight.ErrorCode != null)
                return BadRequest("Voo - " + ErrorMessage.ReturnMessage(flight.ErrorCode));

            return flight;
        }

        [HttpPost]
        [Authorize(Roles = "adm,user")]
        public async Task<ActionResult<Flight>> Create(Flight flight)
        {
            var user = User.Identity.Name;
            var token = Request.Headers[HeaderNames.Authorization].ToString().Split(" ")[1];

            var flightInsertion = await _flightService.Create(flight, user, token);

            if (flightInsertion.ErrorCode == "noLog")
                return BadRequest("Log - " + ErrorMessage.ReturnMessage("noLog"));
            else if (flightInsertion.Origin.ErrorCode != null)
                return BadRequest("Aeroporto de Origem - " + ErrorMessage.ReturnMessage(flightInsertion.Origin.ErrorCode));
            else if (flightInsertion.Destiny.ErrorCode != null)
                return BadRequest("Aeroporto de Destino - " + ErrorMessage.ReturnMessage(flightInsertion.Destiny.ErrorCode));
            else if (flightInsertion.Airship.ErrorCode != null)
                return BadRequest("Aeronave - " + ErrorMessage.ReturnMessage(flightInsertion.Destiny.ErrorCode));

            return CreatedAtRoute("GetFlight", new { id = flight.Id }, flight);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "adm,user")]
        public async Task<IActionResult> Update(string id, Flight flightIn)
        {
            Flight flight = new();
            string returnMsg;
            var user = User.Identity.Name;
            var token = Request.Headers[HeaderNames.Authorization].ToString().Split(" ")[1];

            flight = _flightService.Get(id);

            if (flight == null)
                return NotFound();
            else if (flight.ErrorCode != null)
                return BadRequest("Voo - " + ErrorMessage.ReturnMessage(flight.ErrorCode));
            else
                returnMsg = await _flightService.Update(id, flightIn, user, token);

            if (returnMsg != "ok")
                return BadRequest("Log - " + ErrorMessage.ReturnMessage("noLog"));

            return Ok("Voo atualizado com sucesso. Log gravado com sucesso.");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "adm,user")]
        public async Task<IActionResult> Delete(string id, Flight flightIn)
        {
            Flight flight = new();
            string returnMsg;
            var user = User.Identity.Name;
            var token = Request.Headers[HeaderNames.Authorization].ToString().Split(" ")[1];

            flight = _flightService.Get(id);

            if (flight == null)
                return NotFound();
            else if (flight.ErrorCode != null)
                return BadRequest("Voo - " + ErrorMessage.ReturnMessage(flight.ErrorCode));
            else
                returnMsg = await _flightService.Remove(flight.Id, flight, user, token);

            if (returnMsg != "ok")
                return BadRequest("Log - " + ErrorMessage.ReturnMessage("noLog"));

            return Ok("Voo excluído com sucesso. Log gravado com sucesso.");
        }
    }
}
