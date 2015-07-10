using System;

namespace DtronixModel {
	/// <summary>
	/// Designates a class as an entity class that is associated with a database table.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class TableAttribute : Attribute {

		/// <summary>
		/// Gets or sets the name of the table or view.
		/// By default, the value is the same as the name of the class.
		/// </summary>
		public string Name { get; set; }
	}

}
