using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DtxModeler.Ddl;

namespace DtxModeler.Generator.CodeGen {
	class DatabaseContextGen : CodeGenerator {

		public DatabaseContextGen(Database database) : base(database) {
		}

		private void WriteProperty(string property, string type, string description, string public_modifiers, string private_modifiers) {
			code.Write("private ").Write(private_modifiers).Write(" ").Write(type).Write(" _").Write(property).WriteLine(" = null;");
			code.WriteLine();
			WriteDescriptionComment(description);
			code.BeginBlock("public ").Write(public_modifiers).Write(" ").Write(type).Write(" ").Write(property).Write(" {").WriteLine();
			code.Write("get { return _").Write(property).WriteLine("; }");
			code.Write("set { _").Write(property).WriteLine(" = value; }");
			code.EndBlock("}").WriteLine();
			code.WriteLine();
		}

		private void WriteDescriptionComment(string comment) {
			code.WriteLine("/// <summary>");
			code.Write("/// ").WriteLine(comment);
			code.WriteLine("/// </summary>");
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
			WriteProperty("DefaultConnection", "Func<DbConnection>", "Set a default constructor to allow use of parameterless context calling.", "static", "static");
			WriteProperty("LastInsertIdQuery", "string", "Sets the querty string to retrieve the last insert ID.", "static new", "static");

			code.WriteLine("private static TargetDb _DatabaseType;").WriteLine();
			WriteDescriptionComment("Type of database this context will target.  Automatically sets proper database specific values.");
			code.BeginBlockLine("public static TargetDb DatabaseType {");
			code.WriteLine("get { return _DatabaseType; }");
			code.BeginBlockLine("set {");
			code.WriteLine("_DatabaseType = value;");
			code.BeginBlockLine("switch (value) {");
			code.BeginBlockLine("case TargetDb.MySql:");
			code.WriteLine("LastInsertIdQuery = \"SELECT last_insert_id()\";");
			code.Write("break;").EndBlockLine();

			code.BeginBlockLine("case TargetDb.Sqlite:");
			code.WriteLine("LastInsertIdQuery = \"SELECT last_insert_rowid()\";");
			code.Write("break;").EndBlockLine();

			code.BeginBlockLine("case TargetDb.Other:");
			code.Write("break;").EndBlockLine();
			code.EndBlockLine("}");
			code.EndBlockLine("}");
			code.EndBlockLine("}");
/*
 * 
 * 		private static TargetDb _DatabaseType;

		/// <summary>
		/// Type of database this context will target.  Automatically sets proper specific values.
		/// </summary>
		public static TargetDb DatabaseType {
			get { return _DatabaseType; }
			set { 
				_DatabaseType = value;
				switch (value) {
					case TargetDb.MySql:
						LastInsertIdQuery = "SELECT last_insert_id()";
						break;
					case TargetDb.Sqlite:
						LastInsertIdQuery = "SELECT last_insert_rowid()";
						break;
					case TargetDb.Other:
						break;
				}
			}
		}*/





			// Table properties.
			foreach (var table in database.Table) {
				code.Write("private Table<").Write(table.Name).Write("> _").Write(table.Name).WriteLine(";");
				code.WriteLine();
				code.BeginBlock("public Table<").Write(table.Name).Write("> ").Write(table.Name).WriteLine(" {");
				code.BeginBlockLine("get {");
				code.BeginBlock("if(_").Write(table.Name).WriteLine(" == null) {");
				code.Write("_").Write(table.Name).Write(" = new Table<").Write(table.Name).WriteLine(">(this);");
				code.EndBlockLine("}");
				code.WriteLine();
				code.Write("return _").Write(table.Name).WriteLine(";");
				code.EndBlockLine("}");
				code.EndBlockLine("}");
				code.WriteLine();
			}

			// Constructors
			WriteDescriptionComment("Create a new context of this database's type.  Can only be used if a default connection is specified.");
			code.Write("public ").Write(database.ContextClass).WriteLine("() : base(_DefaultConnection, _LastInsertIdQuery) { }");
			code.WriteLine();

			WriteDescriptionComment("Create a new context of this database's type with a specific connection.");
			code.WriteLine("/// <param name=\"connection\">Existing open database connection to use.</param>");
			code.Write("public ").Write(database.ContextClass).WriteLine("(DbConnection connection) : base(connection, _LastInsertIdQuery) { }");
			code.EndBlock("}").WriteLine();

			return code.ToString();
		}
	}
}