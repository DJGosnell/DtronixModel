using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DtxModelTests.Northwind;
using DtxModelTests.Tome;
using DtxModelTests.Models;

namespace DtxModelTests {
    class Program {
        static void Main(string[] args) {
			ThunderbirdTests();
        }

		static Process proc = Process.GetCurrentProcess();

		static void ThunderbirdTests() {
			ThunderbirdCalendarContext.DefaultConnection = () => {
				var connection = new SQLiteConnection(@"Data Source=C:\Users\mepengadmin\Source\Workspaces\DtronixModel\DtxModel\DtxModelTests\Thunderbird\local.sqlite;Version=3;");
				connection.Open();
				return connection;
			};

			using (var context = new ThunderbirdCalendarContext()) {

				var min_date = (new DateTime(2014, 10, 10, 0, 0, 0, DateTimeKind.Utc) - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds * 1000;
				var max_date = (new DateTime(2014, 10, 20, 0, 0, 0, DateTimeKind.Utc) - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds * 1000;



				var events = context.cal_events.Select("*,rowid")
					.Where("event_start > {0} AND event_start < {1} AND flags != 12", min_date, max_date).ExecuteFetchAll();

				foreach(var ev in events){
					Console.Write("Event: ");
					Console.Write(ev.title);
					Console.Write(" | ");
					Console.WriteLine(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(ev.event_start / 1000).ToShortTimeString());
				}

				Console.ReadLine();
			}
		}


		/*static void insertTests() {

			TomeContext.DefaultConnection = () => {
				var connection = new SQLiteConnection(@"Data Source=C:\Users\mepengadmin\Desktop\Tome.db;Version=3;");
				connection.Open();
				return connection;
			};
			
			NorthwindContext.DefaultConnection = () => {
				var connection = new SQLiteConnection(@"Data Source=Northwind/northwind.sqlite;Version=3;");
				connection.Open();
				return connection;
			};

			timeFunc("Startup select", () => {
				using (var context = new NorthwindContext()) {
					context.Customers.select().limit(1).executeFetchAll();
				}
			});

			using (var context = new TomeContext()) {
				var result = context.Manga.select().executeFetch();
				var r = result.MangaTitles;
			}


			
			
			var customers = new Customers[100000];
			var rand = new Random();

			for (int i = 0; i < customers.Length; i++) {
				customers[i] = new Customers() {
					Address = rand.Next(600000).ToString(),
					City = rand.Next(600000).ToString(),
					CompanyName = rand.Next(600000).ToString(),
					ContactName = rand.Next(600000).ToString(),
					Country = rand.Next(600000).ToString(),
					Fax = rand.Next(600000).ToString(),
					Phone = rand.Next(600000).ToString(),
					PostalCode = rand.Next(600000).ToString(),
					Region = rand.Next(600000).ToString(),
					Categories_rowid = 4
				};

			}


			
			timeFunc("Startup inserts", () => {
				using (var context = new NorthwindContext()) {
					context.Customers.insert(customers);
				}
			});*/

			/*
			timeFunc("Manual Normal Insert", 50, () => {
				using (var command = connection.CreateCommand()) {
					command.CommandText = @"INSERT INTO Customers ('CompanyName', 'ContactName', 'Address', 'City', 'Region', 'PostalCode', 'Country', 'Phone', 'Fax') VALUES (@v1, @v2, @v3, @v4, @v5, @v6, @v7, @v8, @v9)";

					for (int j = 1; j < 10; j++) {
						var param = command.CreateParameter();
						param.ParameterName = "@v" + j;
						param.Value = rand.Next(600000).ToString();
						command.Parameters.Add(param);
					}

					command.ExecuteNonQuery();

				}
			});



			timeFunc("Pooled Normal inserts of " + customers.Length + " records.", 50, () => {
				using (var context = new NorthwindContext()) {
					context.Customers.insert(customers);
				}
			});


			timeFunc("Specified Connection Normal inserts of " + customers.Length + " records.", 50, () => {
				using (var context = new NorthwindContext(connection)) {
					context.Customers.insert(customers);
				}
			});*/

			/*ThreadPool.QueueUserWorkItem((object o) => {
				timeFunc("Threaded Inserts" + customers.Length + " records.", 50, () => {
					using (var context = new NorthwindContext()) {
						context.Customers.insert(customers);
					}
				});
			});

			ThreadPool.QueueUserWorkItem((object o) => {
				timeFunc("Threaded Inserts" + customers.Length + " records.", 50, () => {
					using (var context = new NorthwindContext()) {
						context.Customers.insert(customers);
					}
				});
			});*/
			/*
			using (var context = new NorthwindContext()) {
				timeFunc("Selects", 100, () => {
					var result = context.Customers.select("rowid, *").limit(1000).executeFetch();
				});



				//context.Customers.select().orderBy("Test", DtxModel.SortDirection.)
				//context.Customers.select().where("Customers.rowid == {0} {2} {1} {3}", 25, 1561, 3626, 12512);//.where(cust => (cust.City == "Name" && cust.rowid >= 1245125) || cust.CompanyName == this_name || cust.rowid == customers[24].rowid);
				//context.Customers.insert(customers);
			}*/

			/*timeFunc("Manual Select", () => {
				using (var context = new NorthwindContext()) {
					List<object> rows = new List<object>();
					context.queryRead("SELECT *, rowid FROM Customers", null, (reader) => {
						while(reader.Read()){
							object[] values = new object[reader.FieldCount];
							reader.GetValues(values);
							rows.Add(values);
						}
					});

					//result[52]
				}
			});


			timeFunc("Manual Select", () => {
				using (var context = new NorthwindContext()) {
					List<object> rows = new List<object>();
					context.queryRead("SELECT *, rowid FROM Customers LIMIT 1000", null, (reader) => {
						while(reader.Read()){
							object[] values = new object[reader.FieldCount];
							reader.GetValues(values);
							rows.Add(values);
						}
					});

					//result[52]
				}
			});

			Customers[] custs = new Customers[] {};

			timeFunc("Selects in new contexts", () => {
				using (var context = new NorthwindContext()) {
					custs = context.Customers.select().limit(1000).executeFetchAll();
					//var test = results.Customers;
					/*var results = context.Customers.select().limit(1000, 2526).executeFetchAll();*/
					/*foreach (var result in results) {
						var test = result.Category;
					}*/

					//context.Customers.delete(results);
		/*foreach (var row in results) {
			row.Region = null;
		}
		var sw = Stopwatch.StartNew();
		context.Customers.update(results);
		sw.Stop();


		//result[52]
	}
});

foreach (var cus in custs) {
	var test = cus.Category;
}
		



			
		


Console.ReadLine();




}*/

		private static long timeFunc(string text, int repeat, Action action) {
			Console.Write(text + "... [");
			long current_time;
			long avg = 0;
			for (int i = 0; i < repeat; i++) {
				avg += current_time = timeFunc(null, action);
				Console.Write(current_time.ToString() + ",");
			}
			Console.WriteLine("]ms");
			avg /= repeat;

			Console.WriteLine("Average time of " + repeat.ToString() + " repeats " + avg.ToString() + "ms.");
			return avg;

		}

		private static long timeFunc(string text, Action action) {
			if (text != null) {
				Console.Write(text + "...");
			}
			proc.Refresh();
			long start_memory = proc.PrivateMemorySize64;

			var sw = Stopwatch.StartNew();
			action();
			sw.Stop();

			if (text != null) {
				proc.Refresh();
				Console.WriteLine(" Total Time " + sw.ElapsedMilliseconds + "ms.  Total Bytes Used: " + (proc.PrivateMemorySize64 - start_memory).ToString());
			}
			return sw.ElapsedMilliseconds;
		}

            
    }



}
