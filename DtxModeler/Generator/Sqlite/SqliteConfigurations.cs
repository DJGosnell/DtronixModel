using DtxModeler.Ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtxModeler.Generator.Sqlite {
	public class SqlitePragmaConfigurations {
		private static readonly Configuration[] configurations;

		public static Configuration[] Configurations {
			get { return configurations; }
		}

		static SqlitePragmaConfigurations() {
			var configs = new List<Configuration>();
			string[] pragmas = new string[] {"auto_vacuum", "automatic_index",  "busy_timeout", "cache_size", "cache_spill", "checkpoint_fullfsync", 
				"defer_foreign_keys", "encoding", "foreign_key_check", "foreign_keys", "full_column_names", "fullfsync", "integrity_check", 
				"journal_mode", "journal_size_limit", "legacy_file_format", "locking_mode", "max_page_count", "page_size", "query_only", "quick_check", 
				"read_uncommitted", "recursive_triggers", "reverse_unordered_selects", "schema_version", "user_version", "secure_delete", "soft_heap_limit", 
				"synchronous", "temp_store", "wal_autocheckpoint"};

			foreach (var pragma in pragmas) {
				configs.Add(new Configuration() {
					Name = "sqlite.pragma." + pragma,
					Description = "http://sqlite.org/pragma.html#pragma_" + pragma
				});
			}

			configurations = configs.ToArray();
		}
		
	}


	
}
