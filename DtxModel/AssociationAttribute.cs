using System;

namespace DtxModel {
	/// <summary>
	/// Designates a property to represent a database association, such as a foreign key relationship.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class AssociationAttribute : Attribute {

		/// <summary>
		/// When placed on a 1:1 association whose foreign key members are all non-nullable, deletes the object when the association is set to null.
		/// Setting to True deletes the object. The default value is False.
		/// </summary>
		public bool DeleteOnNull { get; set; }
		
		/// <summary>
		/// Gets or sets delete behavior for an association.
		/// A string representing the rule.
		/// </summary>
		public string DeleteRule { get; set; }
	
		/// <summary>
		///  Gets or sets the member as the foreign key in an association representing a database relationship.
		///  Default = false.
		/// </summary>
		public bool IsForeignKey { get; set; }
		
		/// <summary>
		/// Gets or sets the indication of a uniqueness constraint on the foreign key.
		/// Default = false.
		/// </summary>
		public bool IsUnique { get; set; }
		
		/// <summary>
		/// Gets or sets one or more members of the target entity class as key values on the other side of the association.
		/// Default = Id of the related class.
		/// </summary>
		public string OtherKey { get; set; }
		
		/// <summary>
		/// Gets or sets members of this entity class to represent the key values on this side of the association.
		/// Default = Id of the containing class.
		/// </summary>
		public string ThisKey { get; set; }
	}
}
