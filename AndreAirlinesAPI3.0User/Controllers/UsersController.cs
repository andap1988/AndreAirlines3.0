using AndreAirlinesAPI3._0ErrorMessages;
using AndreAirlinesAPI3._0Models;
using AndreAirlinesAPI3._0SearchZipcode;
using AndreAirlinesAPI3._0User.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0User.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        public bool utilizationSearchZipcode = true;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public ActionResult<List<User>> Get()
        {
            var users = _userService.Get();

            if (users[0].ErrorCode != null)
                return BadRequest("Usuário - " + ErrorMessage.ReturnMessage(users[0].ErrorCode));

            return users;
        }


        [HttpGet("{id}", Name = "GetUser")]
        public ActionResult<User> Get(string id)
        {
            var user = _userService.Get(id);

            if (user.ErrorCode != null)
                return BadRequest("Usuário - " + ErrorMessage.ReturnMessage(user.ErrorCode));
            else if (user == null)
                return NotFound();

            return user;
        }

        [HttpGet("loginUser/{loginUser}")]
        public ActionResult<User> GetLoginUser(string loginUser)
        {
            var user = _userService.GetLoginUser(loginUser);

            if (user.ErrorCode != null)
                return BadRequest("Usuário - " + ErrorMessage.ReturnMessage(user.ErrorCode));
            else if (user == null)
                return NotFound();

            return user;
        }

        [HttpPost]
        public async Task<ActionResult<User>> Create(User user)
        {
            if (utilizationSearchZipcode)
            {
                Address address = await SearchZipcode.ReturnAddress(user.Address);

                if (address.ErrorCode != null)
                    return BadRequest("Endereço - " + ErrorMessage.ReturnMessage(address.ErrorCode));
                else
                    user.Address = address;
            }

            var userInsertion = await _userService.Create(user);

            if (userInsertion.ErrorCode == "noLog")
                return BadRequest("Log - " + ErrorMessage.ReturnMessage("noLog"));
            else if (userInsertion.ErrorCode != null)
                return BadRequest("Usuário - " + ErrorMessage.ReturnMessage(userInsertion.ErrorCode));
            else
                return CreatedAtRoute("GetUser", new { id = user.Id }, user);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, User userIn)
        {
            User userLogin = new();
            string returnMsg;

            userLogin = _userService.GetLoginUser(userIn.LoginUser);

            if (userLogin.LoginUser == null)
                return BadRequest("Usuário - " + ErrorMessage.ReturnMessage("noBlank"));
            else if (userLogin.ErrorCode != null)
                return BadRequest("Usuário - " + ErrorMessage.ReturnMessage(userLogin.ErrorCode));
            else if (userLogin.Sector != "ADM")
                return BadRequest("Usuário - " + ErrorMessage.ReturnMessage("noPermited"));
            else
                returnMsg = await _userService.Update(id, userIn, userLogin);

            if (returnMsg != "ok")
                return BadRequest("Log - " + ErrorMessage.ReturnMessage("noLog"));

            return Ok("Usuário atualizado com sucesso. Log gravado com sucesso.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, User userIn)
        {
            User userLogin = new();
            string returnMsg;

            userLogin = _userService.GetLoginUser(userIn.LoginUser);

            if (userLogin.LoginUser == null)
                return BadRequest("Usuário - " + ErrorMessage.ReturnMessage("noBlank"));
            else if (userLogin.ErrorCode != null)
                return BadRequest("Usuário - " + ErrorMessage.ReturnMessage(userLogin.ErrorCode));
            else if (userLogin.Sector != "ADM")
                return BadRequest("Usuário - " + ErrorMessage.ReturnMessage("noPermited"));
            else
                returnMsg = await _userService.Remove(id, userIn, userLogin);

            if (returnMsg != "ok")
                return BadRequest("Log - " + ErrorMessage.ReturnMessage("noLog"));

            return Ok("Usuário excluído com sucesso. Log gravado com sucesso.");
        }
    }
}
