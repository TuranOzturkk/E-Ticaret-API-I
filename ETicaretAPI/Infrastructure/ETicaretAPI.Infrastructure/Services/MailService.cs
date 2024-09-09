using ETicaretAPI.Application.Abstractions.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ETicaretAPI.Infrastructure.Services
{
    internal class MailService : IMailService
    {
        readonly IConfiguration _configuration;
        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            await SendMailAsync(new[] { to }, subject, body, isBodyHtml);
        }

        public async Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
        {
            MailMessage mail = new();
            mail.IsBodyHtml = isBodyHtml;
            foreach (var to in tos)
                mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.From = new(_configuration["Mail:Username"], "2RAN E-Ticaret", System.Text.Encoding.UTF8);

            SmtpClient smtp = new SmtpClient();
            smtp.Credentials = new NetworkCredential(_configuration["Mail:Username"], _configuration["Mail:Password"]);
            smtp.Host = _configuration["Mail:Host"];
            smtp.Port = int.Parse(_configuration["Mail:Port"]);
            smtp.EnableSsl = true;

            await smtp.SendMailAsync(mail);
        }

        public async Task SendPasswordResetMailAsync(string to, string userId, string resetToken)
        {
            ////link olusturmuyor hata var daha sonra duzeltilecek!
            //StringBuilder mail = new();
            //mail.AppendLine("Merhaba<br/>Eğer yeni şifre Talebinde bulunduysanız aşağıdaki linkten şifrenizi yenileyebilirsiniz.<br/><strong><a target=\"_blank\" href=\"");
            //mail.AppendLine(_configuration["AngularClientUrl"]);
            //mail.AppendLine("/update-password/");
            //mail.AppendLine(userId);
            //mail.AppendLine("/");
            //mail.AppendLine(resetToken);
            //mail.AppendLine("\"> Yeni Şifre talebi için tıklayınız... </a></strong><br/><br/><span style=\"font-size:10px; color:red;\">Eğer Bu Talep Tarafınızca Gerçekleştirilmemiş İse Bu Maili Ciddiye Almayınız.!</span><br/>Saygılarımızla...<br/><br/><br/>2RAN | E-Ticaret");

            //await SendMailAsync(to,"Şifre Yenileme Talebi", mail.ToString());

            string mail = $" Merhaba<br>Eğer yeni şifre Talebinde bulunduysanız aşağıdaki linkten şifrenizi yenileyebilirsiniz.<br>" +
                $"<strong><a target=\"_blank\" href=\"{_configuration["AngularClientUrl"]}/update-password/{userId}/{resetToken}\"> Yeni Şifre talebi için tıklayınız... </a></strong>" + $"<br><span style=\"font-size:10px; color:red;\">Eğer Bu Talep Tarafınızca Gerçekleştirilmemiş İse Bu Maili Ciddiye Almayınız.!</span><br>Saygılarımızla...<br><br><br>2RAN | E-Ticaret";
            await SendMailAsync(to, "Şifre Yenileme Talebi", mail);
        }

        public async Task SenCompletedOrderMailAsync(string to, string orderCode, DateTime orderDate, string userName, string userSurname)
        {
            string mail = $"Sayın {userName} {userSurname} Merhaba<br>" +
                $"<span style=\"font-size:10px; color:red;\">{orderDate}</span> tarihinde Vermiş olduğunuz <span style=\"font-size:10px; color:red;\">{orderCode}</span> kodlu siparişiniz tamamlanmış ve kargo firmasına teslim edilmiştir iyi günlerde kullanın.<br><br> 2RAN | E-Ticaret ";

            await SendMailAsync(to, $"{orderCode} numaralı siparişiniz hakkında ", mail);
        }

    }
}
