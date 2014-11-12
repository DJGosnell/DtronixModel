using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DtxModeler.Ddl;

namespace DtxModeler.Generator.CodeGen {
	class DatabaseContextGen : CodeGenerator {

		public DatabaseContextGen(Database database) : base(database) {
		}

		public string Generate() {
			code.clear();

			code.WriteLine("using System;");
			code.WriteLine("using System.Data.Common;");
			code.WriteLine("using System.Collections.Generic;");
			code.WriteLine("using DtxModel;");
			code.WriteLine();
			code.BeginBlock("namespace ").Write(database.Namespace).WriteLine(" {");
			code.WriteLine();
			code.BeginBlock("public partial class ").Write(database.ContextClass).WriteLine(" : Context {");
			code.WriteLine("private static Func<DbConnection> _default_connection = null;");
			code.WriteLine();
			code.WriteLine("/// <summary>");
			code.WriteLine("/// Set a default constructor to allow use of parameterless context calling.");
			code.WriteLine("/// </summary>");
			code.BeginBlock("public static Func<DbConnection> DefaultConnection {").WriteLine();
			code.WriteLine("get { return _default_connection; }");
			code.WriteLine("set { _default_connection = value; }");
			code.EndBlock("}").WriteLine();
			code.WriteLine();

			// Table properties.
			foreach (var table in database.Table) {
				code.Write("private Table<").Write(table.Name).Write("> _").Write(table.Name).WriteLine(";");
				code.WriteLine();
				code.BeginBlock("public Table<").Write(table.Name).Write("> ").Write(table.Name).WriteLine(" {");
				code.BeginBlock("get {").WriteLine();
				code.BeginBlock("if(_").Write(table.Name).WriteLine(" == null) {");
				code.Write("_").Write(table.Name).Write(" = new Table<").Write(table.Name).WriteLine(">(this);");
				code.EndBlock("}").WriteLine();
				code.WriteLine();
				code.Write("return _").Write(table.Name).WriteLine(";");
				code.EndBlock("}").WriteLine();
				code.EndBlock("}").WriteLine();
				code.WriteLine();
			}

			// Constructors
			code.WriteLine("/// <summary>");
			code.WriteLine("/// Create a new context of this database's type.  Can only be used if a default connection is specified.");
			code.WriteLine("/// </summary>");
			code.Write("public ").Write(database.ContextClass).WriteLine("() : base(_default_connection) { }");
			code.WriteLine();

			code.WriteLine("/// <summary>");
			code.WriteLine("/// Create a new context of this database's type with a specific connection.");
			code.WriteLine("/// </summary>");
			code.WriteLine("/// <param name=\"connection\">Existing open database connection to use.</param>");
			code.Write("public ").Write(database.ContextClass).WriteLine("(DbConnection connection) : base(connection) { }");
			code.EndBlock("}").WriteLine();

			return code.ToString();
		}
	}
}