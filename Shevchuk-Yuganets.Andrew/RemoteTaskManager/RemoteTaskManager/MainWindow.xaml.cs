using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Net.Sockets;
using System.Timers;
using System.Windows;
using Lib;

namespace RemoteTaskManager
{
	/// <summary>
	///     Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly bool _offline = bool.Parse(ConfigurationManager.AppSettings["Offline"]);
		private readonly ObservableCollection<ProcessModel> _processList = new ObservableCollection<ProcessModel>();
		private readonly Timer _timer;
		private ObservableCollection<ProcessModel> _wmiProcessList;

		public MainWindow()
		{
			InitializeComponent();

			_timer = new Timer(100); // 1000ms - 1sec
			_timer.Elapsed += timer_Tick;
			_timer.Enabled = true;
		}

		private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
		{
			ProcessDataGrid.ItemsSource = _processList;

			_timer.Start();
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			UpdateList();
		}

		private void UpdateList()
		{
			var dispather = Application.Current.Dispatcher;

			if (_offline)
			{
				_wmiProcessList = WmiManager.GetProcessList();
			}
			else
			{
				var client = new TcpClient();
				client.Connect(NetworkSettings.ServerIp, NetworkSettings.ServerPort);

				var stream = client.GetStream();

				var buffer = new byte[NetworkSettings.MaxMessageSizeInBytes];

				stream.Read(buffer, 0, client.ReceiveBufferSize); // ???

				_wmiProcessList = GlobalMethods.DeserializeBytesToMessage(buffer); // ???

				MessageBox.Show(_wmiProcessList[0].Name);

				stream.Dispose();
				stream.Flush();

				client.Close();
			}

			foreach (var process in _wmiProcessList)
			{
				var tmpProcess = _processList.FirstOrDefault(pr => pr.ProcessId == process.ProcessId);
				if (tmpProcess != null)
				{
					dispather.Invoke(() =>
					{
						tmpProcess.CpuUsage = process.CpuUsage;
						tmpProcess.MemoryUsage = process.MemoryUsage;
						tmpProcess.Threads = process.Threads;
					});
				}
				else
				{
					dispather.Invoke(() => _processList.Add(process));
				}
			}

			var closedProcess = new List<int>();
			foreach (var process in _processList)
			{
				var tmpProcess = _wmiProcessList.FirstOrDefault(pr => pr.ProcessId == process.ProcessId);
				if (tmpProcess == null)
				{
					closedProcess.Add(process.ProcessId);
				}
			}

			foreach (var processId in closedProcess)
			{
				dispather.Invoke(() => _processList.Remove(_processList.FirstOrDefault(pr => pr.ProcessId == processId)));
			}
		}

		private void EndProcess_Click(object sender, RoutedEventArgs e)
		{
			if (MessageBox.Show("Kill Process?", "Kill Process", MessageBoxButton.OKCancel, MessageBoxImage.Warning) ==
			    MessageBoxResult.OK)
			{
				ProcessManager.KillProcess(ProcessDataGrid.GetSelectedProcessId());
			}
		}

		private void ChangePriority_Click(object sender, RoutedEventArgs e)
		{
			// This is guaranteed to be PriorityMenuItem
			var priorityMenuItem = (PriorityMenuItem) sender;
			ProcessManager.SetProcessPriority(ProcessDataGrid.GetSelectedProcessId(), priorityMenuItem.PriorityValue);
		}

		private void ContextMenu_Opening(object sender, RoutedEventArgs e)
		{
			_timer.Stop();

			var currentPriority = ProcessManager.GetProcessPriority(ProcessDataGrid.GetSelectedProcessId());
			foreach (PriorityMenuItem menuItem in PriorityMenuItem.Items)
			{
				menuItem.IsChecked = menuItem.PriorityValue == currentPriority;
			}
		}

		private void ContextMenu_OnClosed(object sender, RoutedEventArgs e)
		{
			_timer.Start();
		}
	}
}