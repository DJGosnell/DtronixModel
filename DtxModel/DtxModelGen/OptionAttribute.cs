using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DtxModelGen {

	/// <summary>
	/// Provides base properties for creating an attribute, used to define rules for command line parsing.
	/// </summary>
	public class OptionAttribute : Attribute {

		/// <summary>
		/// Initializes a new instance of the OptionAttribute class.
		/// </summary>
		public OptionAttribute() { }

		/// <summary>
		/// Initializes a new instance of the OptionAttribute class.
		/// Validating shortName and longName. This constructor accepts a System.Nullable<T> as short name.
		/// </summary>
		/// <param name="shortName">Short name of the option.</param>
		/// <param name="longName">Long name of the option.</param>
		public OptionAttribute(char? short_name, string long_name) {
			ShortName = short_name;
			LongName = long_name;
		}

		/// <summary>
		/// Initializes a new instance of the OptionAttribute class.
		/// Validating shortName and longName.
		/// </summary>
		/// <param name="longName">Long name of the option.</param>
		public OptionAttribute(string long_name) {
			LongName = long_name;
		}

		/// <summary>
		/// Gets or sets mapped property default value.
		/// </summary>
		public object DefaultValue { get; set; }

		/// <summary>
		/// Gets or sets a short description of this command line option. Usually a sentence summary.
		/// </summary>
		public string HelpText { get; set; }

		/// <summary>
		/// Gets long name of this command line option. This name is usually a single english word.
		/// </summary>
		public string LongName { get; set; }

		/// <summary>
		/// Gets or sets mapped property meta value.
		/// </summary>
		public string MetaValue { get; set; }

		/// <summary>
		/// Gets or sets the option's mutually exclusive set.
		/// </summary>
		public string MutuallyExclusiveSet { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether a command line option is required.
		/// </summary>
		public bool Required { get; set; }

		/// <summary>
		/// Gets a short name of this command line option. You can use only one character.
		/// </summary>
		public char? ShortName { get; set; }

	}
}
