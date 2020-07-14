using System;
using System.Net;
using System.Net.Mail;

namespace TrainingTests.Helpers
{
    public class SendEmail
    {
        public static string EMAIL = "chilo9997@gmail.com";
        public static string PASSWORD = "onwyrzsnxmlhyxih";

        public string SendRequest(string toEmail, string text)
        {
            MailMessage msg = new MailMessage();
            msg.Subject = "Горный университет";
            msg.From = new MailAddress(EMAIL);
            msg.Body = text;
            msg.To.Add(new MailAddress(toEmail));
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            NetworkCredential nc = new NetworkCredential(EMAIL, PASSWORD);
            smtp.Credentials = nc;
            try
            {
                smtp.Send(msg);
                return "Successful";
            }
            catch (Exception e)
            {
                return "Error: " + e.Message;
            }
        }
    }
}
