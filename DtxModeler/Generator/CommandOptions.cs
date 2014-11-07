using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace DtxModeler.Generator {
	class ModelGenOptions : Options {

		[OptionAttribute("code-output", Required = false, HelpText = "The C# file to output the generated code to.")]
		public string CodeOutput { get; set; }

		[OptionAttribute("sql-output", Required = false)]
		public string SqlOutput { get; set; }

		[OptionAttribute("ddl-output", Required = false)]
		public string DdlOutput { get; set; }

		[OptionAttribute("output-db-type", Required = true, HelpText = "The type of datatabase we are dealing with.")]
		public string OutputDbType { get; set; }

		[OptionAttribute("db-class", Required = false, HelpText = "Allowed Values: SQLiteConnection")]
		public string DbClass {
			get {
				return "SQLiteConnection";
			}
		}

		[OptionAttribute("input", Required = true, HelpText = "The input that the generator will be working off of.")]
		public string Input { get; set; }

		[OptionAttribute("input-type", Required = true, HelpText = "Allowed Values: ddl|database-mysql|mwb")]
		public string InputType { get; set; }

		public ModelGenOptions(string[] args) : base(args) { }

	}
}
