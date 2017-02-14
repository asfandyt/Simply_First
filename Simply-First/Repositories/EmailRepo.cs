using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

namespace Simply_First.Repositories
{
    public class EmailRepo
    {
        public void SendEmail(string recipientEmailAddress, string subjectMessage, string text)
        {
            try
            {
                MailMessage mailMsg = new MailMessage();

                // To
                mailMsg.To.Add(new MailAddress(recipientEmailAddress, "Simply First"));

                // From
                mailMsg.From = new MailAddress("guri_bola@hotmail.com", "Simply First");

                // Subject and multipart/alternative Body
                mailMsg.Subject = subjectMessage;

                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));

                // Init SmtpClient and send
                SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("Gurkirat_Bola", "qwerty123456789");

                smtpClient.Credentials = credentials;
                smtpClient.Send(mailMsg);
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
            }
        }
    }
}