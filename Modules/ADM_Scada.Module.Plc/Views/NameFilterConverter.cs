using ADM_Scada.Core.Models;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace ADM_Scada.Modules.Plc.Views
{
    public class NameFilterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ObservableCollection<VariableModel> variables && parameter is string name)
            {
                return variables.FirstOrDefault(v => v.Name == name).Value;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
