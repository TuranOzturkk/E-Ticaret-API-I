using ETicaretAPI.Application.Abstractions.Services;
using QRCoder;

namespace ETicaretAPI.Infrastructure.Services
{
    public class QRCodeService : IQRCodeService
    {

        public byte[] GenerateQRCode(string text)
        {
            QRCodeGenerator generator = new();
            QRCodeData data = generator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qrcode = new(data);
            byte[] byteGraphic = qrcode.GetGraphic(10, new byte[] { 23, 24, 25 }, new byte[] { 240, 240, 240 });
            return byteGraphic;
        }
    }
}
