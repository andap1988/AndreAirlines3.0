using AndreAirlinesAPI3._0Class.Service;
using AndreAirlinesAPI3._0ErrorMessages;
using AndreAirlinesAPI3._0Models;
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

            if (classs.ErrorCode != null)
                return BadRequest("Classe - " + ErrorMessage.ReturnMessage(classs.ErrorCode));
            else if (classs == null)
                return NotFound();

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

            if (classs.ErrorCode != null)
                return BadRequest("Classe - " + ErrorMessage.ReturnMessage(classs.ErrorCode));
            else if (classs == null)
                return NotFound();
            else
                _classService.Update(id, classIn);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var classs = _classService.Get(id);

            if (classs.ErrorCode != null)
                return BadRequest("Classe - " + ErrorMessage.ReturnMessage(classs.ErrorCode));
            else if (classs == null)
                return NotFound();
            else
                _classService.Remove(classs.Id);

            return NoContent();
        }
    }
}
