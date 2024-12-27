using Microsoft.AspNetCore.Mvc;
using Schedule.Models.Models;
using WebDbApi.Services.Services;

namespace WebDbApi.Controllers
{
    [ApiController]
    [Route("week")]
    public class WeekController : Controller
    {
        private readonly WeekService _service;
        public WeekController(WeekService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get_all")]
        public async Task<IActionResult> GetAllWeeks()
        {
            var res = await _service.GetAllWeeks();
            if (res.NotFound)
                return NotFound(res.Error);
            return Ok(res.Result);
        }

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> CreateNewWeek([FromBody] Week week)
        {
            var res = await _service.CreateNewWeek(week);
            if (res.InternalServerError)
                return StatusCode(500, res.Error);
            return Ok(res.Result);
        }

        [Route("update")]
        [HttpPut]
        public async Task<IActionResult> UpdateWeek([FromBody] Week week)
        {
            var res = await _service.UpdateWeek(week);
            if (res.InternalServerError)
                return StatusCode(500, res.Error);
            return Ok(res.Result);
        }

        [Route("delete")]
        [HttpDelete]
        public async Task<IActionResult> DeleteWeek([FromBody] Week week)
        {
            var res = await _service.DeleteWeek(week);
            if (res.InternalServerError)
                return StatusCode(500, res.Error);
            return Ok(res.Result);
        }
    }
}
