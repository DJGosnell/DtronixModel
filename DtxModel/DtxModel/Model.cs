using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DtxModel {

	[TableAttribute(Name = null)]
	public class Model {

		protected Context context;

		public Model() { }

		public virtual void read(DbDataReader reader, Context context) {
			this.context = context;
		}

		protected byte[] GetByteArray(Stream stream){

			byte[] buffer = new byte[1024];
			using (MemoryStream ms = new MemoryStream()) {
				while (true) {
					int read = stream.Read(buffer, 0, buffer.Length);
					if (read <= 0) {
						break;
					}
					ms.Write(buffer, 0, read);
				}

				stream.Close();

				return ms.ToArray();
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

		public virtual string getPKName() {
			return null;
		}

		public virtual object getPKValue() {
			return null;
		}
	}

}
