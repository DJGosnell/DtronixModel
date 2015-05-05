using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DtxModel {
	/// <summary>
	/// Class to handle attribute retrieval and caching of results.
	/// </summary>
	/// <typeparam name="TObject">Main class that the attribute is attached to.</typeparam>
	/// <typeparam name="TAttribute">Attribute type to retrieve.</typeparam>
	static class AttributeCache<TObject, TAttribute> {
		private readonly static TAttribute[] attributes;

		static AttributeCache() {
			List<TAttribute> att_list = new List<TAttribute>();

			var att_objects = typeof(TObject).GetCustomAttributes(typeof(TAttribute), true);
			if (att_objects.Length == 0) {
				throw new ArgumentException("Object " + typeof(TObject).ToString() + "Does not contain attribute " + typeof(TAttribute).ToString() + ".");
			}

			attributes = att_objects as TAttribute[];
		}

		/// <summary>
		/// Gets the first attribute.
		/// </summary>
		/// <returns>TAttribute</returns>
		public static TAttribute GetAttribute() {
			return attributes[0];
		}

		/// <summary>
		/// Retrieves all the attributes.
		/// </summary>
		/// <returns>Array of TAttributes</returns>
		public static TAttribute[] GetAttributes() {
			return attributes;
		}
	}
}
