using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PushNotifications.Classes
{
    public class NotificationContent
    {
        public string Browser { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public string URL { get; set; }
        public string Icon { get; set; }
    }
}