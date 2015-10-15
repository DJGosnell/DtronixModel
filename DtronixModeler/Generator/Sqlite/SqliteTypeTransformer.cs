using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DtronixModeler.Ddl;

namespace DtronixModeler.Generator.Sqlite {
	class SqliteTypeTransformer : TypeTransformer {
		public SqliteTypeTransformer() {
			types = new TypeTransformerType[]{
				new TypeTransformerType("Int64", "INTEGER"),
				new TypeTransformerType("Int16", "SMALLINT"),
				new TypeTransformerType("Int32", "INTEGER"),
				/*new TypeTransformerType("UInt16", "SMALLINT"),
				new TypeTransformerType("UInt32", "INTEGER"),
				new TypeTransformerType("UInt64", "BIGINT"),*/
				new TypeTransformerType("ByteArray", "BLOB"),
				new TypeTransformerType("Byte", "BLOB"),
				new TypeTransformerType("Decimal", "REAL"),
				new TypeTransformerType("Decimal", "NUMERIC"),
				new TypeTransformerType("Float", "FLOAT"),
				new TypeTransformerType("Double", "DOUBLE"),
				new TypeTransformerType("Boolean", "BOOLEAN"),
				new TypeTransformerType("String", "TEXT"),
				new TypeTransformerType("DateTimeOffset", "DATETIME"),
				//new TypeTransformerType("Char", "CHAR")
			};
		}
	}
}
