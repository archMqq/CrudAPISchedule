using Microsoft.AspNetCore.Mvc;
using Schedule.Models.Models;
using WebDbApi.Services.Services;

namespace WebDbApi.Controllers
{
    [ApiController]
    [Route("day")]
    public class DayController : Controller
    {
        private readonly DayService _service;
        public DayController(DayService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get_all")]
        public async Task<IActionResult> GetAllDays()
        {
            var res = await _service.GetAllDays();
            if (res.NotFound)
                return NotFound(res);
            return Ok(res.Result);
        }

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> CreateNewDay([FromBody] Day day)
        {
            var res = await _service.CreateNewDay(day);
            if (res.InternalServerError)
                return StatusCode(500, res.Error);
            return Ok(res.Result);
        }

        [Route("update")]
        [HttpPut]
        public async Task<IActionResult> UpdateDay([FromBody] Day day)
        {
            var res = await _service.UpdateDay(day);
            if (res.InternalServerError)
                return StatusCode(500, res.Error);
            return Ok(res.Result);
        }

        [Route("delete")]
        [HttpDelete]
        public async Task<IActionResult> DeleteDay([FromBody] Day day)
        {
            var res = await _service.DeleteDay(day);
            if (res.InternalServerError)
                return StatusCode(500, res.Error);
            return Ok(res.Result);
        }
    }
}
