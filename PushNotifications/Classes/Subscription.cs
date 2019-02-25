using PushNotifications.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPush;

namespace PushNotifications.Classes
{
    [Serializable]
    public class Subscription
    {
        public string endpoint { get; set; }
        public SubscriptionKeys keys { get; set; }

        public PushSubscription ToPushSubscription()
        {
            return new PushSubscription(endpoint, keys?.p256dh, keys?.auth);
        }
    }

    [Serializable]
    public class SubscriptionKeys
    {
        public string p256dh { get; set; }
        public string auth { get; set; }
    }
}