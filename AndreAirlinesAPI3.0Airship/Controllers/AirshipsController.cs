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
        public async Task<ActionResult<Airship>> Create(Airship airship)
        {
            var airshipInsertion = await _airshipService.Create(airship);

            if (airshipInsertion.ErrorCode == "noLog")
                return BadRequest("Log - " + ErrorMessage.ReturnMessage("noLog"));
            else if (airshipInsertion.ErrorCode != null)
                return BadRequest("Aeronave - " + ErrorMessage.ReturnMessage(airshipInsertion.ErrorCode));

            return CreatedAtRoute("GetAirship", new { id = airship.Id }, airship);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Airship airshipIn)
        {
            Airship airship = new();
            string returnMsg;

            var user = await SearchUser.ReturnUser(airshipIn.LoginUser);

            if (user.LoginUser == null)
                return BadRequest("Aeronave - " + ErrorMessage.ReturnMessage("noBlank"));
            if (user.ErrorCode != null)
                return BadRequest("Aeronave - " + ErrorMessage.ReturnMessage(user.ErrorCode));
            else if (user.Sector != "ADM")
                return BadRequest("Aeronave - " + ErrorMessage.ReturnMessage("noPermited"));
            else
                airship = _airshipService.Get(id);

            if (airship.ErrorCode != null)
                return BadRequest("Aeronave - " + ErrorMessage.ReturnMessage(airship.ErrorCode));
            else if (airship == null)
                return NotFound();
            else
                returnMsg = await _airshipService.Update(id, airshipIn, user);

            if (returnMsg != "ok")
                return BadRequest("Log - " + ErrorMessage.ReturnMessage("noLog"));

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
