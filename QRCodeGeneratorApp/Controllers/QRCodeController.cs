using Microsoft.AspNetCore.Mvc;
using QRCodeGeneratorApp.Models;
using QRCoder;
using System.Drawing;
using static QRCoder.QRCodeGenerator;

namespace QRCodeGeneratorApp.Controllers
{
	public class QRCodeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Index(QRCodeModel codeModel)
		{
			//QRCodeGenerator qrGenerator = new QRCodeGenerator();
			//QRCodeData qrCodeData = qrGenerator.CreateQrCode("The text which should be encoded.", QRCodeGenerator.ECCLevel.Q);
			//QRCode qrCode = new QRCode(qrCodeData);
			//Bitmap qrCodeImage = qrCode.GetGraphic(20);
			byte[]? qrcodeImage = null;
			using (QRCodeGenerator qrGenerator = new())
			using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(codeModel.Text, ECCLevel.Q))
			using (QRCode qrCode = new(qrCodeData))
			{
				Bitmap qrCodeImage = qrCode.GetGraphic(20);
				var qrCodeByte = BitmapToBytes(qrCodeImage);
				var imageUrl = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(qrCodeByte));
				ViewBag.ImageUrl = imageUrl;
			}
			return View(qrcodeImage);
		}
		private static byte[] BitmapToBytes(Bitmap img)
		{
			using MemoryStream stream = new();
			img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
			return stream.ToArray();
		}

	}
}
