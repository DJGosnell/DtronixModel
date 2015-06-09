using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;

namespace DtxModel {

	/// <summary>
	/// Base model which represents a database's table.
	/// </summary>
	[TableAttribute(Name = null)]
	public class Model {

		/// <summary>
		/// Database Context for this class
		/// </summary>
		protected Context context;

		/// <summary>
		/// Bit array which contains the flags for each table column
		/// </summary>
		protected BitArray changed_flags;

		/// <summary>
		/// Values which are returned but not part of this table.
		/// </summary>
		protected Dictionary<string, object> additional_values;

		/// <summary>
		/// Values which are returned but not part of this table.
		/// </summary>
		public Dictionary<string, object> AdditionalValues {
			get { return additional_values; }
		}


		/// <summary>
		/// Creates an instance of a basic model.
		/// </summary>
		public Model() { }

		/// <summary>
		/// Reads from the DbReader the table row information.
		/// </summary>
		/// <param name="reader">Reader to read the data from.</param>
		/// <param name="context">Context which this reader is associated with.</param>
		public virtual void Read(DbDataReader reader, Context context) {
			this.context = context;
		}

		/// <summary>
		/// Get all the bytes in a stream.
		/// </summary>
		/// <param name="stream">Stream to read from.</param>
		/// <returns>Filled byte array with the stream data.</returns>
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

		/// <summary>
		/// Gets all the instance values in the model which have been changed.
		/// </summary>
		/// <returns>Dictionary with the keys of the column names and values of the properties.</returns>
		public virtual Dictionary<string, object> GetChangedValues() {
			return null;
		}

		/// <summary>
		/// Return all the instance values for the entire model.
		/// </summary>
		/// <returns>An object array with all the values of this model.</returns>
		public virtual object[] GetAllValues() {
			return null;
		}

		/// <summary>
		/// Returns all the columns in this model.
		/// </summary>
		/// <returns>A string array with all the columns in this model.</returns>
		public virtual string[] GetColumns() {
			return null;
		}

		/// <summary>
		/// Gets the name of the model primary key.
		/// </summary>
		/// <returns>The name of the primary key</returns>
		public virtual string GetPKName() {
			return null;
		}

		/// <summary>
		/// Gets the value of the primary key.
		/// </summary>
		/// <returns>The value of the primary key.</returns>
		public virtual object GetPKValue() {
			return null;
		}

		/// <summary>
		/// Resets the flags on the changed values to state that the current value has not been changed.
		/// </summary>
		public void ResetChangedFlags() {
			changed_flags.SetAll(false);
		}
	}

}
