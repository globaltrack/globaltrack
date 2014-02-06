using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Utilities
{
    public class SubscriberMailSettings
    {
        public string SmtpUrl;
        public string SmtpPassword;
        public bool UseSsl;
        public string DestinationEmailAddress;
        public string SourceEmailAddress;
        public int SmtpPort;
        public string SourceFullName;
        public string DestinationFullName; 

    }

    public class Mailer
    {
        public static void NotifyTrackingStatusChanged(SubscriberMailSettings settings, string trackingNumber, string status,
                                                string comment)
        {
            var fromAddress = new MailAddress(settings.SourceEmailAddress, settings.SourceFullName);
            var toAddress = new MailAddress(settings.DestinationEmailAddress, settings.DestinationFullName);
            var smtp = new SmtpClient
                {
                    Host = settings.SmtpUrl,
                    Port = settings.SmtpPort,
                    EnableSsl = settings.UseSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, settings.SmtpPassword)
                };
            using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = string.Format("Tracking status was changed - {0}", trackingNumber),
                    Body =
                        string.Format(
                            "Tracking with a number {0} was chaned its status. \nThe new status is: {1}\nComment: {2}",
                            trackingNumber, status, comment)
                })
            {
                try
                {
                    smtp.Send(message);
                }
                catch (Exception)
                {
                    throw;
                }

            }

        }

        public static void NotifyNewTracking(SubscriberMailSettings settings, string trackingNumber, string status,
                                             string comment, string trackingItemName, string qrUrl, string password)
        {
            var fromAddress = new MailAddress(settings.SourceEmailAddress, settings.SourceFullName);
            var toAddress = new MailAddress(settings.DestinationEmailAddress, settings.DestinationFullName);
            var smtp = new SmtpClient
            {
                Host = settings.SmtpUrl,
                Port = settings.SmtpPort,
                EnableSsl = settings.UseSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, settings.SmtpPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = string.Format("New tracking has been created - {0}", trackingNumber),
                    Body = ResourceTemplates.NewTrackingNotificationTemplate, 
                    BodyEncoding = Encoding.Unicode, 
                    IsBodyHtml = true
                })

                

            {
                message.Body = message.Body.Replace("#ServiceProvider#", settings.SourceFullName);
                message.Body = message.Body.Replace("#TrackingItemName#", trackingItemName);
                message.Body = message.Body.Replace("#Status#", status);
                message.Body = message.Body.Replace("#Comment#", comment);
                message.Body = message.Body.Replace("#TrackingNumber#", trackingNumber);
                message.Body = message.Body.Replace("#Comment#", comment);
                message.Body = string.IsNullOrWhiteSpace(password) ? message.Body.Replace("#Password#", "Not applicable") : message.Body.Replace("#Password#", password);
                message.Body = message.Body.Replace("#QrCodeUrl#", qrUrl);
                byte[] bytes = Encoding.Default.GetBytes(message.Body);
                message.Body = Encoding.Unicode.GetString(bytes);
                
                try
                {
                    smtp.Send(message);
                }
                catch (Exception)
                {
                    throw;
                }

            }
        }
    }
}
