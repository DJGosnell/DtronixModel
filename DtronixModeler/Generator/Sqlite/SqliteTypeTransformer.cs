using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DtronixModeler.Ddl;

namespace DtronixModeler.Generator.Sqlite {
	class SqliteTypeTransformer : TypeTransformer {
		public SqliteTypeTransformer() {
			types = new TypeTransformerType[]{
				new TypeTransformerType("Int64", "INTEGER", true),
				new TypeTransformerType("Int16", "SMALLINT", true),
				new TypeTransformerType("Int32", "INTEGER", true),
				/*new TypeTransformerType("UInt16", "SMALLINT"),
				new TypeTransformerType("UInt32", "INTEGER"),
				new TypeTransformerType("UInt64", "BIGINT"),*/
				new TypeTransformerType("ByteArray", "BLOB", false),
				new TypeTransformerType("Byte", "BLOB", true),
				new TypeTransformerType("Decimal", "REAL", true),
				new TypeTransformerType("Decimal", "NUMERIC", true),
				new TypeTransformerType("Float", "FLOAT", true),
				new TypeTransformerType("Double", "DOUBLE", true),
				new TypeTransformerType("Boolean", "BOOLEAN", true),
				new TypeTransformerType("String", "TEXT", false),
				new TypeTransformerType("DateTimeOffset", "DATETIME", true),
				//new TypeTransformerType("Char", "CHAR")
			};
		}
	}
}
