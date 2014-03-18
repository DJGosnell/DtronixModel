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

		public static readonly string[] columns = null;

		protected DbConnection connection;


		public Model() {

		}


		public virtual void internalRead<T>(DbDataReader reader, DbConnection connection) {

			var columns = AttributeCache<T, ColumnAttribute>.getAttributes();
			var dict = new Dictionary<string, FieldInfo>();
			int length = reader.FieldCount;

			foreach (var column in columns) {
				dict.Add(column.Name, typeof(T).GetField(column.Storage, BindingFlags.NonPublic | BindingFlags.Instance));
			}
		}

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
