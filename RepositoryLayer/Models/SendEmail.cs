using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace RepositoryLayer.Models
{
    public class SendEmail
    {
        public string EmailSend(string ToEmail, string Token)
        {
            string FromEmail = "gosaviamol1811@gmail.com";
            MailMessage Message = new MailMessage(FromEmail, ToEmail);

            string MailBody = "Token Generated : " + Token;

            Message.Subject = "Token Generated For Forgot Password";
            Message.Body = MailBody.ToString();
            Message.BodyEncoding = Encoding.UTF8;
            Message.IsBodyHtml = false;

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            NetworkCredential credential = new NetworkCredential("gosaviamol1811@gmail.com", "wkbz mwrm tihe cagq");
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = credential;

            smtpClient.Send(Message);
            return ToEmail;

        }
    }
}
