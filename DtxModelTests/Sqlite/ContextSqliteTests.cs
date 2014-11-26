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

		private void CreateUser() {
			using (var context = new TestDatabaseContext()) {
				context.Users.Insert(new Users() {
					username = "djgosnell",
					password = "my_hashed_password",
					last_logged = Converters.DateTimeToUnix(new DateTime(2014, 11, 25))
				});
			}
		}

		[TestMethod]
		public void DatabaseRowIsCreated() {
			CreateUser();

			using (var context = new TestDatabaseContext()) {
				var user = context.Users.Select().ExecuteFetch();

				Assert.AreNotEqual(0, user.rowid);
				Assert.AreEqual("djgosnell", user.username);
				Assert.AreEqual("my_hashed_password", user.password);
				Assert.AreEqual(Converters.DateTimeToUnix(new DateTime(2014, 11, 25)), user.last_logged);

				context.Users.Delete(user);
				user = context.Users.Select().ExecuteFetch();

				Assert.IsNull(user, "Database is not empty");
			}
		}

		[TestMethod]
		public void DatabaseRowIsDeleted() {
			CreateUser();

			using (var context = new TestDatabaseContext()) {
				var user = context.Users.Select().ExecuteFetch();

				Assert.IsNotNull(user);

				context.Users.Delete(user);

				user = context.Users.Select().ExecuteFetch();

				Assert.IsNull(user, "User was not deleted.");

			}
		}

		[TestMethod]
		public void DatabaseRowIsUpdated() {
			CreateUser();

			using (var context = new TestDatabaseContext()) {
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
			CreateUser();
			ulong log_rowid = 0;
			using (var context = new TestDatabaseContext()) {
				var user = context.Users.Select().ExecuteFetch();

				log_rowid = context.Logs.Insert(new Logs() {
					text = "This is log item one.",
					Users_rowid = user.rowid
				});

				var log = context.Logs.Select().ExecuteFetch();

				var user_fk = log.User;

				Assert.IsNotNull(user_fk);
				Assert.AreEqual(user.rowid, user_fk.rowid);
				Assert.AreEqual(user.password, user_fk.password);
				Assert.AreEqual(user.username, user_fk.username);
				Assert.AreEqual(user.last_logged, user_fk.last_logged);
			}

		}



	}
}
