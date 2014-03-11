using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtxModel
{
	public interface IModel {
		protected IModel readOne(SqlDataReader reader);
		protected IModel[] readAll(SqlDataReader reader);
	}
}
