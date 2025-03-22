using DataloaderApi.Extension.Services.Interface;
using System.Net;
using System.Net.Mail;

namespace DataloaderApi.Extension.Services
{
    public class EmailService : IEmaliSenderService
    {
         async Task IEmaliSenderService.SendEmail(List<string> email, string subject, string message)
        {
            try


            {

                var fromMail = "dataloaderservice03@gmail.com";
                var password = "ebmz vebg zmbn itkm";
                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtpClient.Credentials = new NetworkCredential(fromMail, password);
                    smtpClient.EnableSsl = true;

                    MailMessage mail = new MailMessage
                    {
                        From = new MailAddress(fromMail),
                        Subject = subject,
                        Body = message,
                        IsBodyHtml = true
                    };

                    // Add multiple recipients
                    foreach (string recipient in email)
                    {
                        mail.To.Add(new MailAddress(recipient));
                    }

                    await smtpClient.SendMailAsync(mail);
                    Console.WriteLine("Email sent successfully!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
