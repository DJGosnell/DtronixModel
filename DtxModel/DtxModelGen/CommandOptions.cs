using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;
using System.IO;

namespace DtxModelGen {
	class CommandOptions {
		[Option("code-outupt", Required = false, HelpText = "The C# file to output the generated code to.")]
		public string CodeOutput { get; set; }

		[Option("code-type", Required = false, HelpText = "The type of C# code the generate.  Allowed values: DtxModel.")]
		public string CodeType { get; set; }

		[Option("code-namespace", Required = false, HelpText = "Namespace to place the generated code inside.")]
		public string CodeNamespace { get; set; }

		[Option("sql-outupt", Required = false)]
		public string SqlOutput { get; set; }

		[Option("database-output", Required = false)]
		public string DatabaseOutput { get; set; }


		private string _dbml_input;

		[Option("dbml-input", Required = true)]
		public string DbmlInput {
			get { return _dbml_input; }
			set {
				if (File.Exists(value) == false) {
					throw new ArgumentException("The dbml-input file does not exist.");
				}
				_dbml_input = value;
			}
		}

	}
}
