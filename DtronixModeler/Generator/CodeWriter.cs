using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DtronixModeler.Generator {
	class CodeWriter {
		int indent_level = 0;
		bool fresh_line = true;
		StringBuilder code = new StringBuilder();

		public CodeWriter() {
		}

		public CodeWriter Write(string value) {
			indent();
			code.Append(value);
			return this;
		}

		public CodeWriter Write(object value) {
			indent();
			code.Append(value);
			return this;
		}

		public CodeWriter BeginBlock(string value) {
			indent();
			code.Append(value);
			indent_level++;
			return this;
		}

		public CodeWriter BeginBlockLine(string value) {
			indent();
			code.AppendLine(value);
			fresh_line = true;
			indent_level++;
			return this;
		}

		public CodeWriter EndBlock(string value) {
			indent_level--;
			indent();
			code.Append(value);
			return this;
		}

		public CodeWriter EndBlockLine(string value = null) {
			indent_level--;
			indent();
			code.AppendLine(value);
			fresh_line = true;
			return this;
		}

		public CodeWriter WriteLine(string value) {
			indent();
			code.AppendLine(value);
			fresh_line = true;
			return this;
		}

		public CodeWriter WriteLine() {
			code.AppendLine();
			fresh_line = true;
			return this;
		}

		/// <summary>
		/// Removes a certian number of characters from the end of the string ignoring new lines.
		/// </summary>
		/// <param name="length">Number of characters to remove.</param>
		/// <returns></returns>
		public CodeWriter removeLength(int length) {
			int start_pos = code.Length;
			if (fresh_line) {
				start_pos -= Environment.NewLine.Length;
			}

			code.Remove(start_pos - length, length);

			return this;
		}

		private void indent() {
			if (fresh_line) {
				code.Append(new string('\t', indent_level));
				fresh_line = false;
			}
		}



		public override string ToString() {
			return code.ToString();
		}

		public CodeWriter remove(int start_index, int length) {
			code.Remove(start_index, length);
			return this;
		}

		// public 

		public void clear() {
			indent_level = 0;
			code.Clear();
			fresh_line = true;
		}
	}
}
