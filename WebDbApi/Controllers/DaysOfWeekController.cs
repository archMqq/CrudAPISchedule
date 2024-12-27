using Microsoft.AspNetCore.Mvc;
using WebDbApi.Services.Services;

namespace WebDbApi.Controllers
{
    [ApiController]
    [Route("day_of_week")]
    public class DaysOfWeekController : Controller
    {
        private readonly DayOfWeekService _service;
        public DaysOfWeekController(DayOfWeekService service)
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
    }
}
