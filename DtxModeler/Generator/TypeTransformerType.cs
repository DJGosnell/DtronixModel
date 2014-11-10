using DtxModeler.Ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DtxModeler.Generator {
	public class TypeTransformerType {
		public readonly string db_type;
		public readonly NetTypes net_type;
		public readonly int length;

		public TypeTransformerType(NetTypes net_type, string db_type, int length = 0) {
			this.db_type = db_type;
			this.net_type = net_type;
			this.length = length;
		}
	}
}
