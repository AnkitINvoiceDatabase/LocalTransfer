using Microsoft.AspNetCore.Mvc;
using ZXing;
using ZXing.Windows.Compatibility;
using System.Drawing;

namespace LocalTransfer.Controllers
{
    public class ScanController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Please select a QR image.");
            }

            try
            {
                using var stream = file.OpenReadStream();
                using var bitmap = new Bitmap(stream);

                var reader = new BarcodeReader();

                var result = reader.Decode(bitmap);

                if (result == null)
                {
                    return BadRequest("QR Code not found.");
                }

                string qrText = result.Text;

                // Example:
                // http://192.168.1.5:5000/pair?token=ABC123

                return Ok(new
                {
                    Success = true,
                    QrText = qrText
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
