using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DtxModelTests {
	public class Utilities {
		public static string GetFileContents(string sampleFile) {
			var asm = Assembly.GetExecutingAssembly();
			var resource = string.Format("DtxModelTests.{0}", sampleFile);
			using (var stream = asm.GetManifestResourceStream(resource)) {
				if (stream != null) {
					var reader = new StreamReader(stream);
					return reader.ReadToEnd();
				}
			}
			return string.Empty;
		}
	}
}
