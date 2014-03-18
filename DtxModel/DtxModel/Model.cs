using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DtxModel
{
	public class Model {

		protected DbConnection connection;
		protected List<string> changed_columns = new List<string>();

		public Model() {

		}


		protected Dictionary<string, FieldInfo> getColumnFields<T>() {
			var columns = AttributeCache<T, ColumnAttribute>.getAttributes();
			var dict = new Dictionary<string, FieldInfo>();

			foreach (var column in columns) {
				dict.Add(column.Name, typeof(T).GetField(column.Storage, BindingFlags.NonPublic | BindingFlags.Instance));
			}

			return dict;
		}

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
