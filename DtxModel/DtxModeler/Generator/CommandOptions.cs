using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace DtxModeler.Generator {
	class ModelGenOptions : Options {

		[OptionAttribute("code-outupt", Required = false, HelpText = "The C# file to output the generated code to.")]
		public string CodeOutput { get; set; }

		[OptionAttribute("code-type", Required = false, HelpText = "The type of C# code the generate.  Allowed values: DtxModel.")]
		public string CodeType {
			get {
				return "DtxModel";
			}
		}

		[OptionAttribute("db-outupt", Required = false, HelpText = "The connection string for the database.")]
		public string DbOutput { get; set; }

		[OptionAttribute("sql-outupt", Required = false)]
		public string SqlOutput { get; set; }

		[OptionAttribute("ddl-outupt", Required = false)]
		public string DdlOutput { get; set; }

		[OptionAttribute("input", Required = true, HelpText = "The input that the generator will be working off of.")]
		public string Input { get; set; }

		[OptionAttribute("input-type", Required = true, HelpText = "Allowed Values: ddl|sql(TBD)|database")]
		public string InputType { get; set; }

		[OptionAttribute("db-class", Required = false, HelpText = "Allowed Values: SQLiteConnection")]
		public string DbClass { get; set; }

		public ModelGenOptions(string[] args) : base(args) { }

	}
}
