using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DtxModelGen {
	class Utilities {
		public static void each<T>(object[] values, Func<T, bool> callback) {
			foreach (var value in values){
				if(value is T){
					if (callback((T)value) == false) {
						return;
					} 
				}
			}
		}
	}
}
