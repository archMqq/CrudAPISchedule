using Microsoft.AspNetCore.Mvc;
using WebDbApi.Services.Services;
using Schedule.Models.Models;

namespace WebDbApi.Controllers
{
    [ApiController]
    [Route("class")]
    public class ClassController : Controller
    {
        private readonly ClassService _service;
        public ClassController(ClassService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get_all")]
        public async Task<IActionResult> GetAllClasses()
        {
            var res = await _service.GetAllClasses();
            if (res.NotFound)
                return NotFound(res);
            return Ok(res.Result);
        }

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> CreateNewClass([FromBody] Class clas)
        {
            var res = await _service.CreateNewClass(clas);
            if (res.InternalServerError)
                return StatusCode(500, res.Error);
            return Ok(res.Result);
        }

        [Route("update")]
        [HttpPut]
        public async Task<IActionResult> UpdateClass([FromBody] Class clas)
        {
            var res = await _service.UpdateClass(clas);
            if (res.InternalServerError)
                return StatusCode(500, res.Error);
            return Ok(res.Result);
        }

        [Route("delete")]
        [HttpDelete]
        public async Task<IActionResult> DeleteClass([FromBody] Class clas)
        {
            var res = await _service.DeleteClass(clas);
            if (res.InternalServerError)
                return StatusCode(500, res.Error);
            return Ok(res.Result);
        }

        [Route("update_all")]
        [HttpPut]
        public async Task<IActionResult> UpdateAllClasses([FromBody] List<Class> classes)
        {
            var res = await _service.UpdateClassesAsList(classes);
            if (res.InternalServerError)
                return StatusCode(500, res.Error);
            return Ok(res.Result);
        }
    }
}
