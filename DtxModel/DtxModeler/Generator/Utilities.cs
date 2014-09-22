using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DtxModeler.Generator {
	public static class Utilities {

		public static void addDbParameter(DbCommand command, string name, object value) {
			var param = command.CreateParameter();
			param.ParameterName = name;
			param.Value = value;
			command.Parameters.Add(param);
		}


		public static string XmlSerializeObject(object element) {
			XmlSerializer serializer = new XmlSerializer(element.GetType());
			using (StringWriter writer = new StringWriter()) {
				serializer.Serialize(writer, element);
				return writer.ToString();
			}
		}

		public static T XmlDeserializeString<T>(string data) {
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			using (StringReader reader = new StringReader(data)) {
				try {
					return (T)serializer.Deserialize(reader);
				} catch (Exception) {
					return default(T);
				}
				
			}
		}
	}
}
