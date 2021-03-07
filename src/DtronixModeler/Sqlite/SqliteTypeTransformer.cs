using DtronixModeler.Generator;

namespace DtronixModeler.Sqlite
{
    public class SqliteTypeTransformer : TypeTransformer
    {
        public SqliteTypeTransformer()
        {
            types = new TypeTransformerType[]{
                new TypeTransformerType("Int64", "INTEGER", true, false),
                new TypeTransformerType("Int16", "INTEGER", true, false),
                new TypeTransformerType("Int32", "INTEGER", true, false),
                new TypeTransformerType("UInt16", "INTEGER", true, true),
                new TypeTransformerType("UInt32", "INTEGER", true, true),
                new TypeTransformerType("UInt64", "INTEGER", true, true),
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
