using DtronixModeler.Ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DtronixModeler.Generator {
	public class TypeTransformer {

		protected TypeTransformerType[] types;

		private string[] all_db_types = null;

		public string NetToDbType(string net_type){
			var actual_net_type = NetType(net_type);

			// If this is null, then this is going to have to be an enum.
			if(actual_net_type == null) {
				actual_net_type = NetType("Int32");
			}

            return actual_net_type.DbType;
		}

		public TypeTransformerType NetType(string net_type) {
			return types.FirstOrDefault(type => type.NetType == net_type);
		}



		public string DbToNetType(string db_type) {
			return DbType(db_type).NetType;
		}

		public TypeTransformerType DbType(string db_type) {
			return types.FirstOrDefault(type => type.DbType == db_type);
		}

		public string[] DbTypes() {
			if (all_db_types == null) {
				List<string> final_types = new List<string>();
				foreach (var type in types) {
					if (final_types.Contains(type.DbType) == false) {
						final_types.Add(type.DbType);
					}
				}

				all_db_types = final_types.ToArray();
			}

			return all_db_types;
		}
	}
}
