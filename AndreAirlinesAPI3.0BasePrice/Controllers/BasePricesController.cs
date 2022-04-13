using AndreAirlinesAPI3._0BasePrice.Service;
using AndreAirlinesAPI3._0ErrorMessages;
using AndreAirlinesAPI3._0Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
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
        [Authorize]
        public ActionResult<List<BasePrice>> Get()
        {
            var basePrice = _basePriceService.Get();

            if (basePrice[0].ErrorCode != null)
                return BadRequest("Preço Base - " + ErrorMessage.ReturnMessage(basePrice[0].ErrorCode));

            return basePrice;
        }

        [HttpGet("{id}", Name = "GetBasePrice")]
        [Authorize]
        public ActionResult<BasePrice> Get(string id)
        {
            var baseprice = _basePriceService.Get(id);

            if (baseprice == null)
                return NotFound();
            else if (baseprice.ErrorCode != null)
                return BadRequest("Preço Base - " + ErrorMessage.ReturnMessage(baseprice.ErrorCode));

            return baseprice;
        }

        [HttpPost]
        [Authorize(Roles = "adm")]
        public async Task<ActionResult<BasePrice>> Create(BasePrice basePrice)
        {
            var user = User.Identity.Name;
            var token = Request.Headers[HeaderNames.Authorization].ToString().Split(" ")[1];

            var basePriceInsertion = await _basePriceService.Create(basePrice, user, token);

            if (basePriceInsertion.ErrorCode == "noLog")
                return BadRequest("Log - " + ErrorMessage.ReturnMessage("noLog"));
            else if (basePriceInsertion.ErrorCode != null)
                return BadRequest("Usuário - " + ErrorMessage.ReturnMessage(basePriceInsertion.ErrorCode));
            if (basePriceInsertion.Origin.ErrorCode != null)
                return BadRequest("Aeroporto de Origem - " + ErrorMessage.ReturnMessage(basePriceInsertion.Origin.ErrorCode));
            else if (basePriceInsertion.Destiny.ErrorCode != null)
                return BadRequest("Aeroporto de Destino - " + ErrorMessage.ReturnMessage(basePriceInsertion.Destiny.ErrorCode));

            return CreatedAtRoute("GetBasePrice", new { id = basePrice.Id }, basePrice);

        }

        [HttpPut("{id}")]
        [Authorize(Roles = "adm")]
        public async Task<IActionResult> Update(string id, BasePrice basePriceIn)
        {
            BasePrice basePrice = new();
            string returnMsg;
            var user = User.Identity.Name;
            var token = Request.Headers[HeaderNames.Authorization].ToString().Split(" ")[1];

            basePrice = _basePriceService.Get(id);

            if (basePrice == null)
                return NotFound();
            else if (basePrice.ErrorCode != null)
                return BadRequest("Preço Base - " + ErrorMessage.ReturnMessage(basePrice.ErrorCode));
            else
                returnMsg = await _basePriceService.Update(id, basePriceIn, user, token);

            if (returnMsg == "noUser")
                return BadRequest("Log - " + ErrorMessage.ReturnMessage("noUser"));
            else if (returnMsg != "ok")
                return BadRequest("Log - " + ErrorMessage.ReturnMessage("noLog"));

            return Ok("Preço base atualizado com sucesso. Log gravado com sucesso.");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "adm")]
        public async Task<IActionResult> Delete(string id, BasePrice basePriceIn)
        {
            BasePrice basePrice = new();
            string returnMsg;
            var user = User.Identity.Name;
            var token = Request.Headers[HeaderNames.Authorization].ToString().Split(" ")[1];

            basePrice = _basePriceService.Get(id);

            if (basePrice == null)
                return NotFound();
            else if (basePrice.ErrorCode != null)
                return BadRequest("Preço Base - " + ErrorMessage.ReturnMessage(basePrice.ErrorCode));
            else
                returnMsg = await _basePriceService.Remove(basePrice.Id, basePrice, user, token);

            if (returnMsg == "noUser")
                return BadRequest("Log - " + ErrorMessage.ReturnMessage("noUser"));
            else if (returnMsg != "ok")
                return BadRequest("Log - " + ErrorMessage.ReturnMessage("noLog"));

            return Ok("Preço base excluído com sucesso. Log gravado com sucesso.");
        }
    }
}
