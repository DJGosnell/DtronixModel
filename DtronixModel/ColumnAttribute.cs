using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DtronixModel {

	/// <summary>
	/// Attribute to descript a column in a database.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class ColumnAttribute : Attribute {

		/// <summary>
		/// Name of this column in the database.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Name of this property.
		/// </summary>
		public string Storage { get; set; }

		/// <summary>
		/// Gets or sets whether a column can contain null values.
		/// Default = true.
		/// </summary>
		public bool CanBeNull { get; set; }

		/// <summary>
		/// Gets or sets the type of the database column.
		/// </summary>
		public string DbType { get; set; }

		/// <summary>
		/// Type of the property.
		/// </summary>
		public Type Type { get; set; }

		/// <summary>
		/// Gets or sets whether a column is a computed column in a database.
		/// Default = empty.
		/// </summary>
		public string Expression { get; set; }

		/// <summary>
		/// Gets or sets whether a column contains values that the database auto-generates.
		/// Default = false.
		/// </summary>
		public bool IsDbGenerated { get; set; }

		/// <summary>
		/// Gets or sets whether a column contains a discriminator value for a LINQ to SQL inheritance hierarchy.
		/// Default = false.
		/// </summary>
		public bool IsDiscriminator { get; set; }

		/// <summary>
		/// Gets or sets whether this class member represents a column that is part or all of the primary key of the table.
		/// Default = false.
		/// </summary>
		public bool IsPrimaryKey { get; set; }
	}
}
