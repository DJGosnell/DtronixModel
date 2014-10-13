using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace DtxModeler.Ddl {
	public partial class Table {

	}

	public partial class Database {
		[XmlIgnore]
		public bool _Modified;

		[XmlIgnore]
		public string _FileLocation;

		[XmlIgnore]
		public TreeViewItem _TreeRoot;

		private bool initialized = false;

		public T GetConfiguration<T>(string property, T default_value) {
			Initialize();

			property = property.ToLower();
			foreach (var config in this.configurationField) {
				if (config.Name == property) {
					try {
						return (T)Convert.ChangeType(config.Value, typeof(T));
					} catch {
						return default_value;
					}
				}
			}

			SetConfiguration(property, default_value);

			return default_value;
		}

		public void SetConfiguration(string property, object value) {
			SetConfiguration(property, value, true);
		}

		public void SetConfiguration(string property, object value, bool override_value) {
			SetConfiguration(property, value, override_value, null);
		}

		public void SetConfiguration(string property, object value, bool override_value, string description) {
			Initialize();
			Configuration existing_config = configurationField.FirstOrDefault(config => config.Name == property);
			
			if(existing_config == null) {
				configurationField.Add(new Configuration() {
					Name = property,
					Value = value.ToString(),
					Description = description
				});

			}else if(override_value) {
				existing_config.Value = value.ToString();
				existing_config.Description = description;
			}
		}

		public void Initialize() {
			if (initialized == false) {
				initialized = true;

				SetConfiguration("database.namespace", "", false, "Namespace for all the generated classes.");
				SetConfiguration("database.context_class", Name + "Context", false, "Name of the context class.");
				SetConfiguration("output.sql_tables", false, false, "True to output the SQL database schematic tables.");
				SetConfiguration("output.cs_classes", true, false, "True to output the C# classes.");

				
			}
		}
	}



	public partial class Association {

		[XmlIgnore]
		public Association ChildAssociation;

		[XmlIgnore]
		public Association ParentAssociation;

		[XmlIgnore]
		public Column OtherKeyColumn;

		[XmlIgnore]
		public Column ThisKeyColumn;

		[XmlIgnore]
		public Table Table;

	}

	public partial class Column {

		/// <remarks/>
		[XmlIgnore]
		public Table Table;
	}

	public partial class Index {

		/// <remarks/>
		[XmlIgnore]
		public Table Table;
	}
}

