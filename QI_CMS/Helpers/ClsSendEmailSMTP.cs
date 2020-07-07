using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.ComponentModel;

namespace SendEmailHelper
{
    public class ClsSendEmailSMTP
    {
        public string From = string.Empty;
        public string To = string.Empty;
        public string User = string.Empty;
        public string Password = string.Empty;
        public string Subject = string.Empty;
        public string Body = string.Empty;
        public string AttachmentPath = string.Empty;
        public string Host = "smtp.gmail.com";//"127.0.0.1";
        public int Port = 25;
        public string CC = string.Empty;
        public bool IsHtml = false;
        public int SendUsing = 0;//0 = Network, 1 = PickupDirectory, 2 = SpecifiedPickupDirectory
        public bool UseSSL = true;
        public int AuthenticationMode = 1;//0 = No authentication, 1 = Plain Text, 2 = NTLM authentication
        public string Error { get { return _error; } }
        private string _error = "";

        public void SendEmail()
        {
            //SendMessage(ref _error);
            Thread thread = new Thread(() => SendMessage(ref _error));
            thread.Start();
        }

        /// <summary>
        /// Send Email Message method.
        /// </summary>
        public bool SendMessage(ref string error)
        {
            try
            {
                MailMessage oMessage = new MailMessage();
                SmtpClient smtpClient = new SmtpClient();

                oMessage.From = new MailAddress(From);
                oMessage.To.Add(To);
                oMessage.Subject = Subject;
                oMessage.IsBodyHtml = true;
                oMessage.Body = Body;
                oMessage.BodyEncoding = Encoding.UTF8;                
                
                if (CC != string.Empty)
                    oMessage.CC.Add(CC);

                switch (SendUsing)
                {
                    case 0:
                        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        break;
                    case 1:
                        smtpClient.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis;
                        break;
                    case 2:
                        smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                        break;
                    default:
                        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        break;

                }
                smtpClient.Host = Host;
                smtpClient.Port = Port;
                smtpClient.EnableSsl = true;
                if (AuthenticationMode > 0)
                {
                    smtpClient.Credentials = new NetworkCredential(User, Password);
                }
                // Create and add the attachment
                if (AttachmentPath != string.Empty)
                {
                    string[] attach = AttachmentPath.Split(Convert.ToChar(";"));
                    if (attach.Length > 0)
                    {
                        for (int i = 0; i < attach.Length; i++)
                        {
                            oMessage.Attachments.Add(new Attachment(attach[i]));
                        }
                    }
                }
                try
                {   
                    smtpClient.SendCompleted += new SendCompletedEventHandler(SendCompletedEmail);
                    //smtpClient.Send(oMessage);
                    smtpClient.Send(oMessage);
                    //smtpClient.SendAsync(oMessage, To);
                    return true;
                }
                catch (Exception ex)
                {
                   error = ex.ToString();
                    return false;
                }
            }
            catch (Exception ex)
            {
                error = ex.ToString();
                return false;
            }
        }

        private void SendCompletedEmail(object sender, AsyncCompletedEventArgs e)
        {
            if(e.Cancelled)
            {
                
            }
        }
    }
}
