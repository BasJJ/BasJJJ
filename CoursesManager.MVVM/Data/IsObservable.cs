using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace CoursesManager.MVVM.Data
{
    public abstract class IsObservable : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (Application.Current?.Dispatcher?.CheckAccess() == true)
            {
                PropertyChanged?.Invoke(this, e);
            }
            else
            {
                Application.Current?.Dispatcher.Invoke(() => PropertyChanged?.Invoke(this, e));
            }
        }
    }
}