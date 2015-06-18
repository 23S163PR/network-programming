using System.ComponentModel;
using System.Configuration.Install;

namespace ChatServerService
{
	[RunInstaller(true)]
	public partial class ProjectInstaller : Installer
	{
		public ProjectInstaller()
		{
			InitializeComponent();
		}

		private void serviceInstaller_AfterInstall(object sender, InstallEventArgs e)
		{

		}
	}
}