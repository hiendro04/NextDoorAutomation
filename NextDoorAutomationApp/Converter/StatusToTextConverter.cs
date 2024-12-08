using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NextDoorAutomationApp.Converter
{
    public class StatusToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int status)
            {
                switch (status)
                {
                    case 0: return "Not sent";
                    case 1: return "Sent";
                    case 2: return "Cancel";
                    default: return "Unknown";
                }
            }
            return "Unknown";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
