using DtxModeler.Ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DtxModeler.Generator {
	public class TypeTransformer {

		protected TypeTransformerType[] types;

		private string[] all_db_types = null;

		public string NetToDbType(NetTypes net_type){
			return NetType(net_type).db_type;
		}

		public TypeTransformerType NetType(NetTypes net_type) {
			return types.FirstOrDefault(type => type.net_type == net_type);
		}



		public NetTypes DbToNetType(string db_type) {
			return DbType(db_type).net_type;
		}

		public TypeTransformerType DbType(string db_type) {
			return types.FirstOrDefault(type => type.db_type == db_type);
		}

		public string[] DbTypes() {
			if (all_db_types == null) {
				List<string> final_types = new List<string>();
				foreach (var type in types) {
					if (final_types.Contains(type.db_type) == false) {
						final_types.Add(type.db_type);
					}
				}

				all_db_types = final_types.ToArray();
			}

			return all_db_types;
		}
	}
}
