using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DtxModel {
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class ColumnAttribute : Attribute {

		public string Name { get; set; }

		public string Storage { get; set; }

		//
		// Summary:
		//     Gets or sets whether a column can contain null values.
		//
		// Returns:
		//     Default = true.
		public bool CanBeNull { get; set; }
		//
		// Summary:
		//     Gets or sets the type of the database column.
		//
		// Returns:
		//     See Remarks.
		public string DbType { get; set; }

		public Type Type { get; set; }


		//
		// Summary:
		//     Gets or sets whether a column is a computed column in a database.
		//
		// Returns:
		//     Default = empty.
		public string Expression { get; set; }
		//
		// Summary:
		//     Gets or sets whether a column contains values that the database auto-generates.
		//
		// Returns:
		//     Default = false.
		public bool IsDbGenerated { get; set; }
		//
		// Summary:
		//     Gets or sets whether a column contains a discriminator value for a LINQ to
		//     SQL inheritance hierarchy.
		//
		// Returns:
		//     Default = false.
		public bool IsDiscriminator { get; set; }
		//
		// Summary:
		//     Gets or sets whether this class member represents a column that is part or
		//     all of the primary key of the table.
		//
		// Returns:
		//     Default = false.
		public bool IsPrimaryKey { get; set; }
	}
}
