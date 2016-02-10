using DtronixModeler.Ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DtronixModeler.Generator {
	public class TypeTransformerType {
		public readonly string db_type;
		public readonly string net_type;
		public readonly int length;
		public readonly bool is_struct;

		public TypeTransformerType(string net_type, string db_type, bool is_struct, int length = 0) {
			this.db_type = db_type;
			this.net_type = net_type;
			this.is_struct = is_struct;
			this.length = length;
		}
	}
}
