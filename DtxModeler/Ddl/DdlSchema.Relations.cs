using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace DtxModeler.Ddl {
	public partial class Table {

		public void Rename(Database database, string new_name) {
			string old_name = this.nameField;
			// Update associations
			foreach (var association in database.Association) {
				var assoc_ref = association.ReferencesTable(this);
				if (assoc_ref != Association.Reference.None) {
					if (assoc_ref == Association.Reference.R1) {
						association.Table1 = new_name;
						if (association.Table1Name == old_name) {
							association.Table1Name = new_name;
						}
					} else {
						association.Table2 = new_name;
						if (association.Table2Name == old_name) {
							association.Table2Name = new_name;
						}
					}
				}
			}

			this.Name = new_name;

		}

	}

	public partial class Database {
		[XmlIgnore]
		public bool _Modified;

		[XmlIgnore]
		public string _FileLocation;

		[XmlIgnore]
		public TreeViewItem _TreeRoot;

		private bool initialized = false;

		public T GetConfiguration<T>(string property) {
			Initialize();

			property = property.ToLower();
			foreach (var config in this.configurationField) {
				if (config.Name == property) {
					try {
						return (T)Convert.ChangeType(config.Value, typeof(T));
					} catch {
						return default(T);
					}
				}
			}

			throw new Exception("Configuration not set for the property " + property);
		}

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

			if (existing_config == null) {
				configurationField.Add(new Configuration() {
					Name = property,
					Value = value.ToString(),
					Description = description
				});

			} else if (override_value) {
				existing_config.Value = value.ToString();
				existing_config.Description = description;
			}
		}

		public void Initialize() {
			if (initialized == false) {
				initialized = true;

				if (string.IsNullOrWhiteSpace(this.ContextClass)) {
					if (string.IsNullOrWhiteSpace(Name)) {
						this.ContextClass = "DatabaseContext";
					} else {
						this.ContextClass = Name + "Context";
					}
				}
			}
		}

		public Association[] GetAssociations(string table) {
			return GetAssociations(tableField.FirstOrDefault(t => t.Name == table));
		}

		public Association[] GetAssociations(Table table) {
			if (table == null) {
				return null;
			}
			var table_associations = associationField.Where(association => association.Table1 == table.Name || association.Table2 == table.Name);

			if (table_associations == null || table_associations.Count() == 0) {
				return null;
			}

			return table_associations.ToArray();
		}
	}



	public partial class Association {

		public enum Reference {
			R1,
			R2,
			None
		}

		[XmlIgnore]
		public string DisplayName {
			get {
				string card1 = (Table1Cardinality == Cardinality.Many) ? "[*]" : "[1]";
				string card2 = (Table2Cardinality == Cardinality.Many) ? "[*]" : "[1]";
				return Table1Name + " (" + Table1 + "." + Table1Column + ") " + card1 + " <-> " + card2 + " " + Table2Name + " (" + Table2 + "." + Table2Column + ")";
			}
		}

		public Column GetReferenceColumn(Database database, Reference reference) {
			string table_name = (reference == Reference.R1) ? table1Field : table2Field;
			string column_name = (reference == Reference.R1) ? table1ColumnField : table2ColumnField;

			var table = database.Table.FirstOrDefault(t => t.Name == table_name);
			if (table == null) {
				return null;
			}

			return table.Column.FirstOrDefault(c => c.Name == column_name);
		}

		public Reference ReferencesTable(Table table) {
			if (this.table1Field == table.Name){
				return Reference.R1;

			} else if (this.table2Field == table.Name) {
				return Reference.R2;

			} else {
				return Reference.None;
			}
		}

		public Reference ReferencesTableColumn(Table table, string column_name) {
			var ref_table = ReferencesTable(table);
			if (ref_table == Reference.None) {
				return Reference.None;

			} else if (ref_table == Reference.R1 && this.Table1Column == column_name) {
				return Reference.R1;

			} else if (ref_table == Reference.R2 && this.Table2Column == column_name) {
				return Reference.R2;

			} else {
				return Reference.None;
			}
		}


	}

	public partial class Column {

		public void Rename(Database database, string new_name) {

			Table table = database.Table.FirstOrDefault(t => t.Column.Contains(this));
			if (table == null) {
				return;
			}

			// Rename associations.
			foreach (var association in database.Association) {
				var assoc_ref = association.ReferencesTableColumn(table, this.nameField);
				if (assoc_ref != Association.Reference.None) {
					if (assoc_ref == Association.Reference.R1) {
						association.Table1Column = new_name;
					} else {
						association.Table2Column = new_name;
					}
				}
			}


			this.Name = new_name;
		}

		public void PostRename(Database database, string old_name) {
			// Undo the change and don't notify the property listeners.
			var new_name = this.nameField;
			this.nameField = old_name;
			Rename(database, new_name);
		}
	}

	public partial class Index {

		/// <remarks/>
		[XmlIgnore]
		public Table Table;
	}

	public partial class Configuration {
		[XmlIgnore]
		private Visibility visibilityField;

		[XmlIgnore]
		public Visibility Visibility {
			get {
				return this.visibilityField;
			}
			set {
				if ((visibilityField.Equals(value) != true)) {
					this.visibilityField = value;
					this.OnPropertyChanged("Visibility");
				}
			}
		}
	}
}

