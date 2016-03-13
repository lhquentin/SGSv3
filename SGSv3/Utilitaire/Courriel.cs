using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace SGSv3
{
    public class Courriel
    {
        public static void envoyerCourriel(MailMessage message)
        {
            SmtpClient client = new SmtpClient();
            client.Host = "mail.cgmatane.qc.ca";
            client.Port = 25;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("no-reply@cgmatane.qc.ca", "password");
            ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            client.Send(message); 
        }
    }
}