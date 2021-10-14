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

        public delegate void TimerElapsedEventHandler(Timer timer);
        public event TimerElapsedEventHandler OnTimerElapsed;

        private NotificationManager()
        {
            NotificationTimer = new Timer
            {
                Interval = 60 * 60 * 1000,
                AutoReset = true,
            };
            NotificationTimer.Elapsed += NotificationTimer_Elapsed;
            ClearNotificationTimer = new Timer
            {
                Interval = 7000, // ToastDuration.Short
            };
            ClearNotificationTimer.Elapsed += ClearNotificationTimer_Elapsed;
        }

        public void NotificationTimer_Elapsed(object sender, ElapsedEventArgs e) => ShowNotification();
        public void ShowNotification()
        {
            new ToastContentBuilder()
                .AddText("Check your posture!")
                .SetToastDuration(ToastDuration.Short)
                .Show();
            ClearNotificationTimer.Start();
            OnTimerElapsed?.Invoke(NotificationTimer);
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
            OnTimerElapsed?.Invoke(NotificationTimer);
        }
        public void StopTimer()
        {
            NotificationTimer.Stop();
        }
    }
}
