using Microsoft.AspNetCore.Mvc;
using Schedule.Models.Models;
using WebDbApi.Services.Services;

namespace WebDbApi.Controllers
{
    [ApiController]
    [Route("subject")]
    public class SubjectController : Controller
    {
        private readonly SubjectService _service;
        public SubjectController(SubjectService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get_all")]
        public async Task<IActionResult> GetAllSubjects()
        {
            var res = await _service.GetAllSubjects();
            if (res.NotFound)
                return NotFound(res.Error);
            return Ok(res.Result);
        }

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> CreateNewSubject([FromBody] Subject subject)
        {
            var res = await _service.CreateNewSubject(subject);
            if (res.InternalServerError)
                return StatusCode(500, res.Error);
            return Ok(res.Result);
        }

        [Route("update")]
        [HttpPut]
        public async Task<IActionResult> UpdateSubject([FromBody] Subject subject)
        {
            var res = await _service.UpdateSubject(subject);
            if (res.InternalServerError)
                return StatusCode(500, res.Error);
            return Ok(res.Result);
        }

        [Route("delete")]
        [HttpDelete]
        public async Task<IActionResult> DeleteSubject([FromBody] Subject subject)
        {
            var res = await _service.DeleteSubject(subject);
            if (res.InternalServerError)
                return StatusCode(500, res.Error);
            return Ok(res.Result);
        }
    }
}
