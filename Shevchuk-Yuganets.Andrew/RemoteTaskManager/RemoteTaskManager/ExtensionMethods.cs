using System.Windows.Controls;
using Lib;

namespace RemoteTaskManager
{
	public static class DataGridExtension
	{
		public static int GetSelectedProcessId(this DataGrid dataGrid)
		{
			return (dataGrid.SelectedItem as ProcessModel).ProcessId;
		}
	}
}