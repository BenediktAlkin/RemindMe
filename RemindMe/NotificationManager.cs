using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using Windows.UI.Notifications;

namespace RemindMe
{
    public class NotificationManager
    {
        public static NotificationManager Instance { get; } = new NotificationManager();
        private NotificationManager()
        {
            NotificationTimer = new Timer
            {
                Interval = 60 * 60 * 1000,
                AutoReset = true,
            };
            NotificationTimer.Elapsed += ShowNotification;
            ClearNotificationTimer = new Timer
            {
                Interval = 7000, // ToastDuration.Short
            };
            ClearNotificationTimer.Elapsed += ClearNotificationTimer_Elapsed;
        }

        private void ShowNotification(object sender, ElapsedEventArgs e)
        {
            new ToastContentBuilder()
                .AddText("Check your posture!")
                .SetToastDuration(ToastDuration.Short)
                .Show();
            ClearNotificationTimer.Start();
        }
        private void ClearNotificationTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ClearNotifications();
            ClearNotificationTimer.Stop();
        }
        public void ClearNotifications()
        {
            ToastNotificationManagerCompat.History.Clear();
        }

        private Timer NotificationTimer { get; }
        private Timer ClearNotificationTimer { get; }
        public void StartTimer()
        {
            NotificationTimer.Start();
        }
        public void StopTimer()
        {
            NotificationTimer.Stop();
        }
    }
}
