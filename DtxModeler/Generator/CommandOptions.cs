using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using DtxModeler.Ddl;
using NDesk.Options;
namespace DtxModeler.Generator {
	class ModelGenOptions {

		[OptionAttribute("code-output", Required = false, DefaultValue = "null", HelpText = "The C# file to output the generated code to.")]
		public string CodeOutput { get; set; }

		[OptionAttribute("sql-output", Required = false, DefaultValue = "null", HelpText = "The sql file to output the generated SQL table code to.")]
		public string SqlOutput { get; set; }

		[OptionAttribute("ddl-output", Required = false, DefaultValue="null", HelpText = "The ddl file to output the generated DDL to.")]
		public string DdlOutput { get; set; }

		[OptionAttribute("output-db-type", Required = true, HelpText = "The type of datatabase we are dealing with.")]
		public DbProvider OutputDbType { get; set; }

		[OptionAttribute("input", Required = true, HelpText = "The input that the generator will be working off of.")]
		public string Input { get; set; }

		[OptionAttribute("input-type", Required = true, HelpText = "Allowed Values: ddl|database-mysql|mwb")]
		public string InputType { get; set; }

		public ModelGenOptions(string[] args){
			var option_parser = new OptionSet();
			option_parser.Add("code-output", "The C# file to output the generated code to.", (string test) => CodeOutput = test);
			option_parser.Add("sql-output", "The sql file to output the generated SQL table code to.", (string test) => SqlOutput = test);
			try {
				var ob = option_parser.Parse(args);
			} catch (OptionException e) {
				Console.Write("Error: ");
				Console.WriteLine(e.Message);
				Console.WriteLine("Try `greet --help' for more information.");
				return;
			}
		}

	}
}
