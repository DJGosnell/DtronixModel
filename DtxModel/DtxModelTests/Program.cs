﻿using DtxModelTests.Northwind.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtxModelTests {
    class Program {
        static void Main(string[] args) {
			insertTests();
        }

		static void insertTests() {
			var connection = new SQLiteConnection(@"Data Source=C:\Users\mepengadmin\Source\Workspaces\DtronixModel\DtxModel\DtxModelTests\bin\Debug\Northwind/northwind.sqlite;Version=3;");
				NorthwindContext.DefaultConnection = connection;

				

				var customers = new Customers[100];
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
						Region = rand.Next(600000).ToString()
					};

				}

				/*timeFunc("Startup inserts", () => {
					using (var context = new NorthwindContext(connection)) {
						context.Customers.insert(customers);
					}
				});

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
				});*/

				for (int i = 0; i < 500; i++) {
					using (var context = new NorthwindContext()) {
						context.Customers.insert(customers);
					}
				}

				/*timeFunc("Pooled Normal inserts of " + customers.Length + " records.", 2, () => {
				
				});*/

				/*
				timeFunc("Specified Connection Normal inserts of " + customers.Length + " records.", 50, () => {
					using (var context = new NorthwindContext(connection)) {
						context.Customers.insert(customers);
					}
				});
				*/

				//Console.ReadLine();
				
			


		}

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
			var sw = Stopwatch.StartNew();

			action();

			sw.Stop();
			if (text != null) {
				Console.WriteLine(" Total Time " + sw.ElapsedMilliseconds + "ms.");
			}
			return sw.ElapsedMilliseconds;
		}

            
    }



}
