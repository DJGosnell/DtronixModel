using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace DtxModeler.Generator {
	class Options {

		private class PropertyArg {
			public OptionAttribute OptAttribute { get; set; }
			public PropertyInfo Property { get; set; }

			public PropertyArg(OptionAttribute attribute, PropertyInfo property) {
				OptAttribute = attribute;
				Property = property;
			}

		}

		public bool ParseSuccess { get; private set; }

		Dictionary<string, PropertyArg> properties = new Dictionary<string, PropertyArg>();
		List<string> values = new List<string>();

		public Options(string[] args) {
			string key = null;
			ParseSuccess = true;

			var prop_infos = this.GetType().GetProperties();

			// Get all the properties and option attributes.
			foreach (var prop_info in prop_infos) {
				var attributes = (OptionAttribute[])prop_info.GetCustomAttributes(typeof(OptionAttribute), false);
				foreach (var attribute in attributes) {
					if (attribute.LongName != null) {
						properties.Add(attribute.LongName, new PropertyArg(attribute, prop_info));
						break;
					}
				}
			}


			// Loop through all the inputs.
			foreach (var arg in args) {
				if (arg.StartsWith("--")) {
					if (values.Count > 0) {
						if (properties.ContainsKey(key)) {
							setProperty(key, arg);
						} else {
							Console.WriteLine("Argument '" + key + "' does not have a matching property.");
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
					Console.WriteLine("Value for required attribute '" + prop.OptAttribute.LongName + "' is not set.");
					ParseSuccess = false;
				}
			}

			values.Clear();
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
