using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Cerberus.ViewModel
{
    public abstract class BaseObservableViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Dictionary<string, object> propertyValues = new Dictionary<string, object>();

        protected T GetProperty<T>([CallerMemberName] string propertyName = null)
        {
            if (propertyValues.ContainsKey(propertyName))
                return (T)propertyValues[propertyName];
            return default(T);
        }
        protected bool SetProperty<T>(T value, [CallerMemberName] string propertyName = null)
        {
            T myVar = GetProperty<T>(propertyName);
            if (!EqualityComparer<T>.Default.Equals(myVar, value))
            {
                propertyValues[propertyName] = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }
            return false;
        }
    }
}
