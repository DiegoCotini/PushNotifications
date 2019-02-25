
self.addEventListener('push', function (event) {

    var data = {};
    if (event.data) {
        data = event.data.json();
    }

    const title = data.Title;
    const options = {
        body: data.Message,
        icon: data.Icon,
        badge: 'images/badge.png',
        data: {
            url: data.URL,
        },
    };

    event.waitUntil(self.registration.showNotification(title, options));
});

self.addEventListener('notificationclick', function (event) {
    event.notification.close();

    let clickResponsePromise = Promise.resolve();
    if (event.notification.data && event.notification.data.url) {
        clickResponsePromise = clients.openWindow(event.notification.data.url);
    }

    event.waitUntil(
        Promise.all([
            clickResponsePromise,
            self.analytics.trackEvent('notification-click'),
        ])
    );
});


