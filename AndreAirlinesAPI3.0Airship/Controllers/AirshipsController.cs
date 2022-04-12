using AndreAirlinesAPI3._0Airship.Service;
using AndreAirlinesAPI3._0ErrorMessages;
using AndreAirlinesAPI3._0Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
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

            if (airship == null)
                return NotFound();
            else if (airship.ErrorCode != null)
                return BadRequest("Aeronave - " + ErrorMessage.ReturnMessage(airship.ErrorCode));

            return airship;
        }

        [HttpGet("registration/{registration}")]
        [Authorize(Roles = "adm")]
        public ActionResult<Airship> GetRegistration(string registration)
        {
            var airship = _airshipService.GetRegistration(registration.ToUpper());

            if (airship == null)
                return NotFound();
            else if (airship.ErrorCode != null)
                return BadRequest("Aeroporto - " + ErrorMessage.ReturnMessage(airship.ErrorCode));

            return airship;
        }

        [HttpPost]
        [Authorize(Roles = "adm")]
        public async Task<ActionResult<Airship>> Create(Airship airship)
        {
            var user = User.Identity.Name;
            var token = Request.Headers[HeaderNames.Authorization].ToString().Split(" ")[1];

            var airshipInsertion = await _airshipService.Create(airship, user);

            if (airshipInsertion.ErrorCode == "noLog")
                return BadRequest("Log - " + ErrorMessage.ReturnMessage("noLog"));
            else if (airshipInsertion.ErrorCode != null)
                return BadRequest("Aeronave - " + ErrorMessage.ReturnMessage(airshipInsertion.ErrorCode));

            return CreatedAtRoute("GetAirship", new { id = airship.Id }, airship);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "adm")]
        public async Task<IActionResult> Update(string id, Airship airshipIn)
        {
            Airship airship = new();
            string returnMsg;
            var user = User.Identity.Name;

            airship = _airshipService.Get(id);

            if (airship == null)
                return NotFound();
            else if (airship.ErrorCode != null)
                return BadRequest("Aeronave - " + ErrorMessage.ReturnMessage(airship.ErrorCode));
            else
                returnMsg = await _airshipService.Update(id, airshipIn, user);

            if (returnMsg == "noUser")
                return BadRequest("Log - " + ErrorMessage.ReturnMessage("noUser"));
            else if (returnMsg != "ok")
                return BadRequest("Log - " + ErrorMessage.ReturnMessage("noLog"));

            return Ok("Aeronave atualizada com sucesso. Log gravado com sucesso.");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "adm")]
        public async Task<IActionResult> Delete(string id, Airship airshipIn)
        {
            Airship airship = new();
            string returnMsg;
            var user = User.Identity.Name;

            airship = _airshipService.Get(id);

            if (airship == null)
                return NotFound();
            else if (airship.ErrorCode != null)
                return BadRequest("Aeronave - " + ErrorMessage.ReturnMessage(airship.ErrorCode));
            else
                returnMsg = await _airshipService.Remove(airship.Id, airship, user);

            if (returnMsg == "noUser")
                return BadRequest("Log - " + ErrorMessage.ReturnMessage("noUser"));
            else if (returnMsg != "ok")
                return BadRequest("Log - " + ErrorMessage.ReturnMessage("noLog"));

            return Ok("Aeronave excluído com sucesso. Log gravado com sucesso.");
        }
    }
}
