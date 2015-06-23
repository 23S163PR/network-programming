using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Lib;

namespace ChatClient
{
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