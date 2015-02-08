using PillBox.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace PillBox.Services
{
    public class EmailerService
    {
        private string _fromAddress;
        private string _message;
        private string _subject;
        private string _password;
        private NetworkCredential _credential;
        private MailMessage _mailMessage;
        private SmtpClient _smtpClient;

        public EmailerService()
        {
            _fromAddress = Constants.EMAIL_ADDRESS;
            _password = Constants.PASSWORD;
            SetupSmtp();
        }

        private SmtpClient SetupSmtp()
        {
            _smtpClient = new SmtpClient();
            _smtpClient.Host = Constants.GMAIL_SERVER;
            _smtpClient.Port = Constants.GMAIL_PORT;
            _smtpClient.UseDefaultCredentials = false;
            _smtpClient.EnableSsl = true;
            _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            _smtpClient.Credentials = new NetworkCredential(_fromAddress, _password);

            return _smtpClient;
        }

        public bool SendEmailTo(string address, string html)
        {
            try
            {

                MailMessage localMM = 
                    new MailMessage(_fromAddress, address, "PillBox Reminder!", html);
                localMM.IsBodyHtml = true;

                _smtpClient.Send(localMM);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
