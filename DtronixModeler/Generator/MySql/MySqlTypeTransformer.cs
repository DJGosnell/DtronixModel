using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DtronixModeler.Ddl;

namespace DtronixModeler.Generator.MySql {
	class MySqlTypeTransformer : TypeTransformer {
		public MySqlTypeTransformer() {
			types = new TypeTransformerType[]{
				new TypeTransformerType("Boolean", "BOOL", 1),
				new TypeTransformerType("Boolean", "BOOLEAN", 1),

				new TypeTransformerType("Int16", "TINYINT"),
				new TypeTransformerType("Int16", "SMALLINT"),

				new TypeTransformerType("Int32", "INT"),
				new TypeTransformerType("Int32", "INTEGER"),
				new TypeTransformerType("Int32", "MEDIUMINT"),
				
				new TypeTransformerType("Int64", "BIGINT"),

				new TypeTransformerType("Float", "FLOAT"),

				new TypeTransformerType("Double", "DOUBLE"),
				new TypeTransformerType("Double", "PRECISION"),

				new TypeTransformerType("Decimal", "REAL"),
				new TypeTransformerType("Decimal", "DECIMAL"),
				new TypeTransformerType("Decimal", "NUMERIC"),

				new TypeTransformerType("Byte", "BYTE", 1),

				new TypeTransformerType("ByteArray", "BINARY"),
				new TypeTransformerType("ByteArray", "BLOB"),
				new TypeTransformerType("ByteArray", "MEDIMUMBLOB"),
				new TypeTransformerType("ByteArray", "LONGBLOB"),

				new TypeTransformerType("DateTimeOffset", "DATE"),
				new TypeTransformerType("DateTimeOffset", "DATETIME"),
				new TypeTransformerType("DateTimeOffset", "TIME"),
				
				new TypeTransformerType("String", "CHAR", 32),
				new TypeTransformerType("String", "VARCHAR", 32),

				new TypeTransformerType("String", "TINYTEXT"),
				new TypeTransformerType("String", "TEXT"),
				new TypeTransformerType("String", "MEDIUMTEXT"),
				new TypeTransformerType("String", "LONGTEXT"),

				new TypeTransformerType("Char", "CHAR", 1)
				
			};
		}
	}
}
