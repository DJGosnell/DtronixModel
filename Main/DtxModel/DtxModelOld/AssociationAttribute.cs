using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DtxModel {
	// Summary:
	//     Designates a property to represent a database association, such as a foreign
	//     key relationship.
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class AssociationAttribute : Attribute {

		// Summary:
		//     When placed on a 1:1 association whose foreign key members are all non-nullable,
		//     deletes the object when the association is set to null.
		//
		// Returns:
		//     Setting to True deletes the object. The default value is False.
		public bool DeleteOnNull { get; set; }
		//
		// Summary:
		//     Gets or sets delete behavior for an association.
		//
		// Returns:
		//     A string representing the rule.
		public string DeleteRule { get; set; }
		//
		// Summary:
		//     Gets or sets the member as the foreign key in an association representing
		//     a database relationship.
		//
		// Returns:
		//     Default = false.
		public bool IsForeignKey { get; set; }
		//
		// Summary:
		//     Gets or sets the indication of a uniqueness constraint on the foreign key.
		//
		// Returns:
		//     Default = false.
		public bool IsUnique { get; set; }
		//
		// Summary:
		//     Gets or sets one or more members of the target entity class as key values
		//     on the other side of the association.
		//
		// Returns:
		//     Default = Id of the related class.
		public string OtherKey { get; set; }
		//
		// Summary:
		//     Gets or sets members of this entity class to represent the key values on
		//     this side of the association.
		//
		// Returns:
		//     Default = Id of the containing class.
		public string ThisKey { get; set; }
	}
}
