using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DtxModeler.Ddl;
using System.Data.SqlTypes;

namespace DtxModeler.Generator.CodeGen {
	class TableModelGen : CodeGenerator {

		public TableModelGen(Database database) : base(database) { }

		private string ColumnNetType(Column column) {
			if (column.NetType == NetTypes.ByteArray) {
				return "byte[]";
			}

			string type = Enum.GetName(typeof(NetTypes), column.NetType);

			if (column.Nullable) {
				switch (column.NetType) {
					case NetTypes.Int64:
					case NetTypes.Int16:
					case NetTypes.Int32:
					case NetTypes.Byte:
					case NetTypes.Decimal:
					case NetTypes.Float:
					case NetTypes.Double:
					case NetTypes.Boolean:
					case NetTypes.Char:
						type += "?";
						break;
				}
			}

			return type;
		}

		public string Generate(Table db_table) {
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
				// Changed
				if (column.IsReadOnly == false) {
					code.Write("private bool _").Write(column.Name).WriteLine("Changed = false;");
				}

				// Determine type of field.
				string type = ColumnNetType(column);

				// Field Value
				code.Write("private ").Write(type).Write(" _").Write(column.Name).WriteLine(";");

				// Property Comment
				if (string.IsNullOrWhiteSpace(column.Description) == false) {
					code.WriteLine("/// <summary>");
					code.Write("/// ").WriteLine(column.Description);
					code.WriteLine("/// </summary>");
				}

				// Property
				code.BeginBlock("public ").Write(type).Write(" ").Write(column.Name).WriteLine(" {");

				// Get
				code.Write("get { return _").Write(column.Name).WriteLine("; }");

				// Set
				if (column.IsReadOnly == false) {
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
			code.WriteLine("Read(reader, context);");
			code.EndBlock("}").WriteLine();
			code.WriteLine();

			// read Override
			code.BeginBlock("public override void Read(DbDataReader reader, Context context) {").WriteLine();
			code.WriteLine("this.context = context;");
			code.WriteLine("if (reader == null) { return; }");
			code.WriteLine();
			code.WriteLine("int length = reader.FieldCount;");
			code.BeginBlock("for (int i = 0; i < length; i++) {").WriteLine();
			code.BeginBlock("switch (reader.GetName(i)) {").WriteLine();
			// Read fields

			foreach (var column in db_table.Column) {
				string type = ColumnNetType(column);
				string reader_get = Enum.GetName(typeof(NetTypes), column.NetType);
				bool cast_as = false;
				switch (column.NetType) {
					case NetTypes.String:
						cast_as = true;
						break;
				}

				code.Write("case \"").Write(column.Name).Write("\": _").Write(column.Name).Write(" = ");

				if (column.NetType == NetTypes.ByteArray) {
					code.Write("(reader.IsDBNull(i)) ? default(Byte[]) : ").Write("reader.GetFieldValue<").Write(reader_get).Write(">(i)");
				} else if (cast_as) {
					code.Write("reader.GetValue(i) as ").Write(type);
				}else if(column.Nullable){
					code.Write("(reader.IsDBNull(i)) ? default(").Write(type).Write(") : ").Write("reader.Get").Write(reader_get).Write("(i)");
				} else {
					code.Write("reader.Get").Write(reader_get).Write("(i)");
				}
				code.WriteLine("; break;");

			}

			code.WriteLine("default: break;");
			code.EndBlock("}").WriteLine();
			code.EndBlock("}").WriteLine();
			code.EndBlock("}").WriteLine();
			code.WriteLine();

			// getChangedValues override
			code.BeginBlock("public override Dictionary<string, object> GetChangedValues() {").WriteLine();
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
			code.BeginBlock("public override object[] GetAllValues() {").WriteLine();
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
			code.BeginBlock("public override string[] GetColumns() {").WriteLine();
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
				code.BeginBlock("public override string GetPKName() {").WriteLine();
				code.Write("return \"").Write(pk_column.Name).WriteLine("\";");
				code.EndBlock("}").WriteLine();
				code.WriteLine();

				// getPKValue
				code.BeginBlock("public override object GetPKValue() {").WriteLine();
				code.Write("return _").Write(pk_column.Name).WriteLine(";");
				code.EndBlock("}").WriteLine();
				code.WriteLine();
			}

			code.EndBlock("}").WriteLine();

			return code.ToString();
		}
	}
}
