using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using wpf_commands.Infrastructure;

namespace wpf_commands
{
    public class RegisterPresenter
    {
        public RegisterModel Model { get; set; }

        public RegisterPresenter()
        {
            Model = new RegisterModel();
            Register = new DelegatingCommand(OnRegister, CanRegister);
        }

        public void Show()
        {
            RegisterWindow view = new RegisterWindow();
            view.DataContext = this;
            view.Show();
        }

        public ICommand Register { get; set; }

        private void OnRegister()
        {
            Task.Delay(TimeSpan.FromSeconds(2))
                .ContinueWith(t =>
                    MessageBox.Show("You have successfully registered!\nAll your base are belongs to us!"),
                    TaskScheduler.FromCurrentSynchronizationContext());
        }

        private Fact CanRegister
        {
            get
            {
                return new Fact(new INotifyPropertyChanged[] { Model.Login, Model.Password, Model.PasswordConfirmation }, 
                    () => 
                    !string.IsNullOrWhiteSpace(Model.Login)
                    && !string.IsNullOrWhiteSpace(Model.Password)
                    && Model.Password == Model.PasswordConfirmation);
            }
        }
    }
}
