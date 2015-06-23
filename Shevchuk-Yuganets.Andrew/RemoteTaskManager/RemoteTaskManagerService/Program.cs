using System.ServiceProcess;

namespace RemoteTaskManagerService
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
				new RemoteTaskManagerService()
			};
			ServiceBase.Run(servicesToRun);
		}
	}
}