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

	}
}
