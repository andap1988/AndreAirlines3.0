using AndreAirlinesAPI3._0ErrorMessages;
using AndreAirlinesAPI3._0Models;
using AndreAirlinesAPI3._0Passenger.Service;
using AndreAirlinesAPI3._0SearchZipcode;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0Passenger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassengersController : ControllerBase
    {
        private readonly PassengerService _passengerService;
        public bool utilizationSearchZipcode = true;

        public PassengersController(PassengerService passengerService)
        {
            _passengerService = passengerService;
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
        public ActionResult<List<Passenger>> Get()
        {
            var passengers = _passengerService.Get();

            if (passengers[0].ErrorCode != null)
                return BadRequest("Passageiro - " + ErrorMessage.ReturnMessage(passengers[0].ErrorCode));

            return passengers;
        }


        [HttpGet("{id}", Name = "GetPassenger")]
        public ActionResult<Passenger> Get(string id)
        {
            var passenger = _passengerService.Get(id);

            if (passenger == null)
                return NotFound();
            else if (passenger.ErrorCode != null)
                return BadRequest("Passageiro - " + ErrorMessage.ReturnMessage(passenger.ErrorCode));

            return passenger;
        }

        [HttpGet("cpf/{cpf}")]
        public ActionResult<Passenger> GetCpf(string cpf)
        {
            var passenger = _passengerService.GetCpf(cpf.Replace(".", "").Replace("-", ""));

            if (passenger == null)
                return NotFound();
            else if (passenger.ErrorCode != null)
                return BadRequest("Passageiro - " + ErrorMessage.ReturnMessage(passenger.ErrorCode));

            return passenger;
        }

        [HttpPost]
        [Authorize(Roles = "adm,user")]
        public async Task<ActionResult<Passenger>> Create(Passenger passenger)
        {
            var user = User.Identity.Name;

            if (utilizationSearchZipcode)
            {
                Address address = await SearchZipcode.ReturnAddress(passenger.Address);

                if (address.ErrorCode != null)
                    return BadRequest("Endereço - " + ErrorMessage.ReturnMessage(address.ErrorCode));
                else
                    passenger.Address = address;
            }

            passenger.Cpf = passenger.Cpf.Replace(".", "").Replace("-", "");

            var passengerInsertion = await _passengerService.Create(passenger, user);

            if (passengerInsertion.ErrorCode == "noLog")
                return BadRequest("Log - " + ErrorMessage.ReturnMessage("noLog"));
            else if (passengerInsertion.ErrorCode != null)
                return BadRequest("Passageiro - " + ErrorMessage.ReturnMessage(passengerInsertion.ErrorCode));

            return CreatedAtRoute("GetPassenger", new { id = passenger.Id }, passenger);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "adm,user")]
        public async Task<IActionResult> Update(string id, Passenger passengerIn)
        {
            Passenger passenger = new();
            string returnMsg;

            var user = await SearchUser.ReturnUser(passengerIn.LoginUser);

            if (user.LoginUser == null)
                return BadRequest("Passageiro - " + ErrorMessage.ReturnMessage("noBlank"));
            if (user.ErrorCode != null)
                return BadRequest("Passageiro - " + ErrorMessage.ReturnMessage(user.ErrorCode));
            else if (user.Role != "ADM" && user.Role != "USER")
                return BadRequest("Passageiro - " + ErrorMessage.ReturnMessage("noPermited"));
            else
                passenger = _passengerService.Get(id);

            if (passenger == null)
                return NotFound();
            else if (passenger.ErrorCode != null)
                return BadRequest("Passageiro - " + ErrorMessage.ReturnMessage(passenger.ErrorCode));
            else
                returnMsg = await _passengerService.Update(id, passengerIn, user);

            if (returnMsg != "ok")
                return BadRequest("Log - " + ErrorMessage.ReturnMessage("noLog"));

            return Ok("Passageiro atualizado com sucesso. Log gravado com sucesso.");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "adm,user")]
        public async Task<IActionResult> Delete(string id, Passenger passengerIn)
        {
            Passenger passenger = new();
            string returnMsg;

            var user = await SearchUser.ReturnUser(passengerIn.LoginUser);

            if (user.LoginUser == null)
                return BadRequest("Passageiro - " + ErrorMessage.ReturnMessage("noBlank"));
            if (user.ErrorCode != null)
                return BadRequest("Passageiro - " + ErrorMessage.ReturnMessage(user.ErrorCode));
            else if (user.Role != "ADM" && user.Role != "USER")
                return BadRequest("Passageiro - " + ErrorMessage.ReturnMessage("noPermited"));
            else
                passenger = _passengerService.Get(id);

            if (passenger == null)
                return NotFound();
            else if (passenger.ErrorCode != null)
                return BadRequest("Passageiro - " + ErrorMessage.ReturnMessage(passenger.ErrorCode));
            else
                returnMsg = await _passengerService.Remove(passenger.Id, passengerIn, user);

            if (returnMsg != "ok")
                return BadRequest("Log - " + ErrorMessage.ReturnMessage("noLog"));

            return Ok("Passageiro excluído com sucesso. Log gravado com sucesso.");
        }
    }
}
