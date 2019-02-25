# Setup
Before running you need to set 3 application keys it web.config file.

NotificationAppPublicKey and NotificationAppPrivateKey: These pair Public and Private key must be unique for your application. you can generate a pair through site: https://web-push-codelab.glitch.me/

ApplicationMailTo: an email to your application like "mailto:test@test.com" (it could be the application URL as well).

Reference: https://developers.google.com/web/fundamentals/push-notifications/

# Backend
Notifications in this example are being triggered by backend and it's using the Web Push API for that.

Github Web Push CSharp: https://github.com/web-push-libs/web-push-csharp/ Web Push CSharp License: https://github.com/web-push-libs/web-push-csharp/blob/master/LICENSE

# How to test
Check if browser supports service work;
Request browser's permission to receive notification for this site;
Subscribe to receive Notification (using different browsers will require different subscriptions). Subscribed browsers will appear in the "Select a browser" select;
Select a browser in which will receive notification, Title for notification, Message of the notification and an URL (this URL will redirect to the related page when user clicking on the notification.
