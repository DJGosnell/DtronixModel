using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SQLite;
using System.IO;
using DtxModel;

namespace DtxModelTests.Sqlite {
	[TestClass]
	public class ContextSqliteTests {

		static ContextSqliteTests() {
			File.Delete("test_database.sqlite");
			TestDatabaseContext.DatabaseType = DtxModel.Context.TargetDb.Sqlite;
			TestDatabaseContext.DefaultConnection = () => new SQLiteConnection(@"Data Source=test_database.sqlite;Version=3;");

			string create_script = Utilities.GetFileContents("Sqlite.TestDatabase.sql");

			using (var context = new TestDatabaseContext()) {
				context.Query(Utilities.GetFileContents("Sqlite.TestDatabase.sql"), null);
			}
		}

		[TestMethod]
		public void DatabaseIsCreated() {
			Assert.IsTrue(File.Exists("test_database.sqlite"));
		}

		private void CreateUser(TestDatabaseContext context) {
			context.Users.Insert(new Users() {
				username = "djgosnell",
				password = "my_hashed_password",
				last_logged = Converters.DateTimeToUnix(new DateTime(2014, 11, 25))
			});
		}

		[TestMethod]
		public void DatabaseRowIsCreated() {
			

			using (var context = new TestDatabaseContext()) {
				CreateUser(context);
				var user = context.Users.Select().ExecuteFetch();

				Assert.AreNotEqual(0, user.rowid);
				Assert.AreEqual("djgosnell", user.username);
				Assert.AreEqual("my_hashed_password", user.password);
				Assert.AreEqual(Converters.DateTimeToUnix(new DateTime(2014, 11, 25)), user.last_logged);

				context.Users.Delete(user);
			}
		}

		[TestMethod]
		public void DatabaseRowIsDeleted() {
			using (var context = new TestDatabaseContext()) {
				CreateUser(context);
				var user = context.Users.Select().ExecuteFetch();

				Assert.IsNotNull(user);

				context.Users.Delete(user);

				user = context.Users.Select().ExecuteFetch();

				Assert.IsNull(user, "User was not deleted.");

			}
		}

		[TestMethod]
		public void DatabaseRowIsUpdated() {
			using (var context = new TestDatabaseContext()) {
				CreateUser(context);
				var user = context.Users.Select().ExecuteFetch();
				user.username = "MyNewUsername";
				context.Users.Update(user);

				user = context.Users.Select().ExecuteFetch();

				Assert.AreEqual<string>("MyNewUsername", user.username);

				context.Users.Delete(user);


			}
		}

		[TestMethod]
		public void DatabaseRowForeignKeyAssociationAccess() {
			ulong log_rowid = 0;
			using (var context = new TestDatabaseContext()) {
				CreateUser(context);

				var user = context.Users.Select().ExecuteFetch();

				log_rowid = context.Logs.Insert(new Logs() {
					text = "This is log item 1",
					Users_rowid = user.rowid
				});

				var log = context.Logs.Select().ExecuteFetch();

				var user_fk = log.User;

				context.Logs.Delete(log);
				context.Users.Delete(user);

				Assert.IsNotNull(user_fk);
				Assert.AreEqual(user.rowid, user_fk.rowid);
				Assert.AreEqual(user.password, user_fk.password);
				Assert.AreEqual(user.username, user_fk.username);
				Assert.AreEqual(user.last_logged, user_fk.last_logged);
			}

		}

		[TestMethod]
		public void DatabaseRowAssociationAccess() {
			var logger = new Performancer();

			using (var context = new TestDatabaseContext()) {
				logger.Log("Connected to database.");

				CreateUser(context);
				logger.Log("Created user.");
				
				var user = context.Users.Select().ExecuteFetch();

				logger.Log("Fetched user.");
				Logs[] logs = new Logs[200];
				for (int i = 0; i < logs.Length; i++) {
					logs[i] = new Logs() {
						text = "This is log item " + i.ToString(),
						Users_rowid = user.rowid
					};
				}
				context.Logs.Insert(logs);
				logger.Log("Inserted logs.");
				
				var logs_assoc = user.Logs;
				logger.Log("Accessed user/log association.");

				context.Logs.Delete(logs_assoc);
				context.Users.Delete(user);

				logger.Log("Deleted new rows.");

				Assert.IsNotNull(logs_assoc);
				Assert.AreEqual("This is log item 0", logs_assoc[0].text);
				Assert.AreEqual("This is log item 1", logs_assoc[1].text);
				logger.OutputTraceLog();

				
			}

		}

		[TestMethod]
		public void DatabaseRowBaseTypesStoreAndRetrieve() {
			var logger = new Performancer();

			using (var context = new TestDatabaseContext()) {
				logger.Log("Connected to database.");

				byte[] initial_byte_array = new byte[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 255, 254, 243, 252, 251, 250, 249, 248, 247, 246, 245 };
				var date_time = DateTime.Now;

				context.AllTypes.Insert(new AllTypes() {
					db_bool = true,
					db_byte = 157,
					db_byte_array = initial_byte_array,
					db_char = 'D',
					db_date_time = date_time,
					db_decimal = 3456789.986543M,
					db_double = 12345.54321D,
					db_float = 123.321F,
					db_int16 = 32767,
					db_int32 = 2147483647,
					db_int64 = 9223372036854775807,
					db_string = "Database String With \nNewline\nSpecial Chars: ♥♦♣♠"
				});
				logger.Log("Insertted new row into db.");

				var all_types = context.AllTypes.Select().ExecuteFetch();
				logger.Log("Retrieved new row from db.");

				Assert.AreEqual(true, all_types.db_bool);
				Assert.AreEqual(157, all_types.db_byte);

				// Test the contents of the byte array.
				Assert.AreEqual(initial_byte_array.Length, all_types.db_byte_array.Length);
				for (int i = 0; i < initial_byte_array.Length; i++) {
					Assert.AreEqual(initial_byte_array[i], all_types.db_byte_array[i]);
				}

				Assert.AreEqual('D', all_types.db_char);
				Assert.AreEqual(date_time, all_types.db_date_time);
				Assert.AreEqual(3456789.986543M, all_types.db_decimal);
				Assert.AreEqual(12345.54321D, all_types.db_double);
				Assert.AreEqual(123.321F, all_types.db_float);
				Assert.AreEqual(32767, all_types.db_int16);
				Assert.AreEqual(2147483647, all_types.db_int32);
				Assert.AreEqual(9223372036854775807, all_types.db_int64);
				Assert.AreEqual("Database String With \nNewline\nSpecial Chars: ♥♦♣♠", all_types.db_string);
				logger.Log("Completed tests.");

				context.AllTypes.Delete(all_types);
				logger.Log("Deleted row.");

				logger.OutputTraceLog();
			}
		}

	}
}
