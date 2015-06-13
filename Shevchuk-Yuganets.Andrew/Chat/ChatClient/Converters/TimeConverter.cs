using System;
using System.Globalization;
using System.Windows.Data;

namespace ChatClient.Converters
{
	public class TimeConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((DateTime) value).ToString("T");
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}