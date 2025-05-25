using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class MailService
    {
        private readonly IConfiguration _configuration;
        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendResetTokenAsync(string toEmail, string token)
        {
            try
            {
                string fromEmail = _configuration["MailSettings:Mail"];
                string displayName = _configuration["MailSettings:DisplayName"];

                var message = new MailMessage
                {
                    From = new MailAddress(fromEmail, displayName),
                    Subject = "Reset Password - BookStore",
                    Body = $"<h3>Password Reset Request</h3><p>Your reset token is: <strong>{token}</strong></p>",
                    IsBodyHtml = true,
                    BodyEncoding = Encoding.UTF8
                };
                message.To.Add(toEmail);

                using var smtp = new SmtpClient(_configuration["MailSettings:Host"], int.Parse(_configuration["MailSettings:Port"]))
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(
                        _configuration["MailSettings:Mail"],
                        _configuration["MailSettings:Password"])
                };

                smtp.Timeout = 10000; // Set timeout to 10 seconds

                Console.WriteLine("Sending email to: " + toEmail);
                try
                {
                    await smtp.SendMailAsync(message);
                    //smtp.Send(message);
                    //   Console.WriteLine("Email sent successfully. by latest change that is send method");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("SMTP ERROR: " + ex.Message);
                    throw;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Email error: " + ex.Message);
                throw;
            }
        }
    }
}
