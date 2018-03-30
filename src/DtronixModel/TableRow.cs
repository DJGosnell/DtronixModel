using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlTypes;

namespace DtronixModel
{
    /// <summary>
    /// Base model which represents a database's table's row.
    /// </summary>
    [Table(Name = null)]
    public abstract class TableRow
    {
        /// <summary>
        /// Bit array which contains the flags for each table column
        /// </summary>
        protected BitArray ChangedFlags;

        /// <summary>
        /// Database Context for this class
        /// </summary>
        protected Context Context;

        /// <summary>
        /// Values which are returned but not part of this table.
        /// </summary>
        public Dictionary<string, object> AdditionalValues { get; protected set; }

        /// <summary>
        /// Reads from the DbReader the table row information.
        /// </summary>
        /// <param name="reader">Reader to read the data from.</param>
        /// <param name="context">Context which this reader is associated with.</param>
        public virtual void Read(DbDataReader reader, Context context)
        {
            Context = context;
        }

        /// <summary>
        /// Gets all the instance values in the model which have been changed.
        /// </summary>
        /// <returns>Dictionary with the keys of the column names and values of the properties.</returns>
        public abstract Dictionary<string, object> GetChangedValues();

        /// <summary>
        /// Returns true if any of the values have been modified from the properties.
        /// </summary>
        /// <returns>True if the row has any modified values.</returns>
        public abstract bool IsChanged();

        /// <summary>
        /// Return all the instance values for the entire model.
        /// </summary>
        /// <returns>An object array with all the values of this model.</returns>
        public abstract object[] GetAllValues();

        /// <summary>
        /// Returns all the columns in this model.
        /// </summary>
        /// <returns>A string array with all the columns in this model.</returns>
        public abstract string[] GetColumns();

        /// <summary>
        /// Returns all the columns in this model.
        /// </summary>
        /// <returns>A string array with all the columns in this model.</returns>
        public abstract Type[] GetColumnTypes();

        /// <summary>
        /// Gets the name of the model primary key.
        /// </summary>
        /// <returns>The name of the primary key</returns>
        public abstract string GetPKName();

        /// <summary>
        /// Gets the value of the primary key.
        /// </summary>
        /// <returns>The value of the primary key.</returns>
        public abstract object GetPKValue();

        /// <summary>
        /// Resets the flags on the changed values to state that the current value has not been changed.
        /// </summary>
        public void ResetChangedFlags()
        {
            ChangedFlags.SetAll(false);
        }
    }
}