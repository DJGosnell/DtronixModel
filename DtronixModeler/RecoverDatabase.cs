using DtronixModeler.Ddl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DtronixModeler {
	public class RecoverDatabase {

		StringBuilder log = new StringBuilder();

		public string Log {
			get { return log.ToString(); }
		}

		public bool Success { get; private set; }
		XmlDocument document = new XmlDocument();

		public RecoverDatabase(string db_file) {
			Success = true;
            log.AppendLine("Starting recovery");

			try {
				document.Load(db_file);
			} catch (Exception e) {
				log.AppendLine("Xml document corrupted. Unable to parse document. Details:\r\n" + e.Message);
				Success = false;
            }

			log.AppendLine("Started reconverting tables.");
			FixTables();
			log.AppendLine("Recovered tables.");
			log.AppendLine("Started reconverting columns.");
			FixColumns();
			log.AppendLine("Recovered columns.");
		}

		public Database Recover() {
			if(Success == false) {
				return null;
			}

			Database database = null;
			Exception exception = null;

			log.AppendLine("Reparsing database.");

			if (Database.Deserialize(document.OuterXml, out database, out exception) == false) {
				LogError("Unable to parse recovered database. Details:\r\n " + exception.Message);
			}

			if (Success) {
				log.AppendLine("\r\nRECOVER SUCCESSFUL.");
				return database;
			} else {
				log.AppendLine("\r\nRECOVER FAILED.");
				return null;
			}
		}

		

		private void FixColumns() {
			var nodes = document.SelectNodes("/Database/Table/Column");

			for (int i = 0; i < nodes.Count; i++) {
				if (nodes[i].Attributes["NetType"] == null) {
					LogError("Column " + nodes[i].Attributes["Name"].Value + " does not contain a NetType.");

				} else if (nodes[i].Attributes["NetType"].Value == "DateTime") {
					LogFix("Column " + nodes[i].Attributes["Name"].Value + " Contained an obsolete type 'DateTime'. Converted this type to DateTimeOffset.");
					nodes[i].Attributes["NetType"].Value = "DateTimeOffset";
                }

			}
		}

		private void FixTables() {
			var nodes = document.SelectNodes("/Database/Table");

			for (int i = 0; i < nodes.Count; i++) {
				if(nodes[i].Attributes["Name"] == null) {
					LogError("Table does not contain a name.");
                }
			}
		}

		private void LogError(string error) {
			log.AppendLine("Error: " + error);
			Success = false;
		}

		private void LogFix(string fix) {
			log.AppendLine("Fix: " + fix);
		}
	}
}
