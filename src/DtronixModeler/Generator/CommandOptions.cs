using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using DtronixModeler.Ddl;
using NDesk.Options;
using System.Windows;
namespace DtronixModeler.Generator {
	class CommandOptions {

		public enum InType {
			Ddl,
			DatabaseSqlite,
			//DatabaseMysql,
			Mwb,
			MwbXml
		}

		private TextWriter writer;
		private OptionSet option_set;

		public bool ParseSuccess { get; set; }


		public string Input { get; set; }

		public InType? InputType { get; set; }

		public DbProvider? DbProvider { get; set; }

		public string CodeOutput { get; set; }

		public string SqlOutput { get; set; }

		public string DdlOutput { get; set; }

		public string Namespace { get; set; }

		public string ContextClass { get; set; }

	    public bool NotifyPropertyChanged { get; set; }

	    public bool ProtobufDataContracts { get; set; }

        public CommandOptions() { }

		public CommandOptions(string[] args, TextWriter writer){
			ParseSuccess = false;
			this.writer = writer;
			bool help = false;
			option_set = new OptionSet();
			
			option_set.Add("i=|input=", "(Required) Input which will be used to open or generate a model.", v => Input = v);
			option_set.Add<InType?>("t=|input-type=", "(Required) The type of datatabase we are dealing with. " + EnumValues<InType>(), v => InputType = v);
			option_set.Add<DbProvider?>("db-provider=", "The type of datatabase we are dealing with. " + EnumValues<DbProvider>(), v => DbProvider = v);

			option_set.Add("code-output:", "The C# file to output the generated code to.", v => CodeOutput = (v == null) ? "" : v);
			option_set.Add("sql-output:", "The sql file to output the generated SQL table code to.", v => SqlOutput = (v == null)? "" : v);
			option_set.Add("ddl-output:", "The ddl file to output the generated DDL to.", v => DdlOutput = (v == null) ? "" : v);

			option_set.Add("namespace=", "Namespace used to contain all of the code objects & classes.", v => Namespace = (v == null) ? "" : v);
			option_set.Add("context-class=", "Class name for the main context.", v => ContextClass = (v == null) ? "" : v);

		    option_set.Add("notifypropertychanged=", "Set to true if the INotifyPropertyChanged class should be implemented.",
		        v => NotifyPropertyChanged = bool.Parse(v));
		    option_set.Add("protobufdatacontracts=", "Set to true if protobuf-net data contracts should be implemented.",
		        v => ProtobufDataContracts = bool.Parse(v));

            option_set.Add("h|?|help", "Displays this help menu.", v => {
				help = true;
				Help();
			});

			try {
				var remaining_options = option_set.Parse(args);

				if (help) {
					ParseSuccess = true;
					return;
				}

				if (remaining_options.Count > 0) {
					throw new OptionException("Could not parse option.", remaining_options[0]);
				}

				if (Input == null) {
					throw new OptionException("Required value for --input was not specified.", "input");
				}

				if (InputType.HasValue == false) {
					throw new OptionException("Required value for --input-type was not specified.", "input-type");
				}

			} catch (OptionException e) {
				writer.Write("Error: ");
				writer.WriteLine(e.Message);
				writer.Write("Option: ");
				writer.WriteLine(e.OptionName);
				writer.WriteLine("Type '--help' for more information about program usage.");

				return;
			}

			ParseSuccess = true;
		}

		private string EnumValues<T>() {
			return "Valid Values: [" + string.Join(", ", Enum.GetNames(typeof(T))) + "]";
		}


		private void Help() {

			writer.WriteLine("Dtronix Model Version " + Assembly.GetExecutingAssembly().GetName().Version.ToString());

			writer.WriteLine("Options:");
			option_set.WriteOptionDescriptions(writer);
		}

	}
}
