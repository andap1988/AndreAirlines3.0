using AndreAirlinesAPI3._0Dapper.Repository;
using AndreAirlinesAPI3._0ErrorMessages;
using AndreAirlinesAPI3._0Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AndreAirlinesAPI3._0Dapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AiportsDataController : ControllerBase
    {
        private readonly IAirportRepository _airportRepository;

        public AiportsDataController(IAirportRepository airportRepository)
        {
            _airportRepository = airportRepository;
        }

        [HttpGet]
        public ActionResult<List<AirportData>> Get()
        {
            var airportsData = _airportRepository.GetAll();

            if (airportsData[0].ErrorCode != null)
                return BadRequest("Aeroporto - " + ErrorMessage.ReturnMessage(airportsData[0].ErrorCode));

            return airportsData;
        }

        [HttpGet("{id}")]
        public ActionResult<AirportData> Get(int id)
        {
            var airport = _airportRepository.GetOne(id);

            if (airport == null)
                return NotFound();
            else if (airport.ErrorCode != null)
                return BadRequest("Aeroporto - " + ErrorMessage.ReturnMessage(airport.ErrorCode));

            return airport;
        }

        [HttpGet("code/{iatacode}")]
        public ActionResult<AirportData> GetAiportData(string iatacode)
        {
            var airport = _airportRepository.GetAiportData(iatacode);

            if (airport == null)
                return NotFound();
            else if (airport.ErrorCode != null)
                return BadRequest("Aeroporto - " + ErrorMessage.ReturnMessage(airport.ErrorCode));

            return airport;
        }
    }
}
