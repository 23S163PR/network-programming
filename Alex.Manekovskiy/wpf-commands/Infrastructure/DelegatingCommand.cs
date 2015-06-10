using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace wpf_commands.Infrastructure
{
    public class DelegatingCommand : ICommand
    {
        private Action action;
        private Fact canExecuteFact;

        public event EventHandler CanExecuteChanged;

        public DelegatingCommand(Action action, Fact canExecuteFact)
        {
            this.action = action;

            if (canExecuteFact == null) return;

            var dispatcher = Dispatcher.CurrentDispatcher;
            this.canExecuteFact = canExecuteFact;
            this.canExecuteFact.PropertyChanged += (s, e) =>
            {
                if (CanExecuteChanged != null)
                {
                    dispatcher.Invoke(() => CanExecuteChanged(s, EventArgs.Empty));
                }
            };
        }

        public void Execute(object parameter)
        {
            action();
        }

        public bool CanExecute(object parameter)
        {
            return canExecuteFact != null ? canExecuteFact.Value : true;
        }
    }
}
