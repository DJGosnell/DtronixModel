using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DtxModeler.Generator.Sqlite {
	class SqliteTypeTransformer : TypeTransformer {
		public SqliteTypeTransformer() {
			types = new TypeTransformerType[]{
				new TypeTransformerType("System.Int64", "INTEGER"),
				new TypeTransformerType("System.Int16", "SMALLINT"),
				new TypeTransformerType("System.Int32", "INTEGER"),
				/*new TypeTransformerType("System.UInt16", "SMALLINT"),
				new TypeTransformerType("System.UInt32", "INTEGER"),
				new TypeTransformerType("System.UInt64", "BIGINT"),*/
				new TypeTransformerType("System.Byte[]", "BLOB"),
				new TypeTransformerType("System.Byte", "BLOB"),
				new TypeTransformerType("System.DateTime", "DATETIME"),
				new TypeTransformerType("System.DateTimeOffset", "DATETIME"),
				new TypeTransformerType("System.Decimal", "REAL"),
				new TypeTransformerType("System.Decimal", "NUMERIC"),
				new TypeTransformerType("System.Float", "FLOAT"),
				new TypeTransformerType("System.Double", "DOUBLE"),
				new TypeTransformerType("System.Boolean", "BOOLEAN"),
				new TypeTransformerType("System.String", "TEXT"),
				new TypeTransformerType("System.Char", "CHAR")
			};
		}
	}
}
