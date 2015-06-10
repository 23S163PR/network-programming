using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using wpf_commands.Infrastructure;

namespace wpf_commands
{
    public class RegisterModel
    {
        public Observable<string> Login { get; set; }

        // This should never be a string in a real world app. It should be SecureString.
        public Observable<string> Password { get;  set; }
        public Observable<string> PasswordConfirmation { get; set; }

        public RegisterModel()
        {
            Login = new Observable<string>(string.Empty);
            Password = new Observable<string>(string.Empty);
            PasswordConfirmation = new Observable<string>(string.Empty);
        }
    }
}
