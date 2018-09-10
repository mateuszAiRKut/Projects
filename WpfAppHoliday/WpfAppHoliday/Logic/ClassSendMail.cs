using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;

namespace WpfAppHoliday.Logic
{
    class ClassSendMail
    {
        public String MailFrom { private set; get; }
        public String MailFromName { private set; get; }
        public string MailTo { private set; get; }
        public string PathFile { get; set; }

        private string userName, host, passwordHash;
        private int port;

        public ClassSendMail()
        {
        }

        public ClassSendMail(string from, string fromName, string to, string userN, string password)
        {
            MailFrom = from;
            MailFromName = fromName;
            MailTo = to;
            host = "smtp.gmail.com";
            port = 587;
            userName = userN;
            passwordHash = hashPassword(password);
        }

        private string hashPassword(string password)
        {
            byte[] salt, hash, hashBytes;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            hash = pbkdf2.GetBytes(20);
            hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            return Convert.ToBase64String(hashBytes);
        }

        private void verifyPassword(string password)
        {
            byte[] hashBytes = Convert.FromBase64String(passwordHash);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                    throw new UnauthorizedAccessException();
            }
        }

        public Task SendMailTask(string password, string subject = "Test mail", string body = "", bool attach = false)
        {
            Action action = new Action(
                () =>
                {
                    SendMail(password, subject, body, attach);
                });
            Task task = new Task(action);
            task.Start();
            return task;
        }

        public void SendMail(string password, string subject = "Test mail", string body = "", bool attach = false)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(MailFrom,MailFromName);
                mail.To.Add(new MailAddress(MailTo));
                mail.Subject = subject;
                mail.Body = body;

                if(attach && PathFile != "")
                {
                    Attachment attachment = new Attachment(PathFile);
                    mail.Attachments.Add(attachment);
                }

                using (SmtpClient SmtpServer = new SmtpClient(host,port))
                {
                    verifyPassword(password);
                    SmtpServer.Credentials = new NetworkCredential(userName, password);
                    SmtpServer.EnableSsl = true;
                    try
                    {
                        SmtpServer.Send(mail);
                    }catch(Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
