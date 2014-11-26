using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DtxModeler.Ddl;
using System.Data.SqlTypes;

namespace DtxModeler.Generator.CodeGen {
	class TableModelGen : CodeGenerator {


		private class AssociationCodeGenerator {
			public Table ThisTable { get; set; }
			public Column ThisColumn { get; set; }
			public string ThisAssociationName { get; set; }
			public Cardinality ThisCardinality { get; set; }

			public Table OtherTable { get; set; }
			public Column OtherColumn { get; set; }
			public string OtherAssociationName { get; set; }
			public Cardinality OtherCardinality { get; set; }
		}


		public TableModelGen(Database database) : base(database) { }

		private string ColumnNetType(Column column) {
			if (column.NetType == NetTypes.ByteArray) {
				return "byte[]";
			}

			string type = Enum.GetName(typeof(NetTypes), column.NetType);

			if (column.NetType == NetTypes.Float) {
				type = "float";
			}

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
					code.BeginBlockLine("set {");
					code.Write("_").Write(column.Name).WriteLine(" = value;");
					code.Write("_").Write(column.Name).WriteLine("Changed = true;");
					code.EndBlockLine("}");
				}
				code.EndBlockLine("}");

				code.WriteLine();
			}

			// Table Associations;
			foreach (var db_assoc in database.Association) {
				var reference = db_assoc.ReferencesTable(db_table);
				var assoc = new AssociationCodeGenerator();

				if (reference == Association.Reference.R1) {
					assoc.ThisAssociationName = db_assoc.Table1Name;
					assoc.ThisColumn = db_assoc.GetReferenceColumn(database, Association.Reference.R1);
					assoc.ThisTable = db_table;
					assoc.ThisCardinality = db_assoc.Table1Cardinality;

					assoc.OtherAssociationName = db_assoc.Table2Name;
					assoc.OtherColumn = db_assoc.GetReferenceColumn(database, Association.Reference.R2);
					assoc.OtherTable = database.Table.Single(t => t.Name == db_assoc.Table2);
					assoc.OtherCardinality = db_assoc.Table2Cardinality;
					

				} else if(reference == Association.Reference.R2) {
					assoc.ThisAssociationName = db_assoc.Table2Name;
					assoc.ThisColumn = db_assoc.GetReferenceColumn(database, Association.Reference.R2);
					assoc.ThisTable = db_table;
					assoc.ThisCardinality = db_assoc.Table2Cardinality;

					assoc.OtherAssociationName = db_assoc.Table1Name;
					assoc.OtherColumn = db_assoc.GetReferenceColumn(database, Association.Reference.R1);
					assoc.OtherTable = database.Table.Single(t => t.Name == db_assoc.Table1);
					assoc.OtherCardinality = db_assoc.Table1Cardinality;

				} else {
					continue;
				}




				string field_type = assoc.OtherTable.Name;
				if (assoc.OtherCardinality == Cardinality.Many) {
					field_type += "[]";
				}

				// Caching Field Value
				code.Write("private ").Write(field_type).Write(" _").Write(assoc.OtherAssociationName).WriteLine(";");

				// Association Property
				code.BeginBlock("public ").Write(field_type).Write(" ").Write(assoc.OtherAssociationName).WriteLine(" {");
				code.BeginBlockLine("get {");
				code.BeginBlock("if(_").Write(assoc.OtherAssociationName).WriteLine(" == null){ ");
				code.BeginBlockLine("try {");
				code.Write("_").Write(assoc.OtherAssociationName).Write(" = ((").Write(database.ContextClass).Write(")context).")
					.Write(assoc.OtherTable.Name).Write(".Select().WhereIn(\"").Write(assoc.OtherColumn.Name).Write("\", _").Write(assoc.ThisColumn.Name).Write(").ExecuteFetch");

				if (assoc.OtherCardinality == Cardinality.Many) {
					code.WriteLine("All();");
				} else {
					code.WriteLine("();");
				}
				code.EndBlock("} catch {").BeginBlockLine("");
				code.WriteLine("//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.");
				code.Write("_").Write(assoc.OtherAssociationName).WriteLine(" = null;");
				code.EndBlockLine("}"); // Try/Catch
				code.EndBlockLine("}"); // If
				code.Write("return _").Write(assoc.OtherAssociationName).WriteLine(";");
				code.EndBlockLine("}"); // Get
				code.EndBlockLine("}"); // Property
				code.WriteLine();
			}
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
					code.Write("(reader.IsDBNull(i)) ? default(byte[]) : ").Write("reader.GetFieldValue<byte[]>(i)");
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
