using Microsoft.AspNetCore.Mvc;
using Schedule.Models.Models;
using WebDbApi.Services.Services;

namespace WebDbApi.Controllers
{
    [ApiController]
    [Route("room")]
    public class RoomController : Controller
    {
        private readonly RoomService _service;
        public RoomController(RoomService service)
        {
            _service = service;
        }
        [HttpGet]
        [Route("get_all")]
        public async Task<IActionResult> GetAllRooms()
        {
            var res = await _service.GetAllRooms();
            if (res.NotFound)
                return NotFound(res.Error);
            return Ok(res.Result);
        }

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> CreateNewRoom([FromBody] Room room)
        {
            var res = await _service.CreateNewRoom(room);
            if (res.InternalServerError)
                return StatusCode(500, res.Error);
            return Ok(res.Result);
        }

        [Route("update")]
        [HttpPut]
        public async Task<IActionResult> UpdateRoom([FromBody] Room room)
        {
            var res = await _service.UpdateRoom(room);
            if (res.InternalServerError)
                return StatusCode(500, res.Error);
            return Ok(res.Result);
        }

        [Route("delete")]
        [HttpDelete]
        public async Task<IActionResult> DeleteRoom([FromBody] Room room)
        {
            var res = await _service.DeleteRoom(room);
            if (res.InternalServerError)
                return StatusCode(500, res.Error);
            return Ok(res.Result);
        }
    }
}
