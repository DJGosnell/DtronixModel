using DtronixModeler.Ddl;
using DtronixModeler.Generator.MySql;
using DtronixModeler.Generator.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DtronixModeler.Generator.Output {
	class ProtobufGenerator
    {
		private Database database;
		private CodeWriter code = new CodeWriter();

		public ProtobufGenerator(Database database) { 

			this.database = database;
		}

		public string Generate() {
			code.clear();

            code.WriteLine("syntax = \"proto3\";");
            code.WriteLine($"package {database.ProtobufPackage};");

            // Loop through each of the tables.
            foreach (var table in database.Table) {
                code.WriteLine();
                code.BeginBlock("message ").Write(table.Name).WriteLine(" {");

                // Columns
                for (int i = 0; i < table.Column.Count; i++)
                {
                    string protobufType = ProtobufTypeTransformer.GetProtobufType(table.Column[i].NetType);

                    if(string.IsNullOrWhiteSpace(protobufType))
                        Console.WriteLine(table.Column[i].NetType);

                    code.WriteLine($"{protobufType} {table.Column[i].Name} = {i + 1};");
                }

                code.EndBlock("}").WriteLine();
            }

            //TODO: Handle Enums

            return code.ToString();
        }
            
		
	}

}
