﻿using AndreAirlinesAPI3._0ErrorMessages;
using AndreAirlinesAPI3._0Models;
using AndreAirlinesAPI3._0Ticket.Service;
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

            if (ticket.ErrorCode != null)
                return BadRequest("Reserva - " + ErrorMessage.ReturnMessage(ticket.ErrorCode));
            else if (ticket == null)
                return NotFound();

            return ticket;
        }

        [HttpPost]
        public async Task<ActionResult<Ticket>> Create(Ticket ticket)
        {
            var ticketInsertion = await _ticketService.Create(ticket);

            if (ticketInsertion.Flight.ErrorCode != null)
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
        public IActionResult Update(string id, Ticket ticketIn)
        {
            var ticket = _ticketService.Get(id);

            if (ticket.ErrorCode != null)
                return BadRequest("Reserva - " + ErrorMessage.ReturnMessage(ticket.ErrorCode));
            else if (ticket == null)
                return NotFound();
            else
                _ticketService.Update(id, ticketIn);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var ticket = _ticketService.Get(id);

            if (ticket.ErrorCode != null)
                return BadRequest("Reserva - " + ErrorMessage.ReturnMessage(ticket.ErrorCode));
            else if (ticket == null)
                return NotFound();
            else
                _ticketService.Remove(ticket.Id);

            return NoContent();
        }
    }
}