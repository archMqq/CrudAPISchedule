using Microsoft.AspNetCore.Mvc;
using Schedule.Models.Models;
using WebDbApi.Services.Services;

namespace WebDbApi.Controllers
{
    [ApiController]
    [Route("qr_code")]
    public class QrCodeController : Controller
    {
        private readonly QrCodeService _service;
        public QrCodeController(QrCodeService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("find")]
        public async Task<IActionResult> FindQrCode([FromQuery] string code)
        {
            var res = await _service.FindQrCode(code);
            if (res.NotFound) 
                return NotFound(res.Error);
            return Ok(res.Result);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateNewQrCode([FromBody] QrCode qrCode)
        {
            var res = await _service.CreateNewQrCode(qrCode);
            if (res.InternalServerError)
                return StatusCode(500, res.Error);
            return Ok(res.Result);
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteQrCode([FromQuery] int Id)
        {
            var res = await _service.DeleteQrCode(Id);
            if (res.InternalServerError)
                return StatusCode(500, res.Error);
            return Ok(res.Result);
        }
    }
}
