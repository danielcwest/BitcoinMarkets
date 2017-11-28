using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using Core.Config;

namespace Core
{
    public static class EmailHelper
    {
        private const string gmailHost = "smtp.gmail.com";

        //Simple emails do not have HTML content by definition
        public static void SendSimpleMail(string toAddresses, string subject, string body, string smtpAccountPassword, string fromAddress)
        {
            //try
            //{
            //    // Validate required parameters
            //    if (string.IsNullOrWhiteSpace(toAddresses) || string.IsNullOrWhiteSpace(fromAddress) || string.IsNullOrWhiteSpace(smtpAccountPassword))
            //        return;

            //    using (var smtp = new SmtpClient
            //    {
            //        Host = gmailHost,
            //        Port = 587,
            //        EnableSsl = true,
            //        DeliveryMethod = SmtpDeliveryMethod.Network,
            //        UseDefaultCredentials = false,
            //        Credentials = new NetworkCredential(fromAddress, smtpAccountPassword),
            //        Timeout = 30000
            //    })
            //    using (MailMessage message = new MailMessage(new MailAddress(fromAddress), new MailAddress(toAddresses))
            //    {
            //        Subject = subject,
            //        Body = body
            //    })
            //    {
            //        // Fixed multiple To addresses
            //        message.To.Clear();
            //        message.To.Add(toAddresses);

            //        smtp.Send(message);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex);
            //}
        }

        public static Task SendSimpleMailAsync(string toAddress, string subject,
            string body, string fromAddress, string password)
        {
            //Create and Start Task
            return Task.Factory.StartNew(() => SendSimpleMail(toAddress, subject,
                body, password, fromAddress));
        }

        public static Task SendSimpleMailAsync(GmailConfig gmail, string subject, string body)
        {
            //Create and Start Task
            return Task.Factory.StartNew(() => SendSimpleMail(gmail.To, subject,
                body, gmail.Password, gmail.Account));
        }
    }
}