using EASendMail;
using System;
using System.Threading.Tasks;



namespace PenYourPrayerServer.Supporting
{
    public class CommonMethod
    {
        public static async Task sendAccountActiviationEmail(string userEmailAddress, string Name, string verificationCode, string ID)
        {
            SmtpMail oMail = new SmtpMail("TryIt");
            SmtpClient oSmtp = new SmtpClient();

            oMail.From = QuickReference.emailFrom;
            oMail.To = userEmailAddress;
            oMail.Subject = "Activate your Pen Your Prayer Account";

            oMail.HtmlBody = @"Hi " + Name + @",
                               <br />Confirm your email address to complete your Twitter account. It's easy - just click
                               <br />on the button below.
                               <br />Click on the link below or copy and paste it into a browser:" +
                               @"<br />" + QuickReference.baseURL + "/account/Activation?ID=" + ID + "&VerificationCode=" + verificationCode;

            SmtpServer oServer = new SmtpServer(QuickReference.mailServer);

            oServer.User = QuickReference.emailFrom;
            oServer.Password = QuickReference.emailServerPassword;

            // Set 465 SMTP port
            oServer.Port = 465;

            // Enable SSL connection
            oServer.ConnectType = SmtpConnectType.ConnectSSLAuto;

            try
            {
                await Task.Run(() => oSmtp.SendMail(oServer, oMail));
            }
            catch (Exception ep)
            {
                Console.WriteLine("failed to send email with the following error:");
                Console.WriteLine(ep.Message);
            } 
        }

        public static async Task sendResetPasswordEmail(string userEmailAddress, string Name, string verificationCode, string ID)
        {
            SmtpMail oMail = new SmtpMail("TryIt");
            SmtpClient oSmtp = new SmtpClient();

            oMail.From = QuickReference.emailFrom;
            oMail.To = userEmailAddress;
            oMail.Subject = "Reset your Pen Your Prayer Account Password";

            oMail.HtmlBody = @"Hi " + Name + @",
                               <br />Confirm your email address to complete your Twitter account. It's easy - just click
                               <br />on the button below.
                               <br />Click on the link below or copy and paste it into a browser:" +
                               @"<br />" + QuickReference.baseURL + "/account/ResetPassword?ID=" + ID + "&VerificationCode=" + verificationCode;

            SmtpServer oServer = new SmtpServer(QuickReference.mailServer);

            oServer.User = QuickReference.emailFrom;
            oServer.Password = QuickReference.emailServerPassword;

            // Set 465 SMTP port
            oServer.Port = 465;

            // Enable SSL connection
            oServer.ConnectType = SmtpConnectType.ConnectSSLAuto;

            try
            {
                await Task.Run(() => oSmtp.SendMail(oServer, oMail));
            }
            catch (Exception ep)
            {
                Console.WriteLine("failed to send email with the following error:");
                Console.WriteLine(ep.Message);
            }
        }
    }
}