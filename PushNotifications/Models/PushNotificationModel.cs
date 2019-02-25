using PushNotifications.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using WebPush;

namespace PushNotifications.Models
{
    public class PushNotificationModel 
    {
        public PushNotificationModel()
        {
            var subscriptionBrowsers = SubscriptionBrowsers;
            Browsers = new List<string>();

            foreach (var browser in subscriptionBrowsers.Browsers)
            {
                this.Browsers.Add(browser);
            }
        }

        public void SaveSubscription(string pushSubscription, string browser)
        {
            if (string.IsNullOrWhiteSpace(pushSubscription))
                throw new Exception("PushSubscription is empty");

            if (string.IsNullOrWhiteSpace(browser))
                throw new Exception("Browser is empty");

            if (!SubscriptionBrowsers.Browsers.Contains(browser))
            {
                SubscriptionBrowsers.Browsers.Add(browser);
                Browsers = SubscriptionBrowsers.Browsers;
            }

            Subscription subscription = Newtonsoft.Json.JsonConvert.DeserializeObject<Subscription>(pushSubscription);

            SubscriptionBrowsers.Subscriptions[browser] = subscription;

            SaveSubscriptionBrowsers();
        }

        private void SaveSubscriptionBrowsers()
        {
            BinarySerialization.WriteToBinaryFile<SubscriptionBrowsers>(SubscriptionsFileName, SubscriptionBrowsers);
        }

        public void FireNotification(NotificationContent notificationContent)
        {
            if (SubscriptionBrowsers.Subscriptions.Any())
            {
                if (SubscriptionBrowsers.Subscriptions.ContainsKey(notificationContent.Browser))
                {
                    var pushSubscription = SubscriptionBrowsers.Subscriptions[notificationContent.Browser].ToPushSubscription();

                    var vapidDetails = new VapidDetails(this.ApplicationMailTo, this.ApplicationPublicKey, this.ApplicationPrivateKey);

                    var data = Newtonsoft.Json.JsonConvert.SerializeObject(notificationContent);

                    var webPushClient = new WebPushClient();
                    webPushClient.SendNotification(pushSubscription, data, vapidDetails);
                }
                else
                    throw new Exception("There is no subscription for this browser.");
            }
            else
                throw new Exception("There isn't any subscriptions");
        }



        public List<string> Browsers { get; set; }

        public string ApplicationPublicKey { get { return ConfigurationManager.AppSettings["NotificationAppPublicKey"]; } }

        public string ApplicationPrivateKey { get { return ConfigurationManager.AppSettings["NotificationAppPrivateKey"]; } }

        public string ApplicationMailTo { get { return ConfigurationManager.AppSettings["ApplicationMailTo"]; } }

        private string SubscriptionsFileName { get { return System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/Subscriptions.dat"); } }


        private SubscriptionBrowsers _SubscriptionBrowsers;
        public SubscriptionBrowsers SubscriptionBrowsers { get
            {
                if (_SubscriptionBrowsers == null)
                {
                    if (File.Exists(SubscriptionsFileName))
                        _SubscriptionBrowsers = BinarySerialization.ReadFromBinaryFile<SubscriptionBrowsers>(SubscriptionsFileName);
                    else
                        _SubscriptionBrowsers = new SubscriptionBrowsers();
                }

                return _SubscriptionBrowsers;
            }
        }

    }
}