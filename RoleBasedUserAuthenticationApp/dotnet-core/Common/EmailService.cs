using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Text;

namespace startup_kit_api.Common
{
    public class EmailService : IDisposable
    {
        private const string SenderEmail = "email@dummy.com";
        private const string Password = "xxxxxxxxxxxxxxxxx";

        private SmtpClient _smtpClient;
        public StringBuilder _body;

        public EmailService()
        {
            _body = new StringBuilder();
            _smtpClient = new SmtpClient();
        }

        public void Dispose()
        {
            _body.Clear();
            _smtpClient.Dispose();
        }

        public MimeMessage FormatEmail(string receiverEmail, string subject)
        {
            MimeMessage message = new MimeMessage();

            MailboxAddress mailBoxAddress = new MailboxAddress("name", SenderEmail);
            message.From.Add(mailBoxAddress);

            MailboxAddress temp = MailboxAddress.Parse(receiverEmail);
            message.To.Add(temp);

            message.Subject = subject;

            message.Body = new TextPart("plain") { Text = _body.ToString() };

            return message;
        }

        public bool SendEmailAsync(string receiverEmail, string subject)
        {
            try
            {
                _smtpClient.Connect("smtp.gmail.com", 465, true);
                _smtpClient.Authenticate(SenderEmail, Password);
                MimeMessage mimeMessage = FormatEmail(receiverEmail, subject);
                _smtpClient.Send(mimeMessage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                _smtpClient.Disconnect(true);
                _smtpClient.Dispose();
            }
        }
    }
}