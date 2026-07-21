using LocalTransfer.IService;
using LocalTransfer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LocalTransfer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConnectionController : ControllerBase
    {
        private readonly IConnectionService _connectionService;

        public ConnectionController(IConnectionService connectionService)
        {
            _connectionService = connectionService;
        }
        /// <summary>
        /// Verify device connection
        /// </summary>
        //[HttpPost("verify")]
        //public IActionResult Verify([FromBody] VerifyConnectionRequest request)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);
        //    var result = _connectionService.VerifyConnection(request);
        //    if (!result.Success)
        //        return BadRequest(result);
        //    return Ok(result);
        //}
        [HttpPost("verify")]
        public IActionResult Verify([FromBody] VerifyConnectionRequest request)
        {
            if (request == null)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Request is required."
                });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _connectionService.VerifyConnection(request);

            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        /// <summary>
        /// Get connection status
        /// </summary>
        [HttpGet("status")]
        public IActionResult Status(string sessionId)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
                return BadRequest("SessionId is required.");

            var result = _connectionService.GetStatus(sessionId);

            if (!result.Connected)
                return NotFound(result);

            return Ok(result);
        }
        /// <summary>
        /// Heartbeat
        /// </summary>
        [HttpPost("ping")]
        public IActionResult Ping([FromBody] string sessionId)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
                return BadRequest("SessionId is required.");

            bool success = _connectionService.Ping(sessionId);

            if (!success)
                return NotFound(new
                {
                    Success = false,
                    Message = "Session not found."
                });

            return Ok(new
            {
                Success = true,
                Message = "Heartbeat received.",
                Time = DateTime.Now
            });
        }
        /// <summary>
        /// Disconnect device
        /// </summary>
        [HttpPost("disconnect")]
        public IActionResult Disconnect([FromBody] string sessionId)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
                return BadRequest("SessionId is required.");

            bool success = _connectionService.Disconnect(sessionId);

            if (!success)
                return NotFound(new
                {
                    Success = false,
                    Message = "Session not found."
                });

            return Ok(new
            {
                Success = true,
                Message = "Device disconnected successfully."
            });
        }
        /// <summary>
        /// Get all connected devices
        /// </summary>
        [HttpGet("devices")]
        public IActionResult Devices()
        {
            var devices = _connectionService.GetConnectedDevices();

            return Ok(devices);
        }
    }
}
