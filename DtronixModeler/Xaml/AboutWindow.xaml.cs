using DtronixModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DtronixModeler.Xaml {
	/// <summary>
	/// Interaction logic for AboutWindow.xaml
	/// </summary>
	public partial class AboutWindow : Window {
		public AboutWindow() {
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) {


			Type[] types = new Type[] { GetType(), typeof(Model) };

			_TxtVersionInfo.Text += "\r\n\r\n";
            foreach (var type in types) {
				var assembly = Assembly.GetAssembly(type);
                var assembly_info = new AssemblyInfo(assembly);

				/*
				var assembly = Assembly.GetAssembly(type);
				var version = assembly.GetName().Version.ToString();
				var title = assembly.GetName().FullName; */

				UriBuilder uri = new UriBuilder(assembly.CodeBase);
				var full_path = Uri.UnescapeDataString(uri.Path);
				var file_name = System.IO.Path.GetFileName(full_path);

				_TxtVersionInfo.Text += string.Format("{0} ({1}) Version: {2}\r\n\tLocation: {3}\r\n\r\n", assembly_info.ProductTitle, file_name, assembly_info.Version, full_path);
			}
		}

		private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e) {
			Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
			e.Handled = true;
		}



		private class AssemblyInfo {
			public AssemblyInfo(Assembly assembly) {
				if (assembly == null)
					throw new ArgumentNullException("assembly");
				this.assembly = assembly;
			}

			private readonly Assembly assembly;

			/// <summary>
			/// Gets the title property
			/// </summary>
			public string ProductTitle {
				get {
					return GetAttributeValue<AssemblyTitleAttribute>(a => a.Title,
							Path.GetFileNameWithoutExtension(assembly.CodeBase));
				}
			}

			/// <summary>
			/// Gets the application's version
			/// </summary>
			public string Version {
				get {
					string result = string.Empty;
					Version version = assembly.GetName().Version;
					if (version != null)
						return version.ToString();
					else
						return "1.0.0.0";
				}
			}

			/// <summary>
			/// Gets the description about the application.
			/// </summary>
			public string Description {
				get { return GetAttributeValue<AssemblyDescriptionAttribute>(a => a.Description); }
			}


			/// <summary>
			///  Gets the product's full name.
			/// </summary>
			public string Product {
				get { return GetAttributeValue<AssemblyProductAttribute>(a => a.Product); }
			}

			/// <summary>
			/// Gets the copyright information for the product.
			/// </summary>
			public string Copyright {
				get { return GetAttributeValue<AssemblyCopyrightAttribute>(a => a.Copyright); }
			}

			/// <summary>
			/// Gets the company information for the product.
			/// </summary>
			public string Company {
				get { return GetAttributeValue<AssemblyCompanyAttribute>(a => a.Company); }
			}

			protected string GetAttributeValue<TAttr>(Func<TAttr,
				string> resolveFunc, string defaultResult = null) where TAttr : Attribute {
				object[] attributes = assembly.GetCustomAttributes(typeof(TAttr), false);
				if (attributes.Length > 0)
					return resolveFunc((TAttr)attributes[0]);
				else
					return defaultResult;
			}
		}
	}

	
}
