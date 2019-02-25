using PushNotifications.Classes;
using PushNotifications.Models;
using System;
using System.Configuration;
using System.IO;
using System.Web.Mvc;
using WebPush;

namespace PushNotifications.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = new PushNotificationModel();

            return View(model);
        }
        
        public JsonResult SaveSubscription(string pushSubscription)
        {
            bool success = false;
            string error = "error";

            try
            {
                var model = new PushNotificationModel();

                model.SaveSubscription(pushSubscription, base.Request.Browser.Browser);
                success = true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return Json(new { Success = success, Message = error });
        }

        public JsonResult FireNotification(NotificationContent notificationContent)
        {
            bool success = false;
            string error = "Error firing push notification";

            try
            {
                var model = new PushNotificationModel();
                model.FireNotification(notificationContent);
                success = true;
            }
            catch (WebPushException e)
            {
                error = $"Error pushing notification: {e.Message}";
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return Json(new { Success = success, Message = error });
        }        
    }

}