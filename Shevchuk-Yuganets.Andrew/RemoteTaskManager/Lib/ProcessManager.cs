using System;
using System.Diagnostics;

namespace Lib
{
	public static class ProcessManager
	{
		private static Process GetProcess(int id)
		{
			return Process.GetProcessById(id);
		}

		public static ProcessPriorityClass GetProcessPriority(int id)
		{
			return GetProcess(id).PriorityClass;
		}

		public static void SetProcessPriority(int id, ProcessPriorityClass priority)
		{
			try
			{
				GetProcess(id).PriorityClass = priority;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public static void KillProcess(int id)
		{
			try
			{
				GetProcess(id).Kill();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
	}
}