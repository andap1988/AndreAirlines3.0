using AndreAirlinesAPI3._0ErrorMessages;
using AndreAirlinesAPI3._0Log.Service;
using AndreAirlinesAPI3._0Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0Log.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
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
    }
}
