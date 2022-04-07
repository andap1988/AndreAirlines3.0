using AndreAirlinesAPI3._0ErrorMessages;
using AndreAirlinesAPI3._0Flight.Service;
using AndreAirlinesAPI3._0Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public ActionResult<List<Flight>> Get()
        {
            var flights = _flightService.Get();

            if (flights[0].ErrorCode != null)
                return BadRequest("Voo - " + ErrorMessage.ReturnMessage(flights[0].ErrorCode));

            return flights;
        }

        [HttpGet("{id}", Name = "GetFlight")]
        public ActionResult<Flight> Get(string id)
        {            
            var flight = _flightService.Get(id);

            if (flight.ErrorCode != null)
                return BadRequest("Voo - " + ErrorMessage.ReturnMessage(flight.ErrorCode));
            else if (flight == null)
                return NotFound();

            return flight;
        }

        [HttpPost]
        public async Task<ActionResult<Flight>> Create(Flight flight)
        {
            var flightInsertion = await _flightService.Create(flight);

            if (flightInsertion.Origin.ErrorCode != null)
                return BadRequest("Aeroporto de Origem - " + ErrorMessage.ReturnMessage(flightInsertion.Origin.ErrorCode));
            else if (flightInsertion.Destiny.ErrorCode != null)
                return BadRequest("Aeroporto de Destino - " + ErrorMessage.ReturnMessage(flightInsertion.Destiny.ErrorCode));
            else if (flightInsertion.Airship.ErrorCode != null)
                return BadRequest("Aeronave - " + ErrorMessage.ReturnMessage(flightInsertion.Destiny.ErrorCode));
            else
                return CreatedAtRoute("GetFlight", new { id = flight.Id }, flight);

        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, Flight flightIn)
        {
            var flight = _flightService.Get(id);

            if (flight.ErrorCode != null)
                return BadRequest("Voo - " + ErrorMessage.ReturnMessage(flight.ErrorCode));
            else if (flight == null)
                return NotFound();
            else
                _flightService.Update(id, flightIn);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var flight = _flightService.Get(id);

            if (flight.ErrorCode != null)
                return BadRequest("Voo - " + ErrorMessage.ReturnMessage(flight.ErrorCode));
            else if (flight == null)
                return NotFound();
            else
                _flightService.Remove(flight.Id);

            return NoContent();
        }
    }
}
