using System.ServiceProcess;

namespace ChatServerService
{
	internal static class Program
	{
		/// <summary>
		///     The main entry point for the application.
		/// </summary>
		private static void Main()
		{
			var servicesToRun = new ServiceBase[]
			{
				new ChatServerService()
			};
			ServiceBase.Run(servicesToRun);
		}
	}
}