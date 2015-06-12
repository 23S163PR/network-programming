using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace smtp_client_demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var mailFrom = new MailAddress("groza579@gmail.com");
            var mailTo = new MailAddress("marionbrown@dayrep.com");

            var mailMessage = new MailMessage(mailFrom, mailTo);
            mailMessage.Subject = "Hello Mario!";
            mailMessage.Body = "Hello World!";

            var smtpClient = new SmtpClient();
            smtpClient.Send(mailMessage);
        }
    }
}
