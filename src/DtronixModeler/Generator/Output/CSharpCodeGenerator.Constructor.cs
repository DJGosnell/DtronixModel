using DtronixModeler.Ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtronixModeler.Generator.Output
{
    public partial class CSharpCodeGenerator
    {

        private class AssociationCodeGenerator
        {
            public Table ThisTable { get; set; }
            public Column ThisColumn { get; set; }
            public string ThisAssociationName { get; set; }
            public Cardinality ThisCardinality { get; set; }

            public Table OtherTable { get; set; }
            public Column OtherColumn { get; set; }
            public string OtherAssociationName { get; set; }
            public Cardinality OtherCardinality { get; set; }
        }

        Database database;

        public CSharpCodeGenerator(Database database)
        {
            this.database = database;
        }

        private bool ColumnIsTypeStruct(Column column)
        {
            switch (column.NetType)
            {
                case "Int64":
                case "Int16":
                case "Int32":
                case "UInt64":
                case "UInt16":
                case "UInt32":
                case "Byte":
                case "Decimal":
                case "Float":
                case "Double":
                case "Boolean":
                case "Char":
                case "DateTimeOffset":
                    return true;
            }

            // See if the type is an enum.
            if (Type.GetType("System." + column.NetType, false) == null)
            {
                return true;
            }

            return true;
        }


        private string ColumnNetType(Column column)
        {
            if (column.NetType == "ByteArray")
            {
                return "byte[]";
            }

            string type = column.NetType;

            if (column.NetType == "Float")
            {
                type = "float";
            }

            if (column.Nullable)
            {
                switch (column.NetType)
                {
                    case "Int64":
                    case "Int16":
                    case "Int32":
                    case "UInt64":
                    case "UInt16":
                    case "UInt32":
                    case "Byte":
                    case "Decimal":
                    case "Float":
                    case "Double":
                    case "Boolean":
                    case "Char":
                    case "DateTimeOffset":
                        type += "?";
                        break;
                }
            }

            return type;
        }
    }
}
