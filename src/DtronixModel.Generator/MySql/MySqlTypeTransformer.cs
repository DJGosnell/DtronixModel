namespace DtronixModel.Generator.MySql
{
    public class MySqlTypeTransformer : TypeTransformer
    {
        public MySqlTypeTransformer()
        {
            types = new[]
            {
                new TypeTransformerType("Boolean", "BOOL", true, null, 1),
                new TypeTransformerType("Boolean", "BOOLEAN", true, null, 1),

                new TypeTransformerType("SByte", "TINYINT", true, false),
                new TypeTransformerType("Int16", "SMALLINT", true, false),

                new TypeTransformerType("Byte", "TINYINT", true, true),
                new TypeTransformerType("UInt16", "SMALLINT", true, true),

                new TypeTransformerType("Int32", "INT", true, false),
                new TypeTransformerType("Int32", "INTEGER", true, false),
                new TypeTransformerType("Int32", "MEDIUMINT", true, false),

                new TypeTransformerType("UInt32", "INT", true, true),
                new TypeTransformerType("UInt32", "INTEGER", true, true),
                new TypeTransformerType("UInt32", "MEDIUMINT", true, true),

                new TypeTransformerType("Int64", "BIGINT", true, false),

                new TypeTransformerType("UInt64", "BIGINT", true, true),

                new TypeTransformerType("Float", "FLOAT", true),

                new TypeTransformerType("Double", "DOUBLE", true),
                new TypeTransformerType("Double", "PRECISION", true),

                new TypeTransformerType("Decimal", "REAL", true),
                new TypeTransformerType("Decimal", "DECIMAL", true),
                new TypeTransformerType("Decimal", "NUMERIC", true),

                new TypeTransformerType("Byte", "BYTE", true, null, 1),

                new TypeTransformerType("ByteArray", "BINARY", false),
                new TypeTransformerType("ByteArray", "BLOB", false),
                new TypeTransformerType("ByteArray", "MEDIMUMBLOB", false),
                new TypeTransformerType("ByteArray", "LONGBLOB", false),
                new TypeTransformerType("ByteArray", "TINYBLOB", false),

                new TypeTransformerType("DateTimeOffset", "DATE", true),
                new TypeTransformerType("DateTimeOffset", "DATE_F", true),
                new TypeTransformerType("DateTimeOffset", "DATETIME", true),
                new TypeTransformerType("DateTimeOffset", "DATETIME_F", true),
                new TypeTransformerType("DateTimeOffset", "TIME", true),
                new TypeTransformerType("DateTimeOffset", "TIME_F", true),

                new TypeTransformerType("DateTimeOffset", "TIMESTAMP", true),
                new TypeTransformerType("DateTimeOffset", "TIMESTAMP_F", true),

                new TypeTransformerType("String", "CHAR", false, null, 32),
                new TypeTransformerType("String", "VARCHAR", false, null, 32),

                new TypeTransformerType("String", "TINYTEXT", false),
                new TypeTransformerType("String", "TEXT", false),
                new TypeTransformerType("String", "MEDIUMTEXT", false),
                new TypeTransformerType("String", "LONGTEXT", false),

                new TypeTransformerType("Char", "CHAR", true, null, 1)

            };
        }
    }
}
