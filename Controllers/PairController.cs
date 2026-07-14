using LocalTransfer.IService;
using LocalTransfer.Model;
using LocalTransfer.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System.Text.Json;

namespace LocalTransfer.Controllers
{
    [ApiController]
    [Route("api/pair")]
    public class PairController : ControllerBase
    {
        private readonly IPairingService _pairing;
        private readonly IConnectionService _connectionService;

        public PairController(IPairingService pairing, IConnectionService connectionService)
        {
            _pairing = pairing;
            _connectionService = connectionService;
        }

        //[HttpGet("generate")]
        //public IActionResult Generate()
        //{
        //    var token = _pairing.GenerateToken();
        //    var host = HttpContext.Request.Host.Value;
        //    var url = $"http://{host}/pair?token={token}";
        //    using var qrGenerator = new QRCodeGenerator();
        //    var data = qrGenerator.CreateQrCode( url, QRCodeGenerator.ECCLevel.Q);
        //    var qr = new PngByteQRCode(data);
        //    return File( qr.GetGraphic(20), "image/png");
        //}

        //[HttpGet("generate")]
        //public IActionResult Generate()
        //{
        //    object qrPayload;

        //    try
        //    {
        //        var token = _pairing.GenerateToken();
        //        var connection = _connectionService.GetConnectionInfo();

        //        qrPayload = new
        //        {
        //            success = connection.IsConnected,
        //            error = connection.IsConnected ? "" : connection.Error,

        //            version = 1,
        //            token = token,

        //            scheme = HttpContext.Request.Scheme,
        //            host = connection.IpAddress,
        //            port = HttpContext.Request.Host.Port ?? 80,

        //            baseUrl = $"{HttpContext.Request.Scheme}://{connection.IpAddress}:{HttpContext.Request.Host.Port ?? 80}",

        //            api = new
        //            {
        //                pair = "/api/pair/connect",
        //                upload = "/api/file/upload",
        //                download = "/api/file/download",
        //                discover = "/api/device/discover"
        //            },

        //            device = new
        //            {
        //                name = connection.DeviceName,
        //                host = connection.HostName,
        //                network = connection.NetworkType
        //            },

        //            expires = DateTime.UtcNow.AddMinutes(5)
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        qrPayload = new
        //        {
        //            success = false,
        //            error = ex.Message,
        //            exception = ex.GetType().Name,
        //            generated = DateTime.UtcNow
        //        };
        //    }

        //    var json = JsonSerializer.Serialize(qrPayload);

        //    using var generator = new QRCodeGenerator();
        //    var data = generator.CreateQrCode(json, QRCodeGenerator.ECCLevel.Q);
        //    var qr = new PngByteQRCode(data);

        //    return File(qr.GetGraphic(20), "image/png");
        //}

        [HttpGet("generate")]
        public IActionResult Generate()
        {
            object qrPayload;

            try
            {
                var token = _pairing.GenerateToken();
                var connection = _connectionService.GetConnectionInfo();

                // Unique session for this QR
                var sessionId = Guid.NewGuid().ToString("N");

                // Permanent device id
                var deviceId = Environment.MachineName;

                qrPayload = new
                {
                    version = 1,

                    success = connection.IsConnected,
                    error = connection.IsConnected ? "" : connection.Error,

                    sessionId = sessionId,
                    token = token,

                    connectionState = "WaitingForScanner",

                    scheme = HttpContext.Request.Scheme,
                    host = connection.IpAddress,
                    port = HttpContext.Request.Host.Port ?? 80,

                    baseUrl = $"{HttpContext.Request.Scheme}://{connection.IpAddress}:{HttpContext.Request.Host.Port ?? 80}",

                    api = new
                    {
                        connect = "/api/pair/connect",
                        heartbeat = "/api/pair/heartbeat",
                        disconnect = "/api/pair/disconnect",
                        upload = "/api/file/upload",
                        download = "/api/file/download",
                        discover = "/api/device/discover"
                    },

                    device = new
                    {
                        id = deviceId,
                        name = connection.DeviceName,
                        host = connection.HostName,
                        ip = connection.IpAddress,
                        network = connection.NetworkType,
                        os = Environment.OSVersion.Platform.ToString(),
                        osVersion = Environment.OSVersion.VersionString
                    },

                    expires = DateTime.UtcNow.AddMinutes(5),
                    generated = DateTime.UtcNow
                };

                // Convert object to JSON
                var json = JsonSerializer.Serialize(qrPayload, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                // Create Sessions folder if not exists
                var sessionFolder = Path.Combine(Directory.GetCurrentDirectory(), "Sessions");

                if (!Directory.Exists(sessionFolder))
                    Directory.CreateDirectory(sessionFolder);

                // Save JSON
                var filePath = Path.Combine(sessionFolder, $"{sessionId}.json");

                System.IO.File.WriteAllText(filePath, json);

                // Generate QR Code
                using var generator = new QRCodeGenerator();
                var data = generator.CreateQrCode(json, QRCodeGenerator.ECCLevel.Q);
                var qr = new PngByteQRCode(data);

                return File(qr.GetGraphic(20), "image/png");
            }
            catch (Exception ex)
            {
                var error = new
                {
                    success = false,
                    error = ex.Message,
                    exception = ex.GetType().Name,
                    generated = DateTime.UtcNow
                };

                return BadRequest(error);
            }
        }

        [HttpPost("connect")]
        public IActionResult Connect([FromBody] PairRequest request)
        {
            var filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Sessions",
                $"{request}.json");

            if (!System.IO.File.Exists(filePath))
            {
                return BadRequest("Session not found.");
            }

            var json = System.IO.File.ReadAllText(filePath);

            using var doc = JsonDocument.Parse(json);

            var token = doc.RootElement.GetProperty("token").GetString();

            if (token != request.Token)
            {
                return BadRequest("Invalid Token.");
            }

            if (!_pairing.ValidateToken(request.Token))
            {
                return BadRequest("Token expired.");
            }

            _pairing.RemoveToken(request.Token);

            return Ok(new
            {
                Connected = true,
                Device = request.DeviceName,
                DeviceType = request.DeviceType,
                Time = DateTime.Now
            });
        }



    }
}
