using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace RemindMe.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }


        protected bool SetProperty<T>(ref T member, T value, string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(member, value))
                return false;

            member = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }
    }
}
