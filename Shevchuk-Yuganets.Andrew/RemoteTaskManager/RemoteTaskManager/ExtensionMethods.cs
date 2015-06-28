using System;
using System.Windows.Controls;
using Lib;

namespace RemoteTaskManager
{
	public static class DataGridExtension
	{
		public static int GetSelectedProcessId(this DataGrid dataGrid)
		{
			try
			{
				return (dataGrid.SelectedItem as ProcessModel).ProcessId;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
	}
}