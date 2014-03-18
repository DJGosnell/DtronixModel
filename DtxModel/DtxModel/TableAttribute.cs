using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DtxModel {
	// Summary:
	//     Designates a class as an entity class that is associated with a database
	//     table.
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class TableAttribute : Attribute {

		// Summary:
		//     Gets or sets the name of the table or view.
		//
		// Returns:
		//     By default, the value is the same as the name of the class.
		public string Name { get; set; }
	}

}
