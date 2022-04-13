using AndreAirlinesAPI3._0LogToMongoDB.Service;
using AndreAirlinesAPI3._0Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0LogToMongoDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsToMongoController : ControllerBase
    {
        private readonly LogToMongoService _logtoMongoService;

        public LogsToMongoController(LogToMongoService logtoMongoService)
        {
            _logtoMongoService = logtoMongoService;
        }

        [HttpGet]
        public ActionResult<List<Log>> GetLog()
        {
            var logs = _logtoMongoService.GetLog();

            if (logs[0].ErrorCode != null)
                return BadRequest("Log - Erro");

            return logs;
        }

        [HttpGet("{id}", Name = "GetLog")]
        public ActionResult<Log> GetLog(string id)
        {
            var log = _logtoMongoService.GetLog(id);

            if (log == null || log.ErrorCode != null)
                return NotFound();

            return log;
        }

        [HttpPost]
        public async Task<ActionResult<Log>> Create(Log log)
        {
            var logInsertion = await _logtoMongoService.Create(log);

            if (logInsertion.ErrorCode != null)
                return BadRequest("Log - Erro");

            return CreatedAtRoute("GetLog", new { id = log.Id }, log);

        }
    }
}
