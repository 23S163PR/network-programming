using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Serialization;
using Lib;

namespace ChatClient
{
	/// <summary>
	///     Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private Config _config;

		private readonly TcpClient _clientSocket = new TcpClient();
		private NetworkStream _serverStream = default(NetworkStream);

		public ObservableCollection<Message> MessageList { get; } = new ObservableCollection<Message>();

		public MainWindow()
		{
			LoadConfigXml();
			ConnectToServer();

            InitializeComponent();
			DataContext = this;
		}

		private void LoadConfigXml()
		{
			var xml = new XmlSerializer(typeof(Config));
			try
			{
				using (var fileStream = new FileStream("Config.xml", FileMode.Open))
				{
					_config = (Config)xml.Deserialize(fileStream);
					fileStream.Flush();
				}
			}
			catch
			{
				_config = new Config
				{
					ServerIp = "127.0.0.1",
					ServerPort = 8888,
					UserAvatarPath = @"avatar.jpeg",
					UserName = "Mazillka"
				};

				using (var fileStream = new FileStream("Config.xml", FileMode.Create))
				{
					xml.Serialize(fileStream, _config);
					fileStream.Flush();
				}
				Trace.WriteLine("Created Config.xml File");
			}
		}

		public void ConnectToServer()
		{
			try
			{
				_clientSocket.Connect(_config.ServerIp, _config.ServerPort);
				_serverStream = _clientSocket.GetStream();

				var chatThread = new Thread(GetMessage);
				chatThread.Start();
			}
			catch
			{
				_clientSocket.Close();
				_serverStream.Dispose();
                MessageBox.Show("Can't connect");
			}
		}

		private void GetMessage()
		{
			try
			{
				while (true)
				{
					_serverStream = _clientSocket.GetStream();

					var buffer = new byte[GlobalConfig.MaxMessageSizeInBytes];
					_serverStream.Read(buffer, 0, _clientSocket.ReceiveBufferSize);

					var message = new Message().BytesDeserializeToMessage(buffer);

					ShowMessage(message);
				}
			}
			catch
			{
				_clientSocket.Close();
				_serverStream.Dispose();
                MessageBox.Show("Connection Lost");
			}
		}

		private void ShowMessage(Message message)
		{
			ChatItemsControl.CheckAppendMessage(MessageList, message);
		}

		private void SendMessageButton_OnClick(object sender, EventArgs e)
		{
			SendMessage();
		}

		private void SendMessage()
		{
			if (!_clientSocket.Connected)
				return;

			var message = new Message
			{
				Avatar = File.ReadAllBytes("avatar.jpeg"),
				Name = _config.UserName,
				Text = MessageTextBox.Text,
				Time = DateTime.Now
			};

			var byteData = message.SerializeToBytes();
			_serverStream.Write(byteData, 0, byteData.Length);
			_serverStream.Flush();

			MessageTextBox.Clear();
		}

		private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				SendMessage();
			}
		}
	}



	/// <summary>
	/// 
	/// </summary>

	public static class ItemControlExtensions
	{
		public static void CheckAppendMessage(this ItemsControl control, ObservableCollection<Message> list, Message message,
			bool waitUntilReturn = false)
		{
			Action append = () => list.Add(message);
			if (control.CheckAccess())
			{
				append();
			}
			else if (waitUntilReturn)
			{
				control.Dispatcher.Invoke(append);
			}
			else
			{
				control.Dispatcher.BeginInvoke(append);
			}
		}
	}

	// TODO:
	public class ScrollViewerExtenders : DependencyObject
	{
		public static readonly DependencyProperty AutoScrollToEndProperty =
			DependencyProperty.RegisterAttached("AutoScrollToEnd", typeof (bool), typeof (ScrollViewerExtenders),
				new UIPropertyMetadata(default(bool), OnAutoScrollToEndChanged));

		/// <summary>
		///     Returns the value of the AutoScrollToEndProperty
		/// </summary>
		/// <param name="obj">The dependency-object whichs value should be returned</param>
		/// <returns>The value of the given property</returns>
		public static bool GetAutoScrollToEnd(DependencyObject obj)
		{
			return (bool) obj.GetValue(AutoScrollToEndProperty);
		}

		/// <summary>
		///     Sets the value of the AutoScrollToEndProperty
		/// </summary>
		/// <param name="obj">The dependency-object whichs value should be set</param>
		/// <param name="value">The value which should be assigned to the AutoScrollToEndProperty</param>
		public static void SetAutoScrollToEnd(DependencyObject obj, bool value)
		{
			obj.SetValue(AutoScrollToEndProperty, value);
		}

		/// <summary>
		///     This method will be called when the AutoScrollToEnd
		///     property was changed
		/// </summary>
		/// <param name="s">The sender (the ListBox)</param>
		/// <param name="e">Some additional information</param>
		public static void OnAutoScrollToEndChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			var scrollViewer = obj as ScrollViewer;

			var handler = new SizeChangedEventHandler((_, __) => { scrollViewer.ScrollToEnd(); });

			if ((bool) e.NewValue)
				scrollViewer.SizeChanged += handler;
			else
				scrollViewer.SizeChanged -= handler;
		}
	}
}