using DtronixModel;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using DtronixModel.Attributes;

namespace DtronixModeler.Xaml
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


            Type[] types = { GetType(), typeof(TableAttribute) };

            _TxtVersionInfo.Text += "\r\n\r\n";
            foreach (var type in types)
            {
                var assembly = Assembly.GetAssembly(type);
                var assemblyInfo = new AssemblyInfo(assembly);

                /*
                var assembly = Assembly.GetAssembly(type);
                var version = assembly.GetName().Version.ToString();
                var title = assembly.GetName().FullName; */

                UriBuilder uri = new UriBuilder(assembly.CodeBase);
                var fullPath = Uri.UnescapeDataString(uri.Path);
                var fileName = Path.GetFileName(fullPath);

                _TxtVersionInfo.Text += string.Format("{0} ({1}) Version: {2}\r\n\tLocation: {3}\r\n\r\n", assemblyInfo.ProductTitle, fileName, assemblyInfo.Version, fullPath);
            }
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }



        private class AssemblyInfo
        {
            public AssemblyInfo(Assembly assembly)
            {
                if (assembly == null)
                    throw new ArgumentNullException("assembly");
                _assembly = assembly;
            }

            private readonly Assembly _assembly;

            /// <summary>
            /// Gets the title property
            /// </summary>
            public string ProductTitle {
                get {
                    return GetAttributeValue<AssemblyTitleAttribute>(a => a.Title,
                        Path.GetFileNameWithoutExtension(_assembly.CodeBase));
                }
            }

            /// <summary>
            /// Gets the application's version
            /// </summary>
            public string Version {
                get {
                    string result = string.Empty;
                    Version version = _assembly.GetName().Version;
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
                string> resolveFunc, string defaultResult = null) where TAttr : Attribute
            {
                object[] attributes = _assembly.GetCustomAttributes(typeof(TAttr), false);
                if (attributes.Length > 0)
                    return resolveFunc((TAttr)attributes[0]);
                else
                    return defaultResult;
            }
        }
    }


}
