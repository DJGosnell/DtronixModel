using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace DtxModel
{
	public class Model {

		public static readonly string[] columns = null;

		protected DbConnection connection;

		protected long _rowid;

		[System.Data.Linq.Mapping.Column(Name = "CustomerID", Storage = "_rowid")]
		public long rowid {
			get { return _rowid; }
		}

		public Model() { }


		public virtual void read(DbDataReader reader, DbConnection connection) { }

		public virtual Dictionary<string, object> getChangedValues() {
			return null;
		}

		public virtual object[] getAllValues() {
			return null;
		}

		public virtual string[] getColumns() {
			return null;
		}
	}

}
