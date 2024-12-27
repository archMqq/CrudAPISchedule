using Microsoft.AspNetCore.Mvc;
using Schedule.Models.Models;
using WebDbApi.Services.Services;

namespace WebDbApi.Controllers
{
    [ApiController]
    [Route("teacher")]
    public class TeacherController : Controller
    {
        private readonly TeacherService _service;
        public TeacherController(TeacherService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get_all")]
        public async Task<IActionResult> GetAllTeachers()
        {
            var res = await _service.GetAllTeachers();
            if (res.NotFound)
                return NotFound(res.Error);
            return Ok(res.Result);
        }

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> CreateNewTeacher([FromBody] Teacher teacher)
        {
            var res = await _service.CreateNewTeacher(teacher);
            if (res.InternalServerError)
                return StatusCode(500, res.Error);
            return Ok(res.Result);
        }

        [Route("update")]
        [HttpPut]
        public async Task<IActionResult> UpdateTeacher([FromBody] Teacher teacher)
        {
            var res = await _service.UpdateTeacher(teacher);
            if (res.InternalServerError)
                return StatusCode(500, res.Error);
            return Ok(res.Result);
        }

        [Route("delete")]
        [HttpDelete]
        public async Task<IActionResult> DeleteTeacher([FromBody] Teacher teacher)
        {
            var res = await _service.DeleteTeacher(teacher);
            if (res.InternalServerError)
                return StatusCode(500, res.Error);
            return Ok(res.Result);
        }
    }
}
