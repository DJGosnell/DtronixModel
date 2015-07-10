using DtronixModeler.Ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtronixModeler.Generator.Output {
	public partial class CSharpCodeGenerator {

		private class AssociationCodeGenerator {
			public Table ThisTable { get; set; }
			public Column ThisColumn { get; set; }
			public string ThisAssociationName { get; set; }
			public Cardinality ThisCardinality { get; set; }

			public Table OtherTable { get; set; }
			public Column OtherColumn { get; set; }
			public string OtherAssociationName { get; set; }
			public Cardinality OtherCardinality { get; set; }
		}

		Database database;

		public CSharpCodeGenerator(Database database) {
			this.database = database;
		}


		private string ColumnNetType(Column column) {
			if (column.NetType == NetTypes.ByteArray) {
				return "byte[]";
			}

			string type = Enum.GetName(typeof(NetTypes), column.NetType);

			if (column.NetType == NetTypes.Float) {
				type = "float";
			}

			if (column.Nullable) {
				switch (column.NetType) {
					case NetTypes.Int64:
					case NetTypes.Int16:
					case NetTypes.Int32:
					case NetTypes.Byte:
					case NetTypes.Decimal:
					case NetTypes.Float:
					case NetTypes.Double:
					case NetTypes.Boolean:
					case NetTypes.Char:
						type += "?";
						break;
				}
			}

			return type;
		}
	}
}
