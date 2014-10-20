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

			code.BeginBlock("").WriteLine();
			// Attributes
			code.Write("[TableAttribute(Name = \"").Write(db_table.Name).WriteLine("\")]");
			code.BeginBlock("public partial class ").Write(db_table.Name).WriteLine(" : Model {");

			// Table Properties;
			
			foreach(var column in db_table.Column) {
				bool read_only = false;

				if (column.IsDbGenerated || column.IsAutoIncrement) {
					read_only = true;
				}

				// Changed
				if (read_only == false) {
					code.Write("private bool _").Write(column.Name).WriteLine("Changed = false;");
				}

				// Determine type of field.
				string type = null;
				if (column.NetType == NetTypes.ByteArray) {
					type = "byte[]";
				} else {
					type = Enum.GetName(typeof(NetTypes), column.NetType);
				}

				// Field Value
				code.Write("private ").Write(type).Write(" _").Write(column.Name).WriteLine(";");

				// Property
				code.BeginBlock("public ").Write(type).Write(" ").Write(column.Name).WriteLine(" {");

				// Get
				code.Write("get { return _").Write(column.Name).WriteLine("; }");

				// Set
				if (read_only == false) {
					code.BeginBlock("set {").WriteLine();
					code.Write("_").Write(column.Name).WriteLine(" = value;");
					code.Write("_").Write(column.Name).WriteLine("Changed = true;");
					code.EndBlock("}").WriteLine();
				}
				code.EndBlock("}").WriteLine();

				code.WriteLine();
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
			code.Write("public ").Write(db_table.Name).WriteLine("() : this(null, null) { }");
			code.WriteLine();

			code.BeginBlock("public ").Write(db_table.Name).WriteLine("(DbDataReader reader, Context context) {");
			code.WriteLine("read(reader, context);");
			code.EndBlock("}").WriteLine();
			code.WriteLine();

			// read Override
			code.BeginBlock("public override void read(DbDataReader reader, Context context) {").WriteLine();
			code.WriteLine("this.context = context;");
			code.WriteLine("if (reader == null) { return; }");
			code.WriteLine();
			code.WriteLine("int length = reader.FieldCount;");
			code.BeginBlock("for (int i = 0; i < length; i++) {").WriteLine();
			code.BeginBlock("switch (reader.GetName(i)) {").WriteLine();
			// Read fields

			foreach (var column in db_table.Column) {
				string type = null;
				if (column.NetType == NetTypes.ByteArray) {
					type = "byte[]";
				} else {
					type = Enum.GetName(typeof(NetTypes), column.NetType);
				}

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
						throw new NotImplementedException("Unsigned inttegers are not handled at this time.");

					case NetTypes.ByteArray: 
						break;

					case NetTypes.Byte:
						get_value_type = "GetByteArray";
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

				code.Write("case \"").Write(column.Name).Write("\": _").Write(column.Name).Write(" = ");

				if (get_value_type != null) {
					code.Write("(reader.IsDBNull(i)) ? default(").Write(type).Write(") : ").Write("reader.GetFieldValue<").Write(type).Write(">(i)");
				} else {
					code.Write("reader.GetValue(i) as ").Write(type);
				}
				code.WriteLine("; break;");

			}

			code.WriteLine("default: break;");
			code.EndBlock("}").WriteLine();
			code.EndBlock("}").WriteLine();
			code.EndBlock("}").WriteLine();
			code.WriteLine();

			// getChangedValues override
			code.BeginBlock("public override Dictionary<string, object> getChangedValues() {").WriteLine();
			code.WriteLine("var changed = new Dictionary<string, object>();");

			foreach (var column in db_table.Column) {
				// Ignore primary keys.
				if (column.IsPrimaryKey) {
					continue;
				}
				code.BeginBlock("if (_").Write(column.Name).WriteLine("Changed)");
				code.Write("changed.Add(\"").Write(column.Name).Write("\", _").Write(column.Name).EndBlock(");").WriteLine();
			}

			code.WriteLine();
			code.WriteLine("return changed;");

			code.EndBlock("}").WriteLine();
			code.WriteLine();

			// getAllvalues method
			code.BeginBlock("public override object[] getAllValues() {").WriteLine();
			code.BeginBlock("return new object[] {").WriteLine();

			foreach (var column in db_table.Column) {
				// Ignore primary keys.
				if (column.IsPrimaryKey) {
					continue;
				}
				code.Write("_").Write(column.Name).WriteLine(",");
			}

			code.EndBlock("};").WriteLine();
			code.EndBlock("}").WriteLine();
			code.WriteLine();

			// getColumns method
			code.BeginBlock("public override string[] getColumns() {").WriteLine();
			code.BeginBlock("return new string[] {").WriteLine();

			foreach (var column in db_table.Column) {
				// Ignore primary keys.
				if (column.IsPrimaryKey) {
					continue;
				}
				code.Write("\"").Write(column.Name).WriteLine("\",");
			}

			code.EndBlock("};").WriteLine();
			code.EndBlock("}").WriteLine();
			code.WriteLine();

			if (pk_column != null) {
				// getPKName method
				code.BeginBlock("public override string getPKName() {").WriteLine();
				code.Write("return \"").Write(pk_column.Name).WriteLine("\";");
				code.EndBlock("}").WriteLine();
				code.WriteLine();

				// getPKValue
				code.BeginBlock("public override object getPKValue() {").WriteLine();
				code.Write("return _").Write(pk_column.Name).WriteLine(";");
				code.EndBlock("}").WriteLine();
				code.WriteLine();
			}

			code.EndBlock("}").WriteLine();

			return code.ToString();
		}
	}
}
