using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_commands.Infrastructure
{
    public class Fact : INotifyPropertyChanged
    {
        private readonly Func<bool> predicate;

        public Fact(INotifyPropertyChanged[] observables, Func<bool> predicate)
        {
            this.predicate = predicate;
            foreach (var observable in observables)
            {
                observable.PropertyChanged += (sender, args) => PropertyChanged(this, new PropertyChangedEventArgs("Value"));
            }
        }

        public bool Value
        {
            get
            {
                return predicate();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
