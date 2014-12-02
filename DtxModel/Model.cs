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

		protected Dictionary<string, object> additional_values;

		public Dictionary<string, object> AdditionalValues {
			get { return additional_values; }
		}


		public Model() { }

		public virtual void Read(DbDataReader reader, Context context) {
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

		public virtual Dictionary<string, object> GetChangedValues() {
			return null;
		}

		public virtual object[] GetAllValues() {
			return null;
		}

		public virtual string[] GetColumns() {
			return null;
		}

		public virtual string GetPKName() {
			return null;
		}

		public virtual object GetPKValue() {
			return null;
		}
	}

}
