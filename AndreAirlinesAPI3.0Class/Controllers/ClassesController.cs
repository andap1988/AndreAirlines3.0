using AndreAirlinesAPI3._0Class.Service;
using AndreAirlinesAPI3._0ErrorMessages;
using AndreAirlinesAPI3._0Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0Class.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly ClassService _classService;

        public ClassesController(ClassService classService)
        {
            _classService = classService;
        }

/*        [HttpPost]
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
        }*/

        [HttpGet]
        public ActionResult<List<Class>> Get()
        {
            var classes = _classService.Get();

            if (classes[0].ErrorCode != null)
                return BadRequest("Classe - " + ErrorMessage.ReturnMessage(classes[0].ErrorCode));

            return classes;
        }

        [HttpGet("{id}", Name = "GetClass")]
        public ActionResult<Class> Get(string id)
        {
            var classs = _classService.Get(id);

            if (classs == null)
                return NotFound();
            else if (classs.ErrorCode != null)
                return BadRequest("Classe - " + ErrorMessage.ReturnMessage(classs.ErrorCode));

            return classs;
        }

        [HttpPost]
        public ActionResult<Class> Create(Class classs)
        {
            var classInsertion = _classService.Create(classs);

            if (classInsertion.ErrorCode != null)
                return BadRequest("Classe - " + ErrorMessage.ReturnMessage(classInsertion.ErrorCode));
            else
                return CreatedAtRoute("GetClass", new { id = classs.Id }, classs);

        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, Class classIn)
        {
            var classs = _classService.Get(id);

            if (classs == null)
                return NotFound();
            else if (classs.ErrorCode != null)
                return BadRequest("Classe - " + ErrorMessage.ReturnMessage(classs.ErrorCode));
            else
                _classService.Update(id, classIn);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var classs = _classService.Get(id);

            if (classs == null)
                return NotFound();
            else if (classs.ErrorCode != null)
                return BadRequest("Classe - " + ErrorMessage.ReturnMessage(classs.ErrorCode));
            else
                _classService.Remove(classs.Id);

            return NoContent();
        }
    }
}
