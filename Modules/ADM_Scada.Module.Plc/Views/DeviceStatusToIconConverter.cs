using ADM_Scada.Cores.PlcService;
using MaterialDesignThemes.Wpf;
using System;
using System.Globalization;
using System.Windows.Data;

namespace ADM_Scada.Modules.Plc.Views
{
    public class DeviceStatusToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DeviceStatus status)
            {
                switch (status)
                {
                    case DeviceStatus.NotPresent:
                        return PackIconKind.TelevisionOff;
                    case DeviceStatus.DisConnect:
                        return PackIconKind.LanDisconnect;
                    case DeviceStatus.Connecting:
                        return PackIconKind.Connection;
                    case DeviceStatus.Connected:
                        return PackIconKind.LanConnect;
                    default:
                        return PackIconKind.Help;
                }
            }

            return PackIconKind.Help;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
