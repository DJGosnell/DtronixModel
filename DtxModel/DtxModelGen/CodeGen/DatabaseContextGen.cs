using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DtxModelGen.Schema.Dbml;

namespace DtxModelGen.CodeGen {
	class DatabaseContextGen : CodeGenerator {
		private string _ns;

		public string Ns {
			get { return _ns; }
			set { _ns = value; }
		}

		private Database _database;

		public Database DatabaseName {
			get { return _database; }
			set { _database = value; }
		}


		


		public string generate() {
			code.clear();

			code.writeLine("using System;");
			code.writeLine("using System.Data.Common;");
			code.writeLine("using System.Collections.Generic;");
			code.writeLine("using DtxModel;");
			code.writeLine();
			code.beginBlock("namespace ").write(_ns).writeLine(" {");
			code.writeLine();
			code.beginBlock("public class ").write(_database.Class).writeLine(" : Context {");
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
			foreach (var table in _database.Table) {
				code.write("private Table<").write(table.Name).write("> _").write(table.Name).writeLine(";");
				code.writeLine();
				code.beginBlock("private Table<").write(table.Name).write("> ").write(table.Name).writeLine(" {");
				code.beginBlock("get {").writeLine();
				code.beginBlock("if(_").write(table.Name).writeLine(" == null) {");
				code.write("_").write(table.Name).write(" = new Table<").write(table.Name).writeLine(">(connection);");
				code.endBlock("}").writeLine();
				code.writeLine();
				code.write("return _").write(table.Name).write(";");
				code.endBlock("}").writeLine();
				code.endBlock("}").writeLine();
				code.writeLine();
			}

			// Constructors
			code.writeLine("/// <summary>");
			code.writeLine("/// Create a new context of this database's type.  Can only be used if a default connection is specified.");
			code.writeLine("/// </summary>");
			code.write("public ").write(_database).writeLine("Context() : base(_default_connection) { }");
			code.writeLine();

			code.writeLine("/// <summary>");
			code.writeLine("/// Create a new context of this database's type with a specific connection.");
			code.writeLine("/// </summary>");
			code.writeLine("/// <param name=\"connection\">Existing open database connection to use.</param>");
			code.write("public ").write(_database).writeLine("Context(DbConnection connection) : base(connection) { }");
			code.endBlock("}").writeLine();

			return code.ToString();
		}
	}
}