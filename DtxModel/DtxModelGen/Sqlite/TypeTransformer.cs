using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DtxModelGen.Sqlite {
	class TypeTransformer {

		public class Type{
			public readonly string db_type;
			public readonly string net_type;

			public Type(string net_type, string db_type) {
				this.db_type = db_type;
				this.net_type = net_type;
			}
		}

		private Type[] types = new List<Type>();

		public TypeTransformer() {
			new Type("System.Int16", "INTEGER");
			new Type("System.Int32", "INTEGER");
			new Type("System.Int64", "INTEGER");
			new Type("System.UInt16", "INTEGER");
			new Type("System.UInt32", "INTEGER");
			new Type("System.UInt64", "INTEGER");
			new Type("System.Byte", "BLOB");
			new Type("System.Byte[]", "BLOB");
			new Type("System.DateTime", "DATETIME");
			new Type("System.DateTimeOffset", "DATETIME");
			new Type("System.Decimal", "REAL");
			new Type("System.Float", "REAL");
			new Type("System.Double", "REAL");
			new Type("System.Boolean", "NUMERIC");
			new Type("System.String", "TEXT");
			new Type("System.Char", "TEXT");
		}

		public string netToSqliteType(string net_type) {
			if (net_to_sqlite_type == null) {
				var sqlite_types = ;



				net_to_sqlite_type = sqlite_types;
			}

			if (net_to_sqlite_type.ContainsKey(net_type) == false) {
				return null;
			}

			return net_to_sqlite_type[net_type];
		}


	}
}
