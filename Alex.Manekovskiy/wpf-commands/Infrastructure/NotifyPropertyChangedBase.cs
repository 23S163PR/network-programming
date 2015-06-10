using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace wpf_commands.Infrastructure
{
    public class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void Set<T>(ref T value, T newValue, [CallerMemberName]string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(value, newValue)) return;

            value = newValue;
            InvokePropertyChanged(propertyName);
        }

        private void InvokePropertyChanged(string propertyName)
        {
            if (PropertyChanged == null) return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
