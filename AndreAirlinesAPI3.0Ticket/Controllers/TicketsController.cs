using AndreAirlinesAPI3._0ErrorMessages;
using AndreAirlinesAPI3._0Models;
using AndreAirlinesAPI3._0Ticket.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0Ticket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly TicketService _ticketService;

        public TicketsController(TicketService ticketService)
        {
            _ticketService = ticketService;
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
        public ActionResult<List<Ticket>> Get()
        {
            var tickets = _ticketService.Get();

            if (tickets[0].ErrorCode != null)
                return BadRequest("Reserva - " + ErrorMessage.ReturnMessage(tickets[0].ErrorCode));

            return tickets;
        }


        [HttpGet("{id}", Name = "GetTicket")]
        public ActionResult<Ticket> Get(string id)
        {
            var ticket = _ticketService.Get(id);

            if (ticket == null)
                return NotFound();
            else if (ticket.ErrorCode != null)
                return BadRequest("Reserva - " + ErrorMessage.ReturnMessage(ticket.ErrorCode));

            return ticket;
        }

        [HttpPost]
        [Authorize(Roles = "adm,user")]
        public async Task<ActionResult<Ticket>> Create(Ticket ticket)
        {
            var user = User.Identity.Name;
            var ticketInsertion = await _ticketService.Create(ticket, user);

            if (ticketInsertion.ErrorCode == "noLog")
                return BadRequest("Log - " + ErrorMessage.ReturnMessage("noLog"));
            else if (ticketInsertion.ErrorCode != null)
                return BadRequest("Usuário - " + ErrorMessage.ReturnMessage(ticketInsertion.Flight.ErrorCode));
            else if (ticketInsertion.Flight.ErrorCode != null)
                return BadRequest("Voo - " + ErrorMessage.ReturnMessage(ticketInsertion.Flight.ErrorCode));
            else if (ticketInsertion.Passenger.ErrorCode != null)
                return BadRequest("Passageiro - " + ErrorMessage.ReturnMessage(ticketInsertion.Passenger.ErrorCode));
            else if (ticketInsertion.BasePrice.ErrorCode != null)
                return BadRequest("Base de Preço - " + ErrorMessage.ReturnMessage(ticketInsertion.BasePrice.ErrorCode));
            else if (ticketInsertion.Class.ErrorCode != null)
                return BadRequest("Classe - " + ErrorMessage.ReturnMessage(ticketInsertion.Class.ErrorCode));
            else
                return CreatedAtRoute("GetTicket", new { id = ticket.Id }, ticket);

        }

        [HttpPut("{id}")]
        [Authorize(Roles = "adm,user")]
        public async Task<IActionResult> Update(string id, Ticket ticketIn)
        {
            Ticket ticket = new();
            string returnMsg;
            var user = User.Identity.Name;

            ticket = _ticketService.Get(id);

            if (ticket == null)
                return NotFound();
            else if (ticket.ErrorCode != null)
                return BadRequest("Reserva - " + ErrorMessage.ReturnMessage(ticket.ErrorCode));
            else
                returnMsg = await _ticketService.Update(id, ticketIn, user);

            if (returnMsg != "ok")
                return BadRequest("Log - " + ErrorMessage.ReturnMessage("noLog"));

            return Ok("Reserva atualizada com sucesso. Log gravado com sucesso.");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "adm,user")]
        public async Task<IActionResult> Delete(string id, Ticket ticketIn)
        {
            Ticket ticket = new();
            string returnMsg;
            var user = User.Identity.Name;

            ticket = _ticketService.Get(id);

            if (ticket == null)
                return NotFound();
            else if (ticket.ErrorCode != null)
                return BadRequest("Reserva - " + ErrorMessage.ReturnMessage(ticket.ErrorCode));
            else
                returnMsg = await _ticketService.Remove(ticket.Id, ticketIn, user);

            if (returnMsg != "ok")
                return BadRequest("Log - " + ErrorMessage.ReturnMessage("noLog"));

            return Ok("Reserva excluída com sucesso. Log gravado com sucesso.");
        }
    }
}
