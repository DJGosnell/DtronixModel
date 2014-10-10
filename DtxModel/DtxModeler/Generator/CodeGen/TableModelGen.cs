using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DtxModeler.Ddl;

namespace DtxModeler.Generator.CodeGen {
	class TableModelGen : CodeGenerator {

		public TableModelGen(Database database) : base(database) { }

		public string generate(Table db_table) {
			code.clear();

			Column pk_column = null;

			foreach (var column in db_table.Column) {
				if (pk_column == null && column.IsPrimaryKey) {
					pk_column = column;
				}
			}

			code.beginBlock("").writeLine();
			// Attributes
			code.write("[TableAttribute(Name = \"").write(db_table.Name).writeLine("\")]");
			code.beginBlock("public partial class ").write(db_table.Name).writeLine(" : Model {");

			// Table Properties;
			
			foreach(var column in db_table.Column) {
				bool read_only = false;

				if (column.IsDbGenerated) {
					read_only = true;
				}

				// Changed
				if (read_only == false) {
					code.write("private bool _").write(column.Name).writeLine("Changed = false;");
				}

				// Field Value
				code.write("private ").write(Enum.GetName(typeof(NetTypes), column.NetType)).write(" _").write(column.Name).writeLine(";");

				// Property
				code.beginBlock("public ").write(Enum.GetName(typeof(NetTypes), column.NetType)).write(" ").write(column.Name).writeLine(" {");

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
			}

			// Table Associations;
			/*foreach (var association in database.Association) {

				string field_type = association.Type;
				if (association.ParentAssociation != null && association.ParentAssociation.Cardinality == Cardinality.Many) {
					field_type += "[]";
				}

				// Caching Field Value
				code.write("private ").write(field_type).write(" _").write(association.Name).writeLine(";");

				// Association Property
				code.beginBlock("public ").write(field_type).write(" ").write(association.Name).writeLine(" {");
				code.beginBlock("get {").writeLine();
				code.beginBlock("if(_").write(association.Name).writeLine(" == null){ ");
				code.beginBlock("try {").writeLine("");
				code.write("_").write(association.Name).write(" = ((").write(database.Class).write(")context).")
					.write(association.Type).write(".select().whereIn(\"").write(association.OtherKeyColumn.Name).write("\", _").write(association.ThisKey).write(").executeFetch");

				if (association.ParentAssociation != null && association.ParentAssociation.Cardinality == Cardinality.Many) {
					code.writeLine("All();");
				} else {
					code.writeLine("();");
				}
				code.endBlock("} catch {").beginBlock("").writeLine();
				code.writeLine("//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.");
				code.write("_").write(association.Name).writeLine(" = null;");
				code.endBlock("}").writeLine(); // Try/Catch
				code.endBlock("}").writeLine(); // If
				code.write("return _").write(association.Name).writeLine(";");
				code.endBlock("}").writeLine(); // Get
				code.endBlock("}").writeLine(); // Property
				code.writeLine();
			}*/
			/*
			 * } catch (Exception e ) {
				throw new InvalidOperationException("SQL operations are not allowed outside of the Database Context.", e);
			}
			 * */

			// Constructors
			code.write("public ").write(db_table.Name).writeLine("() : this(null, null) { }");
			code.writeLine();

			code.beginBlock("public ").write(db_table.Name).writeLine("(DbDataReader reader, Context context) {");
			code.writeLine("read(reader, context);");
			code.endBlock("}").writeLine();
			code.writeLine();

			// read Override
			code.beginBlock("public override void read(DbDataReader reader, Context context) {").writeLine();
			code.writeLine("this.context = context;");
			code.writeLine("if (reader == null) { return; }");
			code.writeLine();
			code.writeLine("int length = reader.FieldCount;");
			code.beginBlock("for (int i = 0; i < length; i++) {").writeLine();
			code.beginBlock("switch (reader.GetName(i)) {").writeLine();
			// Read fields

			foreach (var column in db_table.Column) {
				string get_value_type = null;
				switch (column.NetType) {
					case NetTypes.Int64:
						get_value_type = "GetInt64";
						break;

					case NetTypes.Int16:
						get_value_type = "GetInt16";
						break;

					case NetTypes.Int32:
						get_value_type = "GetInt32";
						break;

					case NetTypes.UInt32:
					case NetTypes.UInt64:
					case NetTypes.ByteArray:
						throw new NotImplementedException("Unsigned inttegers are not handled at this time.");

					case NetTypes.Byte:
						get_value_type = "GetByte";
						break;

					case NetTypes.DateTime:
						get_value_type = "GetDateTime";
						break;

					case NetTypes.DateTimeOffset:
						get_value_type = "GetDateTime";
						break;

					case NetTypes.Decimal:
						get_value_type = "GetDouble";
						break;

					case NetTypes.Float:
						get_value_type = "GetFloat";
						break;

					case NetTypes.Double:
						get_value_type = "GetDouble";
						break;

					case NetTypes.Boolean:
						get_value_type = "GetBoolean";
						break;

					case NetTypes.String:
						break;

					case NetTypes.Char:
						get_value_type = "GetChar";
						break;

					default:
						throw new NotImplementedException("Unknown type.");
				}

				code.write("case \"").write(column.Name).write("\": _").write(column.Name).write(" = ");
				if (get_value_type != null) {

					code.write("(reader.IsDBNull(i)) ? default(").write(Enum.GetName(typeof(NetTypes), column.NetType)).write(") : ").write("reader.").write(get_value_type).write("(i)");
				} else {
					code.write("reader.GetValue(i) as ").write(Enum.GetName(typeof(NetTypes), column.NetType));
				}
				code.writeLine("; break;");

			}

			code.writeLine("default: break;");
			code.endBlock("}").writeLine();
			code.endBlock("}").writeLine();
			code.endBlock("}").writeLine();
			code.writeLine();

			// getChangedValues override
			code.beginBlock("public override Dictionary<string, object> getChangedValues() {").writeLine();
			code.writeLine("var changed = new Dictionary<string, object>();");

			foreach (var column in db_table.Column) {
				// Ignore primary keys.
				if (column.IsPrimaryKey) {
					continue;
				}
				code.beginBlock("if (_").write(column.Name).writeLine("Changed)");
				code.write("changed.Add(\"").write(column.Name).write("\", _").write(column.Name).endBlock(");").writeLine();
			}

			code.writeLine();
			code.writeLine("return changed;");

			code.endBlock("}").writeLine();
			code.writeLine();

			// getAllvalues method
			code.beginBlock("public override object[] getAllValues() {").writeLine();
			code.beginBlock("return new object[] {").writeLine();

			foreach (var column in db_table.Column) {
				// Ignore primary keys.
				if (column.IsPrimaryKey) {
					continue;
				}
				code.write("_").write(column.Name).writeLine(",");
			}

			code.endBlock("};").writeLine();
			code.endBlock("}").writeLine();
			code.writeLine();

			// getColumns method
			code.beginBlock("public override string[] getColumns() {").writeLine();
			code.beginBlock("return new string[] {").writeLine();

			foreach (var column in db_table.Column) {
				// Ignore primary keys.
				if (column.IsPrimaryKey) {
					continue;
				}
				code.write("\"").write(column.Name).writeLine("\",");
			}

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
				code.beginBlock("public override object getPKValue() {").writeLine();
				code.write("return _").write(pk_column.Name).writeLine(";");
				code.endBlock("}").writeLine();
				code.writeLine();
			}

			code.endBlock("}").writeLine();

			return code.ToString();
		}
	}
}
