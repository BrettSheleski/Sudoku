using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sudoku
{
    public abstract class Observable : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanging([CallerMemberName]string propertyName = null)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        protected virtual void Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            Set(ref field, value, null, propertyName);
        }

        protected virtual void Set<T>(ref T field, T value, Action callbackAction, [CallerMemberName] string propertyName = null)
        {
            if (!System.Collections.Generic.EqualityComparer<T>.Default.Equals(field, value))
            {
                OnPropertyChanging(propertyName);

                field = value;

                OnPropertyChanged(propertyName);

                callbackAction?.Invoke();
            }
        }
    }
}