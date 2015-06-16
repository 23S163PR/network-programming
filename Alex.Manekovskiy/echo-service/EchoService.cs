using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace EchoService
{
    public partial class EchoService : ServiceBase
    {
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly TcpListener listener;
        private readonly int port;
        private readonly int maxClients;

        private const int MaxMessageSizeInBytes = 1024;

        public EchoService()
        {
            InitializeComponent();

            var portValue = ConfigurationManager.AppSettings["Port"];
            var maxClientsValue = ConfigurationManager.AppSettings["MaxClients"];

            if (!int.TryParse(portValue, out port))
                throw new ConfigurationErrorsException("The port value is invalid. Please check your configuration file.");
            
            if (!int.TryParse(maxClientsValue, out maxClients))
                throw new ConfigurationErrorsException("The maximum clients value is invalid. Please check your configuration file.");

            listener = new TcpListener(IPAddress.Any, port);
        }

        protected override void OnStart(string[] args)
        {
            logger.Debug("Service starting...");
            Task.Run(() =>
            {
                listener.Start(maxClients);

                while (true)
                {
                    var buffer = new byte[MaxMessageSizeInBytes];

                    var client = listener.AcceptTcpClient();

                    Task.Run(() => 
                    {
                        var networkStream = client.GetStream();
                        var receivedBytes = networkStream.Read(buffer, 0, client.Available);

                        if (receivedBytes > 0)
                        {
                            logger.InfoFormat("NEW MESSAGE: {0}", Encoding.ASCII.GetString(buffer, 0, receivedBytes));
                        }
                        else
                        {
                            logger.Error("FAILED TO RECEIVE MESSAGE");
                        }
                    });
                }
            });
            logger.Debug("Service started.");
        }

        protected override void OnStop()
        {
            logger.Debug("Service stopping.");
            listener.Stop();
            logger.Debug("Service stopped.");
        }

        protected override void OnShutdown()
        {
            base.OnShutdown();
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        protected override void OnContinue()
        {
            base.OnContinue();
        }

        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            return base.OnPowerEvent(powerStatus);
        }

        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            base.OnSessionChange(changeDescription);
        }
    }
}
