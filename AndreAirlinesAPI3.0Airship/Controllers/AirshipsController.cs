using AndreAirlinesAPI3._0Airship.Service;
using AndreAirlinesAPI3._0ErrorMessages;
using AndreAirlinesAPI3._0Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0Airship.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirshipsController : ControllerBase
    {
        private readonly AirshipService _airshipService;

        public AirshipsController(AirshipService airshipService)
        {
            _airshipService = airshipService;
        }

        [HttpGet]
        public ActionResult<List<Airship>> Get()
        {
            var airships = _airshipService.Get();

            if (airships[0].ErrorCode != null)
                return BadRequest("Aeronave - " + ErrorMessage.ReturnMessage(airships[0].ErrorCode));

            return airships;

        }

        [HttpGet("{id}", Name = "GetAirship")]
        public ActionResult<Airship> Get(string id)
        {
            var airship = _airshipService.Get(id);

            if (airship.ErrorCode != null)
                return BadRequest("Aeronave - " + ErrorMessage.ReturnMessage(airship.ErrorCode));
            else if (airship == null)
                return NotFound();

            return airship;
        }

        [HttpPost]
        public ActionResult<Airship> Create(Airship airship)
        {
            var airshipInsertion = _airshipService.Create(airship);

            if (airshipInsertion.ErrorCode != null)
                return BadRequest("Aeronave - " + ErrorMessage.ReturnMessage(airshipInsertion.ErrorCode));

            return CreatedAtRoute("GetAirship", new { id = airship.Id }, airship);

        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, Airship airshipIn)
        {
            var airship = _airshipService.Get(id);

            if (airship.ErrorCode != null)
                return BadRequest("Aeronave - " + ErrorMessage.ReturnMessage(airship.ErrorCode));
            else if (airship == null)
                return NotFound();
            else
                _airshipService.Update(id, airshipIn);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var airship = _airshipService.Get(id);

            if (airship.ErrorCode != null)
                return BadRequest("Aeronave - " + ErrorMessage.ReturnMessage(airship.ErrorCode));
            else if (airship == null)
                return NotFound();
            else
                _airshipService.Remove(airship.Id);

            return NoContent();
        }
    }
}
