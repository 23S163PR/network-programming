using System.Diagnostics;
using System.Windows.Controls;

namespace RemoteTaskManager
{
	internal class PriorityMenuItem : MenuItem
	{
		public ProcessPriorityClass PriorityValue { get; set; }
	}
}