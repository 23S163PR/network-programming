using System.ServiceProcess;
using log4net.Config;

namespace ChatServerService
{
	internal static class Program
	{
		/// <summary>
		///     The main entry point for the application.
		/// </summary>
		private static void Main()
		{
			XmlConfigurator.Configure();

			var servicesToRun = new ServiceBase[]
			{
				new ChatServerService()
			};
			ServiceBase.Run(servicesToRun);
		}
	}
}