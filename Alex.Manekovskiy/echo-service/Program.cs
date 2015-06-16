using log4net.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace EchoService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            XmlConfigurator.Configure();
#if DEBUG
            // This is not considered a good practice and here is only for reference purpose. 
            // Ideally we should extract our service logic to the separate class with public API that
            // could be called directly from the host application.
            var echoService = new EchoService();

            MethodInfo onStart = typeof(EchoService).GetMethod("OnStart", BindingFlags.NonPublic | BindingFlags.Instance);
            onStart.Invoke(echoService, new object[] { null });

            Task.Run(async () => 
            {
                while (true)
                {
                    await Task.Delay(10000);
                }
            })
            .Wait();
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new EchoService() 
            };
            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}
