using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using sconvert = System.Convert;

namespace WixBootstrapperWPF.Converter {


    public class EnumConverter : IValueConverter {


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            try {
                return value.Equals( parameter )? Visibility.Visible : Visibility.Collapsed;
            }catch(Exception ex) {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is bool flag) {
                if (flag)
                    return parameter;
            }
            return null;
        }
    }
}
