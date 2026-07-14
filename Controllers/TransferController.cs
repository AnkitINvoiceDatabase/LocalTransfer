using LocalTransfer.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LocalTransfer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferController : ControllerBase
    {

        private readonly ITransferService _transfer;

        public TransferController(ITransferService transfer)
        {
            _transfer = transfer;
        }

        [HttpPost("start")]
        public IActionResult Start(   string sender, string receiver, string fileName, long fileSize, string fileType)
        {
            var result = _transfer.StartTransfer(  sender, receiver, fileName,  fileSize, fileType);
            return Ok(result);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload( string transferId, IFormFile file)
        {
            if (file == null)
                return BadRequest();

            var ok = await _transfer.SaveFileAsync(
                transferId,
                file);

            if (!ok)
                return BadRequest();

            return Ok();
        }

        [HttpPost("finish")]
        public IActionResult Finish(string transferId)
        {
            return Ok(_transfer.FinishTransfer(transferId));
        }

        [HttpGet("history")]
        public IActionResult History()
        {
            return Ok(_transfer.GetHistory());
        }
    }
}
