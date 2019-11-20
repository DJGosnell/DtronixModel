using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DtronixModeler.Generator
{
    public static class ProtobufTypeTransformer
    {
        private static List<(string, string)> ProtoNetTypes { get; } = new List<(string, string)>()
        {
            { ("double", "Decimal") },
            { ("double", "Double") },
            { ("float", "Float") },
            { ("int32", "Int16") },
            { ("int32", "Byte") },
            { ("int32", "SByte") },
            { ("int32", "Int32") },
            { ("int64", "Int64") },
            { ("uint32", "UInt16") },
            { ("uint32", "UInt32") },
            { ("uint64", "UInt64") },
            { ("bool", "Boolean") },
            { ("string", "String") },
            { ("string", "Char") },
            { ("bytes", "ByteArray") },
            { ("uint64", "DateTimeOffset") },
        };

        public static string GetProtobufType(string netType)
        {
            return ProtoNetTypes.FirstOrDefault(t => t.Item2 == netType).Item1;
        }

        public static string NetType(string netType)
        {
            return ProtoNetTypes.FirstOrDefault(t => t.Item1 == netType).Item2;
        }
    }
}
