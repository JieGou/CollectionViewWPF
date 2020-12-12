using System;
using System.Windows;
using System.Windows.Data;

namespace ViewJumperWPF.Convert
{
    public class BooleanVisiblityConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool convParameter = this.GetConverterParameter(parameter);
            bool selected = (bool)value;

            return convParameter == selected ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("Not Implemented");
        }

        #endregion IValueConverter Members

        private bool GetConverterParameter(object parameter)
        {
            try
            {
                bool convParameter = true;
                if (parameter != null)
                    convParameter = System.Convert.ToBoolean(parameter);
                return convParameter;
            }
            catch { return false; }
        }
    }
}