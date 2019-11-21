using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MedGUI
{
    public class MedicineConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(Int32))
            {
                if (System.Convert.ToInt32(value) == 0)
                {
                    return string.Empty;
                }
                else
                {
                    return value.ToString();
                }
            }
            else
            {
                if (System.Convert.ToDouble(value) == 0)
                {
                    return string.Empty;
                }
                else
                {
                    return value.ToString();
                }
            }

            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strValue = (value as string).Replace(",", ".");
            if (string.IsNullOrEmpty(strValue))
                return 0;
            if (targetType == typeof(Int32))
            {
                try
                {
                    return Int32.Parse(strValue);
                }
                catch
                {
                    return 0;
                }
            }
            else
            {
                try
                {
                    return double.Parse(strValue, CultureInfo.InvariantCulture);
                }
                catch
                {
                    return 0.0;
                }
            }
        }
    }
}
