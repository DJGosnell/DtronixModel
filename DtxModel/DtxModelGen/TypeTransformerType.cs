using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DtxModelGen {
	public class TypeTransformerType {
		public readonly string db_type;
		public readonly string net_type;

		public TypeTransformerType(string net_type, string db_type) {
			this.db_type = db_type;
			this.net_type = net_type;
		}
	}
}
