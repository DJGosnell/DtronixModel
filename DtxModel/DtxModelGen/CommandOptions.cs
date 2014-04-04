using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace DtxModelGen {
	class CommandOptions {

		private class PropertyArg {
			public OptionAttribute OptAttribute { get; set; }
			public PropertyInfo Property {get; set;}

			public PropertyArg(OptionAttribute attribute, PropertyInfo property) {
				OptAttribute = attribute;
				Property = property;
			}

		}


		public bool ParseSuccess { get; private set; }

		Dictionary<string, PropertyArg> properties = new Dictionary<string, PropertyArg>();
		List<string> values = new List<string>();

		[OptionAttribute("code-outupt", Required = false, HelpText = "The C# file to output the generated code to.")]
		public string CodeOutput { get; set; }

		[OptionAttribute("code-type", Required = false, HelpText = "The type of C# code the generate.  Allowed values: DtxModel.")]
		public string CodeType {
			get {
				return "DtxModel";
			}
		}

		[OptionAttribute("db-outupt", Required = false, HelpText = "The connection string for the database.")]
		public string DbOutput { get; set; }

		[OptionAttribute("sql-outupt", Required = false)]
		public string SqlOutput { get; set; }

		[OptionAttribute("dbml-outupt", Required = false)]
		public string DbmlOutput { get; set; }

		[OptionAttribute("input", Required = true, HelpText = "The input that the generator will be working off of.")]
		public string Input { get; set; }

		[OptionAttribute("input-type", Required = true, HelpText = "Allowed Values: dbml|sql(TBD)|database(SQLiteConnection)")]
		public string InputType { get; set; }

		[OptionAttribute("db-class", Required = false, HelpText = "Allowed Values: SQLiteConnection")]
		public string DbClass { get; set; }


		public CommandOptions(string[] args) {
			string key = null;
			ParseSuccess = true;

			var prop_infos = this.GetType().GetProperties();

			foreach (var prop_info in prop_infos) {
				var attributes = (OptionAttribute[])prop_info.GetCustomAttributes(typeof(OptionAttribute), false);
				foreach (var attribute in attributes) {
					if (attribute.LongName != null) {
						properties.Add(attribute.LongName, new PropertyArg(attribute, prop_info));
						break;
					}
				}
			}


			foreach (var arg in args) {
				if (arg.StartsWith("--")) {
					if (values.Count > 0) {
						if (properties.ContainsKey(key)) {
							setProperty(key, arg);
						} else {
							Console.WriteLine("Argument '" + arg + "' does not have a matching property.");
							ParseSuccess = false;
						}
					}

					// Strip the dashes off the argument.
					key = arg.Substring(2).ToLower();

				} else {
					values.Add(arg);
				}
			}

			// Register the last argument if any.
			if (values.Count > 0) {
				setProperty(key, key);
			}

			foreach (var prop in properties.Values) {
				if (prop.OptAttribute.Required && prop.Property.GetValue(this, null) == null) {
					Console.WriteLine("Value for required attribute'" + prop.OptAttribute.LongName + "' is not set.");
					ParseSuccess = false;
				}
			}
		}

		private void setProperty(string key, string arg) {
			if (values.Count == 1) {
				properties[key].Property.SetValue(this, values[0], null);
			} else {
				if (properties[key].Property.PropertyType.IsArray == false) {
					Console.WriteLine("Array provided for argument '" + arg + "' but the property is singular.");
					ParseSuccess = false;
					return;
				}
				properties[key].Property.SetValue(this, values.ToArray(), null);
			}

			values.Clear();
		}

	}
}
