using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ChatClient.Converters
{
	public class AvatarConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			try
			{
				var image = new BitmapImage();
				using (var byteStream = new MemoryStream((byte[]) value))
				{
					image.BeginInit();
					image.StreamSource = byteStream;
					image.EndInit();
				}

				return image;
			}
			catch
			{
				return new BitmapImage();
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}