using MailKit.Security;
using MimeKit;
using MimeKit;
using WorkFlowHR.Application.DTOs.MailDTOs;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;


namespace WorkFlowHR.Application.Services.MailServices
{
    public class MailService:IMailService
    {
        public async Task SendMailAsync(MailDTO mailDTO)
        {//xhqwydkdlastoobo

            try
            {
                var newMail = new MimeMessage
                {
                    From = { MailboxAddress.Parse("info.hrmanagementsystem@gmail.com") },
                    To = { MailboxAddress.Parse(mailDTO.Email) },
                    Subject = mailDTO.Subject
                };

                var builder = new BodyBuilder
                {
                    HtmlBody = mailDTO.Message
                };
                newMail.Body = builder.ToMessageBody();

                using (var smtp = new SmtpClient())
                {
                    await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

                    await smtp.AuthenticateAsync("info.hrmanagementsystem@gmail.com", "xhqwydkdlastoobo");

                    await smtp.SendAsync(newMail);

                    await smtp.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Email gönderilirken hata oluştu: {ex.Message}", ex);
            }

        }
    }
}
