using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_commands.Infrastructure
{
    public class Observable<T> : NotifyPropertyChangedBase
    {
        private T value;

        public Observable() 
        {
        }

        public Observable(T value)
        {
            this.value = value;
        }

        public T Value
        {
            get { return value; }
            set
            {
                Set(ref this.value, value);
            }
        }

        public static implicit operator T(Observable<T> val)
        {
            return val.value;
        }

        public static bool operator ==(Observable<T> left, Observable<T> right)
        {
            return EqualityComparer<T>.Default.Equals(left.Value, right.Value);
        }

        public static bool operator !=(Observable<T> left, Observable<T> right)
        {
            return !(left == right);
        }
    }
}
