using System;
using System.Collections.Generic;

namespace DtronixModel
{
    /// <summary>
    /// Class used for creation of generic SqlStatements on unspecified tables
    /// </summary>
    public class GenericTableRow : TableRow
    {

        /// <summary>
        /// Not implemented.  Throws exception.
        /// </summary>
        public override Dictionary<string, object> GetChangedValues()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not implemented.  Throws exception.
        /// </summary>
        public override bool IsChanged()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not implemented.  Throws exception.
        /// </summary>
        public override object[] GetAllValues()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not implemented.  Throws exception.
        /// </summary>
        public override string[] GetColumns()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not implemented.  Throws exception.
        /// </summary>
        public override Type[] GetColumnTypes()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not implemented.  Throws exception.
        /// </summary>
        public override string GetPKName()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not implemented.  Throws exception.
        /// </summary>
        public override object GetPKValue()
        {
            throw new NotImplementedException();
        }
    }
}
