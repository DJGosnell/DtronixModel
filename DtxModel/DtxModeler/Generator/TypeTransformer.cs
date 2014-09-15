using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DtxModeler.Generator {
	public class TypeTransformer {

		protected TypeTransformerType[] types;

		private string[] all_net_types = null;
		private string[] all_db_types = null;

		public string NetToDbType(string net_type){
			return types.FirstOrDefault(type => type.net_type == net_type).db_type;
		}
		public string DbToNetType(string db_type) {
			return types.FirstOrDefault(type => type.db_type == db_type).net_type;
		}

		public string[] NetTypes() {
			if (all_net_types == null) {

				List<string> final_types = new List<string>();
				foreach (var type in types) {
					if (final_types.Contains(type.net_type) == false) {
						final_types.Add(type.net_type);
					}
				}
				all_net_types = final_types.ToArray();
			}

			return all_net_types;
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
