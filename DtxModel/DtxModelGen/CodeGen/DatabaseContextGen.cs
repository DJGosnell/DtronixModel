using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DtxModel.Ddl;

namespace DtxModelGen.CodeGen {
	class DatabaseContextGen : CodeGenerator {

		public DatabaseContextGen(Database database) : base(database) {
		}

		public string generate() {
			code.clear();

			code.writeLine("using System;");
			code.writeLine("using System.Data.Common;");
			code.writeLine("using System.Collections.Generic;");
			code.writeLine("using DtxModel;");
			code.writeLine();
			code.beginBlock("namespace ").write(database.ContextNamespace).writeLine(" {");
			code.writeLine();
			code.beginBlock("public partial class ").write(database.Class).writeLine(" : Context {");
			code.writeLine("private static Func<DbConnection> _default_connection = null;");
			code.writeLine();
			code.writeLine("/// <summary>");
			code.writeLine("/// Set a default constructor to allow use of parameterless context calling.");
			code.writeLine("/// </summary>");
			code.beginBlock("public static Func<DbConnection> DefaultConnection {").writeLine();
			code.writeLine("get { return _default_connection; }");
			code.writeLine("set { _default_connection = value; }");
			code.endBlock("}").writeLine();
			code.writeLine();

			// Table properties.
			foreach (var table in database.Table) {
				code.write("private Table<").write(table.Name).write("> _").write(table.Name).writeLine(";");
				code.writeLine();
				code.beginBlock("public Table<").write(table.Name).write("> ").write(table.Name).writeLine(" {");
				code.beginBlock("get {").writeLine();
				code.beginBlock("if(_").write(table.Name).writeLine(" == null) {");
				code.write("_").write(table.Name).write(" = new Table<").write(table.Name).writeLine(">(this);");
				code.endBlock("}").writeLine();
				code.writeLine();
				code.write("return _").write(table.Name).writeLine(";");
				code.endBlock("}").writeLine();
				code.endBlock("}").writeLine();
				code.writeLine();
			}

			// Constructors
			code.writeLine("/// <summary>");
			code.writeLine("/// Create a new context of this database's type.  Can only be used if a default connection is specified.");
			code.writeLine("/// </summary>");
			code.write("public ").write(database.Class).writeLine("() : base(_default_connection) { }");
			code.writeLine();

			code.writeLine("/// <summary>");
			code.writeLine("/// Create a new context of this database's type with a specific connection.");
			code.writeLine("/// </summary>");
			code.writeLine("/// <param name=\"connection\">Existing open database connection to use.</param>");
			code.write("public ").write(database.Class).writeLine("(DbConnection connection) : base(connection) { }");
			code.endBlock("}").writeLine();

			return code.ToString();
		}
	}
}