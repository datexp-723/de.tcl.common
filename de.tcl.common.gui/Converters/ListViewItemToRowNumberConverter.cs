using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace de.tcl.common.gui.Converters
{
    public class ListViewItemToRowNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ListView listView = parameter as ListView;
            if (listView != null
                && value != null)
            {
                IList<object> items = listView.ItemsSource.Cast<object>().ToList();
                if (items != null)
                {
                    int index = items.IndexOf(value);
                    if (index > -1)
                    {
                        return string.Format("{0})", index + 1);
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
