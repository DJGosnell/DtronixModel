using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtronixModelTests {
	public class Performancer {

		private class LogItem {
			public long elapsed_time;
			public string message;
		}

		Stopwatch sw = new Stopwatch();
		List<LogItem> logs = new List<LogItem>();

		public Performancer() {
			sw.Start();
			Log("Performancer started logging.");
		}

		public void Log(string event_description) {
			logs.Add(new LogItem() {
				message = event_description,
				elapsed_time = sw.ElapsedMilliseconds
			});
		}

		public void OutputTraceLog() {
			sw.Stop();
			LogItem last_log = new LogItem() { elapsed_time = 0 };
			long total_time = sw.ElapsedMilliseconds;

			foreach (var log in logs) {
				long this_time = log.elapsed_time - last_log.elapsed_time;
				Trace.WriteLine(string.Format("{0,-7:N}ms ({1,5:P1}) {2}", this_time, (double)this_time / (double)total_time, log.message));
				last_log = log;
			}
			Trace.WriteLine("Performancer completed.");
		}
	}
}
