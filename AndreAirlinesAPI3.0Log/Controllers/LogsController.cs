using AndreAirlinesAPI3._0ErrorMessages;
using AndreAirlinesAPI3._0Log.Service;
using AndreAirlinesAPI3._0Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0Log.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly ConnectionFactory _factory;
        private const string QUEUE_NAME = "queuelogstomongo";

        public LogsController()
        {
            _factory = new ConnectionFactory
            {
                HostName = "localhost",
            };
        }

        [HttpPost]
        public IActionResult PostMessage([FromBody] Log log)
        {
            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: QUEUE_NAME,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                        );
                    var stringFieldMessage = JsonConvert.SerializeObject(log);
                    var byteMessage = Encoding.UTF8.GetBytes(stringFieldMessage);

                    channel.BasicPublish(
                        exchange: "",
                        routingKey: QUEUE_NAME,
                        basicProperties: null,
                        body: byteMessage
                        );
                }
            }
            return Accepted();
        }


        /*
        private readonly LogService _logService;

        public LogsController(LogService logService)
        {
            _logService = logService;
        }

        [HttpGet]
        public ActionResult<List<Log>> Get()
        {
            var logs = _logService.Get();

            if (logs[0].ErrorCode != null)
                return BadRequest("Log - " + ErrorMessage.ReturnMessage(logs[0].ErrorCode));

            return logs;
        }

        [HttpGet("{id}", Name = "GetLog")]
        public ActionResult<Log> Get(string id)
        {
            var airport = _logService.Get(id);

            if (airport == null)
                return NotFound();
            else if (airport.ErrorCode != null)
                return BadRequest("Log - " + ErrorMessage.ReturnMessage(airport.ErrorCode));

            return airport;
        }

        [HttpPost]
        public async Task<ActionResult<Log>> Create(Log log)
        {
            var logInsertion = await _logService.Create(log);

            if (logInsertion.ErrorCode != null)
                return BadRequest("Log - " + ErrorMessage.ReturnMessage(logInsertion.ErrorCode));

            return CreatedAtRoute("GetLog", new { id = log.Id }, log);

        }
        */
    }
}
