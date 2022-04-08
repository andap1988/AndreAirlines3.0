using AndreAirlinesAPI3._0ErrorMessages;
using AndreAirlinesAPI3._0Models;
using AndreAirlinesAPI3._0Passenger.Service;
using AndreAirlinesAPI3._0SearchZipcode;
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

            if (passenger.ErrorCode != null)
                return BadRequest("Passageiro - " + ErrorMessage.ReturnMessage(passenger.ErrorCode));
            else if (passenger == null)
                return NotFound();

            return passenger;
        }

        [HttpPost]
        public async Task<ActionResult<Passenger>> Create(Passenger passenger)
        {
            if (utilizationSearchZipcode)
            {
                Address address = await SearchZipcode.ReturnAddress(passenger.Address);

                if (address.ErrorCode != null)
                    return BadRequest("Endereço - " + ErrorMessage.ReturnMessage(address.ErrorCode));
                else
                    passenger.Address = address;
            }

            var passengerInsertion = await _passengerService.Create(passenger);

            if (passengerInsertion.ErrorCode == "noLog")
                return BadRequest("Log - " + ErrorMessage.ReturnMessage("noLog"));
            else if (passengerInsertion.ErrorCode != null)
                return BadRequest("Passageiro - " + ErrorMessage.ReturnMessage(passengerInsertion.ErrorCode));

            return CreatedAtRoute("GetPassenger", new { id = passenger.Id }, passenger);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Passenger passengerIn)
        {
            Passenger passenger = new();
            string returnMsg;

            var user = await SearchUser.ReturnUser(passengerIn.LoginUser);

            if (user.LoginUser == null)
                return BadRequest("Passageiro - " + ErrorMessage.ReturnMessage("noBlank"));
            if (user.ErrorCode != null)
                return BadRequest("Passageiro - " + ErrorMessage.ReturnMessage(user.ErrorCode));
            else if (user.Sector != "ADM")
                return BadRequest("Passageiro - " + ErrorMessage.ReturnMessage("noPermited"));
            else
                passenger = _passengerService.Get(id);

            if (passenger.ErrorCode != null)
                return BadRequest("Passageiro - " + ErrorMessage.ReturnMessage(passenger.ErrorCode));
            else if (passenger == null)
                return NotFound();
            else
                returnMsg = await _passengerService.Update(id, passengerIn, user);

            if (returnMsg != "ok")
                return BadRequest("Log - " + ErrorMessage.ReturnMessage("noLog"));

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var passenger = _passengerService.Get(id);

            if (passenger.ErrorCode != null)
                return BadRequest("Passageiro - " + ErrorMessage.ReturnMessage(passenger.ErrorCode));
            else if (passenger == null)
                return NotFound();
            else
                _passengerService.Remove(passenger.Id);

            return NoContent();
        }
    }
}
