using RemindMe.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;

namespace RemindMe.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {

        public MainWindowViewModel()
        {
            NotificationManager.Instance.OnTimerElapsed += Instance_OnTimerElapsed;
        }

        #region update viewmodel timer
        private void Instance_OnTimerElapsed(System.Timers.Timer timer)
        {
            NextTimerTick = DateTime.Now.AddMilliseconds(timer.Interval);
        }
        private DateTime nextTimerTick;
        public DateTime NextTimerTick
        {
            get => nextTimerTick;
            set
            {
                SetProperty(ref nextTimerTick, value, nameof(NextTimerTick));
                NotifyPropertyChanged(nameof(TimeTillNextTimerTick));
            }
        }
        public TimeSpan TimeTillNextTimerTick => NextTimerTick - DateTime.Now ;
        #endregion

        #region update ui
        private Task UpdateTask { get; set; }
        private CancellationTokenSource UpdateTaskTokenSource { get; set; }
        public void StartUpdateTask()
        {
            if (UpdateTask != null) return;

            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            UpdateTaskTokenSource = tokenSource;
            
            UpdateTask = Task.Run(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    NotifyPropertyChanged(nameof(TimeTillNextTimerTick));
                    Task.Delay(1000);
                }
            }, tokenSource.Token);
        }
        public void StopUpdateTask()
        {
            if (UpdateTask == null) return;

            UpdateTaskTokenSource.Cancel();
            UpdateTask = null;
        }
        #endregion

        public ICommand TestNotificationCommand { get; } = new CommandImpl(sender => NotificationManager.Instance.ShowNotification());
    }
}
