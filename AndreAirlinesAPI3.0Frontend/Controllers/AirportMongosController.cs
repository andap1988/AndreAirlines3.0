using AndreAirlinesAPI3._0Frontend.Service;
using Microsoft.AspNetCore.Mvc;

namespace AndreAirlinesAPI3._0Frontend.Controllers
{
    public class AirportMongosController : Controller
    {

        private readonly AirportService _airportService;

        public AirportMongosController(AirportService airportService)
        {
            _airportService = airportService;
        }

        public IActionResult Index()
        {
            return View(_airportService.Get());
        }
    }
}
