using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DtxModelGen.Sqlite {
	class SqliteTypeTransformer : TypeTransformer {
		public SqliteTypeTransformer() {
			types = new TypeTransformerType[]{
				new TypeTransformerType("System.Int16", "INTEGER"),
				new TypeTransformerType("System.Int32", "INTEGER"),
				new TypeTransformerType("System.Int64", "INTEGER"),
				new TypeTransformerType("System.UInt16", "INTEGER"),
				new TypeTransformerType("System.UInt32", "INTEGER"),
				new TypeTransformerType("System.UInt64", "INTEGER"),
				new TypeTransformerType("System.Byte", "BLOB"),
				new TypeTransformerType("System.Byte[]", "BLOB"),
				new TypeTransformerType("System.DateTime", "DATETIME"),
				new TypeTransformerType("System.DateTimeOffset", "DATETIME"),
				new TypeTransformerType("System.Decimal", "REAL"),
				new TypeTransformerType("System.Float", "REAL"),
				new TypeTransformerType("System.Double", "REAL"),
				new TypeTransformerType("System.Boolean", "NUMERIC"),
				new TypeTransformerType("System.String", "TEXT"),
				new TypeTransformerType("System.Char", "TEXT")
			};
		}
	}
}
