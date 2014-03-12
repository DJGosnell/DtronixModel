using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using DtxModelTests.Northwind.Models;

namespace DtxModelTests.Northwind {
    class SqlSelect {

        public SqlSelect() {
            var connection = new SQLiteConnection("Data Source=Northwind/northwind.sqlite;Version=3;");
            connection.Open();
			var customers = new Customers[10000];
			var rand = new Random();

			for (int i = 0; i < 10000; i++) {
				customers[i] = new Customers() {
					Address = rand.Next(600000).ToString(),
					City = rand.Next(600000).ToString(),
					CompanyName = rand.Next(600000).ToString(),
					ContactName = rand.Next(600000).ToString(),
					Country = rand.Next(600000).ToString(),
					CustomerID = i.ToString(),
					Fax = rand.Next(600000).ToString(),
					Phone = rand.Next(600000).ToString(),
					PostalCode = rand.Next(600000).ToString(),
					Region = rand.Next(600000).ToString()
				};
				
			}

			using (var context = new NorthwindContext(connection)) {
				using (var transaction = connection.BeginTransaction()) {
					foreach (var customer in customers) {
						context.Customers.insert(customer);
					}

					transaction.Commit();
				}
			}

			string test = "";

			/*
            using (var command = connection.CreateCommand()) {
                command.CommandText = "SELECT rowid, * FROM Customers";
                using (var reader = command.ExecuteReader()) {
                    var depth = reader.Depth;
					var list = new List<Customers>();
                    while (reader.Read()) {
                        list.Add(new Customers(reader, connection));
                    }
                }
            }*/

        }
    }
}
