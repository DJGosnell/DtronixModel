using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DtxModelGen {
	class CodeWriter {
		int indent_level = 0;
		bool fresh_line = true;
		StringBuilder code = new StringBuilder();

		public CodeWriter() {
		}

		public CodeWriter write(string value) {
			indent();
			code.Append(value);
			return this;
		}

		public CodeWriter write(object value) {
			indent();
			code.Append(value);
			return this;
		}

		public CodeWriter beginBlock(string value) {
			indent();
			code.Append(value);
			indent_level++;
			return this;
		}

		public CodeWriter endBlock(string value) {
			indent_level--;
			indent();
			code.Append(value);
			return this;
		}

		public CodeWriter writeLine(string value) {
			indent();
			code.AppendLine(value);
			fresh_line = true;
			return this;
		}

		public CodeWriter writeLine() {
			code.AppendLine();
			fresh_line = true;
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

		public void clear() {
			indent_level = 0;
			code.Clear();
			fresh_line = true;
		}
	}
}
