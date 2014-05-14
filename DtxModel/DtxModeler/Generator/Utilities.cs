using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace DtxModeler.Generator {
	class Utilities {

		public static void addDbParameter(DbCommand command, string name, object value) {
			var param = command.CreateParameter();
			param.ParameterName = name;
			param.Value = value;
			command.Parameters.Add(param);
		}
	}
}
