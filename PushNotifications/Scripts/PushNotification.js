var pushNotification = {
    appPublicKey: null,
    serviceWorkerRegistration: null,

    start: function (applicationPublicKey) {
        pushNotification.appPublicKey = applicationPublicKey;

        if (pushNotification.validateBrowserPermissions()) {
            pushNotification.serviceWorkerRegistration = navigator.serviceWorker.register('/Scripts/service-worker.js')
                .catch(function (err) {
                    console.error('Unable to register service-worker.', err);
                });
        }
    },
    
    validateBrowserPermissions: function() {
        if (!('serviceWorker' in navigator)) {
            alert("Service Worker isn't supported on this browser, disable or hide UI.");
            return false;
        }

        if (!('PushManager' in window)) {
            alert("Push isn't supported on this browser, disable or hide UI.");
            return false;
        }

        return true;
    },

    askPermissionPromise: function() {
        return new Promise(function (resolve, reject) {
            const permissionResult = Notification.requestPermission(function (result) {
                resolve(result);
            });

            if (permissionResult) {
                permissionResult.then(resolve, reject);
            }
        })
        .then(function (permissionResult) {
            if (permissionResult !== 'granted') {
                alert('We weren\'t granted permission.');
                return false;
            }

            alert('User granted permission.');

            return true;
        });
    },

    //this is deprecated but few browser may support this option instead of using promisse
    askPermissionCallback: function() {
        Notification.requestPermission().then(function (result) {
            if (result === 'denied') {
                alert('Permission wasn\'t granted. Allow a retry.');
                return false;
            }
            if (result === 'default') {
                alert('The permission request was dismissed.');
                return false;
            }

            alert('User granted permission.');

            console.log('The permission was granted.');
            return true;
        });
    },

    askPermission: function () {
        try {
            hasPermission = pushNotification.askPermissionPromise();
        } catch (e) {
            hasPermission = pushNotification.askPermissionCallback();
        }
    },

    subscribeUserToPush: function (onSuccess) {
        if (pushNotification.validateBrowserPermissions()) {
            return navigator.serviceWorker.register('/Scripts/service-worker.js')
                .then(function (registration) {
                    const subscribeOptions = {
                        userVisibleOnly: true,
                        applicationServerKey: pushNotification.urlBase64ToUint8Array(pushNotification.appPublicKey)
                    };

                    return registration.pushManager.subscribe(subscribeOptions);
                }).catch(function (err) {
                    console.error('Unable to register service-worker.', err);
                    return false;
                })
                .then(function (pushSubscription) {
                    console.log('Received PushSubscription: ', JSON.stringify(pushSubscription));

                    if (pushSubscription) {
                        $.post("/Home/SaveSubscription", { pushSubscription: JSON.stringify(pushSubscription) }, function (data) {
                            if (data.Success) {
                                alert('Subscription saved.');
                                
                                if ($.isFunction(onSuccess))
                                    onSuccess();
                            }
                            else
                                alert('Error Subscribing: ' + data.Message);
                        });
                    }

                    return pushSubscription;
                });
        }
    },

    fireMessage: function (message, browser, url, title) {
        $.post("/Home/FireNotification", { Message: message, Browser: browser, URL: url, Title: title, Icon: "images/icon.png" }, function (data) {
            if (!data.Success)
                alert('Error firing notification: ' + data.Message);
        });
    },

    urlBase64ToUint8Array: function(base64String) {
        var padding = '='.repeat((4 - base64String.length % 4) % 4);
        var base64 = (base64String + padding)
            .replace(/\-/g, '+')
            .replace(/_/g, '/');

        var rawData = window.atob(base64);
        var outputArray = new Uint8Array(rawData.length);

        for (var i = 0; i < rawData.length; ++i) {
            outputArray[i] = rawData.charCodeAt(i);
        }
        return outputArray;
    }
}

