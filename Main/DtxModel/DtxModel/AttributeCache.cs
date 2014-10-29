using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DtxModel {
	public static class AttributeCache<TObject, TAttribute> {
		private readonly static TAttribute[] attributes;

		static AttributeCache() {
			List<TAttribute> att_list = new List<TAttribute>();

			var att_objects = typeof(TObject).GetCustomAttributes(typeof(TAttribute), true);
			if (att_objects.Length == 0) {
				throw new ArgumentException("Object " + typeof(TObject).ToString() + "Does not contain attribute " + typeof(TAttribute).ToString() + ".");
			}

			attributes = att_objects as TAttribute[];
		}

		public static TAttribute GetAttribute(){
			return attributes[0];
		}

		public static TAttribute[] GetAttributes() {
			return attributes;
		}
	}
}
