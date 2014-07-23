using DtxModeler.Ddl;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace DtxModeler.Generator {
	public abstract class DdlGenerator {
		protected DbConnection connection = null;
		public readonly TypeTransformer TypeTransformer;
		protected Database database = new Database();

		public DdlGenerator(string connection_string, TypeTransformer type_transformer) {
			this.TypeTransformer = type_transformer;
		}

		public abstract Database generateDdl();
		public abstract Column[] getTableColumns(string table);
		public abstract Index[] getIndexes(string table_name);
		public abstract Table[] getTables();

		protected Table getTableByName(string name) {
			foreach (var table in database.Table) {
				if (table.Name == name) {
					return table;
				}
			}

			return null;
		}

	}
}
