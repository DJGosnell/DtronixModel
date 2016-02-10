using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DtronixModeler.Ddl;

namespace DtronixModeler.Generator.MySql {
	class MySqlTypeTransformer : TypeTransformer {
		public MySqlTypeTransformer() {
			types = new TypeTransformerType[]{
				new TypeTransformerType("Boolean", "BOOL", true, 1),
				new TypeTransformerType("Boolean", "BOOLEAN", true, 1),

				new TypeTransformerType("Int16", "TINYINT", true),
				new TypeTransformerType("Int16", "SMALLINT", true),

				new TypeTransformerType("Int32", "INT", true),
				new TypeTransformerType("Int32", "INTEGER", true),
				new TypeTransformerType("Int32", "MEDIUMINT", true),

				new TypeTransformerType("Int64", "BIGINT", true),

				new TypeTransformerType("Float", "FLOAT", true),

				new TypeTransformerType("Double", "DOUBLE", true),
				new TypeTransformerType("Double", "PRECISION", true),

				new TypeTransformerType("Decimal", "REAL", true),
				new TypeTransformerType("Decimal", "DECIMAL", true),
				new TypeTransformerType("Decimal", "NUMERIC", true),

				new TypeTransformerType("Byte", "BYTE", true, 1),

				new TypeTransformerType("ByteArray", "BINARY", false),
				new TypeTransformerType("ByteArray", "BLOB", false),
				new TypeTransformerType("ByteArray", "MEDIMUMBLOB", false),
				new TypeTransformerType("ByteArray", "LONGBLOB", false),

				new TypeTransformerType("DateTimeOffset", "DATE", true),
				new TypeTransformerType("DateTimeOffset", "DATETIME", true),
				new TypeTransformerType("DateTimeOffset", "TIME", true),

				new TypeTransformerType("String", "CHAR", false, 32),
				new TypeTransformerType("String", "VARCHAR", false, 32),

				new TypeTransformerType("String", "TINYTEXT", false),
				new TypeTransformerType("String", "TEXT", false),
				new TypeTransformerType("String", "MEDIUMTEXT", false),
				new TypeTransformerType("String", "LONGTEXT", false),

				new TypeTransformerType("Char", "CHAR", true, 1)

			};
		}
	}
}
