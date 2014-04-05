using DtxModel.Ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DtxModelGen.CodeGen {
	class CodeGenerator {
		protected Database database;
		protected CodeWriter code = new CodeWriter();
		protected TypeTransformer type_transformer;

		public CodeGenerator(Database database) {
			this.database = database;
		}
	}
}
