using DtxModeler.Ddl;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtxModeler.Generator {
	public abstract class DdlGenerator {
		protected DbConnection connection = null;
		public readonly TypeTransformer TypeTransformer;
		protected Database database = new Database();

		public DdlGenerator(TypeTransformer type_transformer) {
			this.TypeTransformer = type_transformer;
		}

		public abstract Task<Database> GenerateDdl();

		protected Table GetTableByName(string name) {
			foreach (var table in database.Table) {
				if (table.Name == name) {
					return table;
				}
			}

			return null;
		}

	}
}
