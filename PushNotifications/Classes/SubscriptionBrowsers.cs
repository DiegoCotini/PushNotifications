using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PushNotifications.Classes
{
    [Serializable]
    public class SubscriptionBrowsers
    {
        public List<string> Browsers { get; set; }
        public Dictionary<string, Subscription> Subscriptions { get; set; }

        public SubscriptionBrowsers()
        {
            Browsers = new List<string>();
            Subscriptions = new Dictionary<string, Subscription>();
        }
    }
}