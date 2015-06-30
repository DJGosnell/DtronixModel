using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DtxModeler.Ddl;

namespace DtxModeler.Generator.Sqlite {
	class SqliteTypeTransformer : TypeTransformer {
		public SqliteTypeTransformer() {
			types = new TypeTransformerType[]{
				new TypeTransformerType(NetTypes.Int64, "INTEGER"),
				new TypeTransformerType(NetTypes.Int16, "SMALLINT"),
				new TypeTransformerType(NetTypes.Int32, "INTEGER"),
				/*new TypeTransformerType(NetTypes.UInt16, "SMALLINT"),
				new TypeTransformerType(NetTypes.UInt32, "INTEGER"),
				new TypeTransformerType(NetTypes.UInt64, "BIGINT"),*/
				new TypeTransformerType(NetTypes.ByteArray, "BLOB"),
				new TypeTransformerType(NetTypes.Byte, "BLOB"),
				new TypeTransformerType(NetTypes.Decimal, "REAL"),
				new TypeTransformerType(NetTypes.Decimal, "NUMERIC"),
				new TypeTransformerType(NetTypes.Float, "FLOAT"),
				new TypeTransformerType(NetTypes.Double, "DOUBLE"),
				new TypeTransformerType(NetTypes.Boolean, "BOOLEAN"),
				new TypeTransformerType(NetTypes.String, "TEXT"),
				new TypeTransformerType(NetTypes.DateTimeOffset, "DATETIME"),
				//new TypeTransformerType(NetTypes.Char, "CHAR")
			};
		}
	}
}
