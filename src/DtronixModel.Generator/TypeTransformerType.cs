namespace DtronixModel.Generator
{
    public class TypeTransformerType
    {
        public readonly string DbType;
        public readonly string NetType;
        public readonly int Length;
        public readonly bool IsStruct;
        public readonly bool? IsUnsigned;

        public TypeTransformerType(string netType, string dbType, bool isStruct, bool? isUnsigned = null, int length = 0)
        {
            DbType = dbType;
            NetType = netType;
            IsStruct = isStruct;
            IsUnsigned = isUnsigned;
            Length = length;
        }
    }
}
