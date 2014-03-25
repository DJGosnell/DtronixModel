using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DtxModelGen.Schema.Dbml;

namespace DtxModelGen.CodeGen {
	class TableModelGen : CodeGenerator {
		private Table _db_table;

		public Table DbTable {
			get { return _db_table; }
			set { _db_table = value; }
		}

		private string _ns;

		public string Ns {
			get { return _ns; }
			set { _ns = value; }
		}

		private TypeTransformer _type_transformer;

		public TypeTransformer Transformer {
			get { return _type_transformer; }
			set { _type_transformer = value; }
		}


		public TableModelGen() {
		}


		public string generate() {
			code.clear();

			Column pk_column = null;

			each<Column>(column => {
				if (pk_column == null && column.IsPrimaryKeySpecified && column.IsPrimaryKey) {
					pk_column = column;
				}
			});

			code.beginBlock("").writeLine();
			// Attributes
			code.write("[TableAttribute(Name = \"").write(_db_table.Name).writeLine("\")]");
			code.beginBlock("public class ").write(_db_table.Name).writeLine(" : Model {");

			// Table Properties;
			each<Column>(column => {
				bool read_only = false;

				if (column.IsDbGeneratedSpecified && column.IsDbGenerated) {
					read_only = true;
				}

				// Changed
				if (read_only == false) {
					code.write("private bool _").write(column.Name).writeLine("Changed = false;");
				}

				// Field Value
				code.write("private ").write(column.Type).write(" _").write(column.Name).writeLine(";");

				// Property
				code.beginBlock("public ").write(column.Type).write(" ").write(column.Name).writeLine(" {");

				// Get
				code.write("get { return _").write(column.Name).writeLine("; }");

				// Set
				if (read_only == false) {
					code.beginBlock("set {").writeLine();
					code.write("_").write(column.Name).writeLine(" = value;");
					code.write("_").write(column.Name).writeLine("Changed = true;");
					code.endBlock("}").writeLine();
				}
				code.endBlock("}").writeLine();

				code.writeLine();
			});

			//Table associations
			each<Association>(association => {
				//association.

			});

			// Constructors
			code.write("public ").write(_db_table.Name).writeLine("() : this(null, null) { }");
			code.writeLine();

			code.beginBlock("public ").write(_db_table.Name).writeLine("(DbDataReader reader, DbConnection connection) {");
			code.writeLine("read(reader, connection);");
			code.endBlock("}").writeLine();
			code.writeLine();

			// read Override
			code.beginBlock("public override void read(DbDataReader reader, DbConnection connection) {").writeLine();
			code.writeLine("this.connection = connection;");
			code.writeLine("if (reader == null) { return; }");
			code.writeLine();
			code.writeLine("int length = reader.FieldCount;");
			code.beginBlock("for (int i = 0; i < length; i++) {").writeLine();
			code.beginBlock("switch (reader.GetName(i)) {").writeLine();
			// Read fields
			each<Column>(column => {
				bool hard_cast = false;
				switch (column.Type.ToLower()) {
					case "system.int16":
					case "system.int32":
					case "system.int64":
					case "system.uint16":
					case "system.uint32":
					case "system.uint64":
					case "short":
					case "int":
					case "long":
						hard_cast = true;
						break;
				}

				code.write("case \"").write(column.Name).write("\": _").write(column.Name).write(" = ");
				if (hard_cast) {
					code.write("(").write(column.Type).write(")reader.GetValue(i)");
				} else {
					code.write("reader.GetValue(i) as ").write(column.Type);
				}
				code.writeLine("; break;");

			});

			code.writeLine("default: break;");
			code.endBlock("}").writeLine();
			code.endBlock("}").writeLine();
			code.endBlock("}").writeLine();
			code.writeLine();

			// getChangedValues override
			code.beginBlock("public override Dictionary<string, object> getChangedValues() {").writeLine();
			code.writeLine("var changed = new Dictionary<string, object>();");
			each<Column>(column => {
				// Ignore primary keys.
				if (column.IsPrimaryKeySpecified && column.IsPrimaryKey) {
					return;
				}
				code.beginBlock("if (_").write(column.Name).writeLine("Changed)");
				code.write("changed.Add(\"").write(column.Name).write("\", _").write(column.Name).endBlock(");").writeLine();
			});
			code.writeLine();
			code.writeLine("return changed;");

			code.endBlock("}").writeLine();
			code.writeLine();

			// getAllvalues method
			code.beginBlock("public override object[] getAllValues() {").writeLine();
			code.beginBlock("return new object[] {").writeLine();

			each<Column>(column => {
				// Ignore primary keys.
				if (column.IsPrimaryKeySpecified && column.IsPrimaryKey) {
					return;
				}
				code.write("_").write(column.Name).writeLine(",");
			});

			code.endBlock("};").writeLine();
			code.endBlock("}").writeLine();
			code.writeLine();

			// getColumns method
			code.beginBlock("public override string[] getColumns() {").writeLine();
			code.beginBlock("return new string[] {").writeLine();

			each<Column>(column => {
				// Ignore primary keys.
				if (column.IsPrimaryKeySpecified && column.IsPrimaryKey) {
					return;
				}
				code.write("\"").write(column.Name).writeLine("\",");
			});

			code.endBlock("};").writeLine();
			code.endBlock("}").writeLine();
			code.writeLine();

			if (pk_column != null) {
				// getPKName method
				code.beginBlock("public override string getPKName() {").writeLine();
				code.write("return \"").write(pk_column.Name).writeLine("\";");
				code.endBlock("}").writeLine();
				code.writeLine();

				// getPKValue
				code.beginBlock("public override ").write(pk_column.Type).write(" getPKValue() {").writeLine();
				code.write("return _").write(pk_column.Name).writeLine(";");
				code.endBlock("}").writeLine();
				code.writeLine();
			}

			code.endBlock("}").writeLine();

			return code.ToString();
		}

		private void each<T>(Action<T> method) where T : class {
			foreach (var item in _db_table.Type.Items) {
				if (item is T) {
					method(item as T);
				}
			}
		}
	}
}
