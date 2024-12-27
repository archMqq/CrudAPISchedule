using Microsoft.AspNetCore.Mvc;
using Schedule.Models.Models;
using WebDbApi.Services.Services;

namespace WebDbApi.Controllers
{
    [ApiController]
    [Route("group")]
    public class GroupController : Controller
    {
        private readonly GroupService _groupService;

        public GroupController(GroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpGet]
        [Route("get_all")]
        public async Task<IActionResult> GetAllGroups()
        {
            var res = await _groupService.GetAllGroups();
            if (res.NotFound)
                return NotFound(res.Error);
            return Ok(res.Result);
        }

       

        [Route("update_numb")]
        [HttpPut]
        public async Task<IActionResult> UpdateNumber([FromQuery] int groupId, [FromBody] string groupNumber)
        {
            var res = await _groupService.UpdateGroupNumber(groupId, groupNumber);
            if (res.NotFound)
                return NotFound(res.Error);
            return Ok(res.Result);
        }

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> CreateNewGroup([FromBody] Group group)
        {
            var res = await _groupService.CreateNewGroup(group);
            if (res.InternalServerError)
                return StatusCode(500, res.Error);
            return Ok(res.Result);
        }

        [Route("update")]
        [HttpPut]
        public async Task<IActionResult> UpdateGroup([FromBody] Group group)
        {
            var res = await _groupService.UpdateGroup(group);
            if (res.InternalServerError)
                return StatusCode(500, res.Error);
            return Ok(res.Result);
        }

        [Route("delete")]
        [HttpDelete]
        public async Task<IActionResult> DeleteGroup([FromBody] Group group)
        {
            var res = await _groupService.DeleteGroup(group);
            if (res.InternalServerError)
                return StatusCode(500, res.Error);
            return Ok(res.Result);
        }
    }
}
