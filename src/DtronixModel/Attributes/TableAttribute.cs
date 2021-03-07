using System;

namespace DtronixModel.Attributes
{
    /// <summary>
    /// Designates a class as an entity class that is associated with a database table.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class TableAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name of the table or view.
        /// By default, the value is the same as the name of the class.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// All the column names of the table except the primary key.
        /// </summary>
        public string[] ColumnNames { get; set; }

        /// <summary>
        /// All of the column types of teh table except for the primary key.
        /// </summary>
        public Type[] ColumnTypes { get; set; }

        /// <summary>
        /// Primary key name
        /// </summary>
        public string PrimaryKey { get; set; }

    }
}