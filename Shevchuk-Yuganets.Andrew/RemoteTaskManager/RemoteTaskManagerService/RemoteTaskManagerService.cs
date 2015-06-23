using System.ServiceProcess;

namespace RemoteTaskManagerService
{
	public partial class RemoteTaskManagerService : ServiceBase
	{
		public RemoteTaskManagerService()
		{
			InitializeComponent();
		}

		protected override void OnStart(string[] args)
		{
		}

		protected override void OnStop()
		{
		}
	}
}