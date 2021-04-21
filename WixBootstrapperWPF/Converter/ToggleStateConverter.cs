using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace WixBootstrapperWPF.Converter {
    public class ToggleStateConverter : IValueConverter {

        public object Status1 { get; set; }
        public object Status2 { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return (value is bool state && state) ? Status1 : Status2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
