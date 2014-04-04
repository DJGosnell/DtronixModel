using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DtxModelGen {
	public class TypeTransformer {

		protected TypeTransformerType[] types;

		public string netToDbType(string net_type){
			return types.FirstOrDefault(type => type.net_type == net_type).db_type;
		}
		public string dbToNetType(string db_type) {
			return types.FirstOrDefault(type => type.db_type == db_type).net_type;
		}
	}
}
