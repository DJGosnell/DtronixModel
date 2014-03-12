using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtxModel
{
	public class Model {

		public static readonly string[] columns = null;

		private DbConnection connection;

		protected long _rowid;

		public long rowid {
			get { return _rowid; }
		}

		public Model() : this(null) { }


		public Model(DbConnection connection) {
			this.connection = connection;
		}

		public virtual Dictionary<string, object> getChangedValues() {
			return null;
		}

		public virtual Dictionary<string, object> getAllValues() {
			return null;
		}

		public virtual string[] getColumns() {
			return null;
		}
	}

}
