using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace DtxModeler.Generator {
	public static class Utilities {

		public static void addDbParameter(DbCommand command, string name, object value) {
			var param = command.CreateParameter();
			param.ParameterName = name;
			param.Value = value;
			command.Parameters.Add(param);
		}

		public static void ForEachType<T>(this object[] objects, Action<T> callback) {
			foreach (var obj in objects) {
				if (obj is T) {
					callback((T)obj);
				}
			}
		}
	}
}
