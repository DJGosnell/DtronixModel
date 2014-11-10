using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DtxModeler.Ddl;

namespace DtxModeler.Generator.MySql {
	class MySqlTypeTransformer : TypeTransformer {
		public MySqlTypeTransformer() {
			types = new TypeTransformerType[]{
				new TypeTransformerType(NetTypes.Boolean, "BOOL", 1),
				new TypeTransformerType(NetTypes.Boolean, "BOOLEAN", 1),

				new TypeTransformerType(NetTypes.Int16, "TINYINT"),
				new TypeTransformerType(NetTypes.Int16, "SMALLINT"),

				new TypeTransformerType(NetTypes.Int32, "INT"),
				new TypeTransformerType(NetTypes.Int32, "INTEGER"),
				new TypeTransformerType(NetTypes.Int32, "MEDIUMINT"),
				
				new TypeTransformerType(NetTypes.Int64, "BIGINT"),

				new TypeTransformerType(NetTypes.Float, "FLOAT"),

				new TypeTransformerType(NetTypes.Double, "DOUBLE"),
				new TypeTransformerType(NetTypes.Double, "PRECISION"),

				new TypeTransformerType(NetTypes.Decimal, "REAL"),
				new TypeTransformerType(NetTypes.Decimal, "DECIMAL"),
				new TypeTransformerType(NetTypes.Decimal, "NUMERIC"),

				new TypeTransformerType(NetTypes.Byte, "BYTE", 1),

				new TypeTransformerType(NetTypes.ByteArray, "BLOB"),
				new TypeTransformerType(NetTypes.ByteArray, "MEDIMUMBLOB"),
				new TypeTransformerType(NetTypes.ByteArray, "LONGBLOB"),

				new TypeTransformerType(NetTypes.DateTime, "DATE"),
				new TypeTransformerType(NetTypes.DateTime, "DATETIME"),
				new TypeTransformerType(NetTypes.DateTime, "TIME"),
				new TypeTransformerType(NetTypes.DateTimeOffset, "DATETIME"),
				
				new TypeTransformerType(NetTypes.String, "CHAR", 32),
				new TypeTransformerType(NetTypes.String, "VARCHAR", 32),

				new TypeTransformerType(NetTypes.String, "TINYTEXT"),
				new TypeTransformerType(NetTypes.String, "TEXT"),
				new TypeTransformerType(NetTypes.String, "MEDIUMTEXT"),
				new TypeTransformerType(NetTypes.String, "LONGTEXT"),

				new TypeTransformerType(NetTypes.Char, "CHAR", 1)
				
			};
		}
	}
}
