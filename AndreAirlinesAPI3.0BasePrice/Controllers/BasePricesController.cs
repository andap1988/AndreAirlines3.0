using AndreAirlinesAPI3._0BasePrice.Service;
using AndreAirlinesAPI3._0ErrorMessages;
using AndreAirlinesAPI3._0Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0BasePrice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasePricesController : ControllerBase
    {
        private readonly BasePriceService _basePriceService;

        public BasePricesController(BasePriceService basePriceService)
        {
            _basePriceService = basePriceService;
        }

        [HttpGet]
        public ActionResult<List<BasePrice>> Get()
        {
            var basePrice = _basePriceService.Get();

            if (basePrice[0].ErrorCode != null)
                return BadRequest("Preço Base - " + ErrorMessage.ReturnMessage(basePrice[0].ErrorCode));

            return basePrice;
        }

        [HttpGet("{id}", Name = "GetBasePrice")]
        public ActionResult<BasePrice> Get(string id)
        {
            var baseprice = _basePriceService.Get(id);

            if (baseprice.ErrorCode != null)
                return BadRequest("Preço Base - " + ErrorMessage.ReturnMessage(baseprice.ErrorCode));
            else if (baseprice == null)
                return NotFound();

            return baseprice;
        }

        [HttpPost]
        public async Task<ActionResult<BasePrice>> Create(BasePrice basePrice)
        {
            var basePriceInsertion = await _basePriceService.Create(basePrice);

            if (basePriceInsertion.Origin.ErrorCode != null)
                return BadRequest("Aeroporto de Origem - " + ErrorMessage.ReturnMessage(basePriceInsertion.Origin.ErrorCode));
            else if (basePriceInsertion.Destiny.ErrorCode != null)
                return BadRequest("Aeroporto de Destino - " + ErrorMessage.ReturnMessage(basePriceInsertion.Destiny.ErrorCode));
            else
                return CreatedAtRoute("GetBasePrice", new { id = basePrice.Id }, basePrice);

        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, BasePrice basePriceIn)
        {
            var basePrice = _basePriceService.Get(id);

            if (basePrice.ErrorCode != null)
                return BadRequest("Preço Base - " + ErrorMessage.ReturnMessage(basePrice.ErrorCode));
            else if (basePrice == null)
                return NotFound();
            else
                _basePriceService.Update(id, basePriceIn);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var basePrice = _basePriceService.Get(id);

            if (basePrice.ErrorCode != null)
                return BadRequest("Preço Base - " + ErrorMessage.ReturnMessage(basePrice.ErrorCode));
            else if (basePrice == null)
                return NotFound();
            else
                _basePriceService.Remove(basePrice.Id);

            return NoContent();
        }
    }
}
