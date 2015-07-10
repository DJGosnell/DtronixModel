using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace DtronixModeler.Xaml {
	public class BooleanToVisibiltyConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value == null)
				throw new ArgumentNullException("value");

			if (!(value is bool))
				throw new ArgumentException("Expected type of value must be boolean");

			if (parameter != null && !(parameter is Visibility))
				throw new ArgumentException("Expected type of parameter must be Visibility");

			Visibility falseVisibility = Visibility.Collapsed;

			if (parameter != null) {
				falseVisibility = (Visibility)parameter;

				if (falseVisibility == Visibility.Visible) {
					throw new ArgumentException("Cannot pass visible to expected false value parameter.");
				}
			}

			return (bool)value ? Visibility.Visible : falseVisibility;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value == null)
				throw new ArgumentNullException("value");

			if (!(value is Visibility))
				throw new ArgumentException("Expected type of value must be Visibility");

			if (value == null) {
				throw new ArgumentNullException("value");
			}

			Visibility v = (Visibility)value;

			return v == Visibility.Visible;
		}
	}
}
