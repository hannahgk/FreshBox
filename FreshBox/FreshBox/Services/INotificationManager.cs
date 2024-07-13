using System;

namespace FreshBox.Services
{
    public interface INotificationManager
    {
        event EventHandler NotificationReceived;
        event EventHandler NotificationSent;
        void Initialize();
        void SendNotification(string title, string message, DateTime? notifyTime = null);
        void ReceiveNotification(string title, string message);
    }
}
