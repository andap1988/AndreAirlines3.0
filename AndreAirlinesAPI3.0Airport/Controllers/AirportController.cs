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
            if (utilizationSearchZipcode)
            {
                Address address = await SearchZipcode.ReturnZipcode(airport.Address);

                if (address.ErrorCode != null)
                    return BadRequest("Endereço - " + ErrorMessage.ReturnMessage(address.ErrorCode));
                else
                    airport.Address = address;
            }

            var airportInsertion = await _airportService.Create(airport);

            if (airportInsertion.ErrorCode != null)
                return BadRequest("Aeroporto - " + ErrorMessage.ReturnMessage(airportInsertion.ErrorCode));

            return CreatedAtRoute("GetAirport", new { id = airport.Id }, airport);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Airport airportIn)
        {
            Airport airport = new();

            var user = await SearchUser.ReturnUser(airportIn.LoginUser);

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
                _airportService.Update(id, airportIn);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var airport = _airportService.Get(id);

            if (airport.ErrorCode != null)
                return BadRequest("Aeroporto - " + ErrorMessage.ReturnMessage(airport.ErrorCode));
            else if (airport == null)
                return NotFound();
            else
                _airportService.Remove(airport.Id);

            return NoContent();
        }
    }
}
