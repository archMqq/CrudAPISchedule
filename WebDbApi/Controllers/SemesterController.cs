using Microsoft.AspNetCore.Mvc;
using Schedule.Models.Models;
using WebDbApi.Services.Services;

namespace WebDbApi.Controllers
{
    [ApiController]
    [Route("semester")]
    public class SemesterController : Controller
    {
        private readonly SemesterService _service;
        public SemesterController(SemesterService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get_all")]
        public async Task<IActionResult> GetAllSemesters()
        {
            var res = await _service.GetAllSemesters();
            if (res.NotFound)
                return NotFound(res.Error);
            return Ok(res.Result);
        }

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> CreateNewSemester([FromBody] Semester semester)
        {
            var res = await _service.CreateNewSemester(semester);
            if (res.InternalServerError)
                return StatusCode(500, res.Error);
            return Ok(res.Result);
        }

        [Route("update")]
        [HttpPut]
        public async Task<IActionResult> UpdateSemester([FromBody] Semester semester)
        {
            var res = await _service.UpdateSemester(semester);
            if (res.InternalServerError)
                return StatusCode(500, res.Error);
            return Ok(res.Result);
        }

        [Route("delete")]
        [HttpDelete]
        public async Task<IActionResult> DeleteSemester([FromBody] Semester semester)
        {
            var res = await _service.DeleteSemester(semester);
            if (res.InternalServerError)
                return StatusCode(500, res.Error);
            return Ok(res.Result);
        }
    }
}
