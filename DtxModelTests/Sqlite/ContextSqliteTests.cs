using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SQLite;
using System.IO;
using DtxModel;
using System.Reflection;

namespace DtxModelTests.Sqlite {
	[TestClass]
	public class ContextSqliteTests {
		static ContextSqliteTests() {
			TestDatabaseContext.DatabaseType = DtxModel.Context.TargetDb.Sqlite;
			Directory.CreateDirectory("sqlite_dbs");
		}

		private TestDatabaseContext CreateContext(string method_name) {
			
			string class_name = this.GetType().Name;
			string database_name = class_name + "_" + method_name + ".sqlite";
			string path = Path.Combine(Directory.GetCurrentDirectory(), "sqlite_dbs", database_name);
			File.Delete(path);

			var connection = new SQLiteConnection("Data Source=\"" + path + "\";Version=3;");
			connection.Open();

			var context = new TestDatabaseContext(connection);
			context.Query(Utilities.GetFileContents("Sqlite.TestDatabase.sql"), null);

			return context;



		}

		private ulong CreateUser(TestDatabaseContext context, string append = null) {
			return context.Users.Insert(new Users() {
				username = "user_name" + append,
				password = "my_hashed_password" + append,
				last_logged = Converters.DateTimeToUnix(new DateTime(2014, 11, 25))
			});
		}

		[TestMethod]
		public void SelectedRow() {
			using (var context = CreateContext(MethodBase.GetCurrentMethod().Name)) {
				CreateUser(context);
				var user = context.Users.Select().ExecuteFetch();

				Assert.IsNotNull(user);
			}
		}

		[TestMethod]
		public void SelectedRows() {
			using (var context = CreateContext(MethodBase.GetCurrentMethod().Name)) {
				CreateUser(context);
				CreateUser(context);
				var users = context.Users.Select().ExecuteFetchAll();

				Assert.AreEqual(2, users.Length);
			}
		}

		[TestMethod]
		public void SelectedSpecifiedRows() {
			using (var context = CreateContext(MethodBase.GetCurrentMethod().Name)) {
				CreateUser(context);
				var user = context.Users.Select("username, last_logged").ExecuteFetch();

				Assert.AreEqual("user_name", user.username);
				Assert.AreEqual(new DateTime(2014, 11, 25), Converters.UnixToDateTime(user.last_logged));
				Assert.IsNull(user.password);
			}
		}

		[TestMethod]
		public void SelectedLimitCount() {
			using (var context = CreateContext(MethodBase.GetCurrentMethod().Name)) {
				CreateUser(context, "1");
				CreateUser(context, "2");
				CreateUser(context, "3");
				var users = context.Users.Select().Limit(2).ExecuteFetchAll();

				Assert.AreEqual(2, users.Length);
				Assert.AreEqual("user_name1", users[0].username);
				Assert.AreEqual("user_name2", users[1].username);
			}
		}


		[TestMethod]
		public void SelectedLimitCountStart() {
			using (var context = CreateContext(MethodBase.GetCurrentMethod().Name)) {
				CreateUser(context, "1");
				CreateUser(context, "2");
				CreateUser(context, "3");
				CreateUser(context, "4");
				var users = context.Users.Select().Limit(2, 1).ExecuteFetchAll();

				Assert.AreEqual(2, users.Length);
				Assert.AreEqual("user_name2", users[0].username);
				Assert.AreEqual("user_name3", users[1].username);
			}
		}

		[TestMethod]
		public void SelectedOrderByDescending() {
			using (var context = CreateContext(MethodBase.GetCurrentMethod().Name)) {
				CreateUser(context, "1");
				CreateUser(context, "2");
				CreateUser(context, "3");
				CreateUser(context, "4");
				var users = context.Users.Select().OrderBy("username", SortDirection.Descending).ExecuteFetchAll();

				Assert.AreEqual(4, users.Length);
				Assert.AreEqual("user_name4", users[0].username);
				Assert.AreEqual("user_name3", users[1].username);
				Assert.AreEqual("user_name2", users[2].username);
				Assert.AreEqual("user_name1", users[3].username);
			}
		}

		[TestMethod]
		public void SelectedWhereModel() {
			using (var context = CreateContext(MethodBase.GetCurrentMethod().Name)) {
				CreateUser(context, "1");
				CreateUser(context, "2");

				var user_where = context.Users.Select().ExecuteFetch();

				var users = context.Users.Select().Where(user_where).ExecuteFetchAll();

				Assert.AreEqual(1, users.Length);
				Assert.AreEqual("user_name1", users[0].username);
			}
		}

		[TestMethod]
		public void SelectedWhereModels() {
			using (var context = CreateContext(MethodBase.GetCurrentMethod().Name)) {
				CreateUser(context, "1");
				CreateUser(context, "2");
				CreateUser(context, "3");

				var users_where = context.Users.Select().Limit(2).ExecuteFetchAll();

				var users = context.Users.Select().Where(users_where).ExecuteFetchAll();

				Assert.AreEqual(2, users.Length);
				Assert.AreEqual("user_name1", users[0].username);
				Assert.AreEqual("user_name2", users[1].username);
			}
		}

		[TestMethod]
		public void SelectedWhereCustom() {
			using (var context = CreateContext(MethodBase.GetCurrentMethod().Name)) {
				CreateUser(context, "1");
				CreateUser(context, "2");
				CreateUser(context, "3");

				var users = context.Users.Select().Where("username = {0} AND password = {1}", "user_name1", "my_hashed_password1").ExecuteFetchAll();

				Assert.AreEqual(1, users.Length);
				Assert.AreEqual("user_name1", users[0].username);
			}
		}

		[TestMethod]
		public void SelectedWhereIn() {
			using (var context = CreateContext(MethodBase.GetCurrentMethod().Name)) {
				CreateUser(context, "1");
				CreateUser(context, "2");
				CreateUser(context, "3");

				var users = context.Users.Select().WhereIn("username", "user_name1", "user_name3").ExecuteFetchAll();

				Assert.AreEqual(2, users.Length);
				Assert.AreEqual("user_name1", users[0].username);
				Assert.AreEqual("user_name3", users[1].username);
			}
		}

		[TestMethod]
		public void QueryTables() {
			using (var context = CreateContext(MethodBase.GetCurrentMethod().Name)) {
				CreateUser(context, "1");
				CreateUser(context, "2");
				CreateUser(context, "3");

				context.QueryRead(@"SELECT * FROM Users WHERE username LIKE {0} LIMIT 1", new[] { "%name2" }, reader => {
					int count = 0;
					while (reader.Read()) {
						Assert.AreEqual(1, ++count);
						Assert.AreEqual("user_name2", reader.GetString(reader.GetOrdinal("username")));
						Assert.AreEqual("my_hashed_password2", reader.GetString(reader.GetOrdinal("password")));
					}
				});
			}
		}


		[TestMethod]
		public void SelectedOrderByAscending() {
			using (var context = CreateContext(MethodBase.GetCurrentMethod().Name)) {
				CreateUser(context, "1");
				CreateUser(context, "2");
				CreateUser(context, "3");
				CreateUser(context, "4");
				var users = context.Users.Select().OrderBy("username", SortDirection.Ascending).ExecuteFetchAll();

				Assert.AreEqual(4, users.Length);
				Assert.AreEqual("user_name1", users[0].username);
				Assert.AreEqual("user_name2", users[1].username);
				Assert.AreEqual("user_name3", users[2].username);
				Assert.AreEqual("user_name4", users[3].username);
			}
		}

		[TestMethod]
		public void SelectEmptyTable() {
			using (var context = CreateContext(MethodBase.GetCurrentMethod().Name)) {
				var user = context.Users.Select().ExecuteFetch();
				var users = context.Users.Select().ExecuteFetchAll();

				Assert.IsNull(user);
				Assert.AreEqual(0, users.Length);
			}
		}

		[TestMethod]
		public void RowIsCreated() {
			using (var context = CreateContext(MethodBase.GetCurrentMethod().Name)) {
				CreateUser(context);
				var user = context.Users.Select().ExecuteFetch();

				Assert.AreNotEqual(0, user.rowid);
				Assert.AreEqual("user_name", user.username);
				Assert.AreEqual("my_hashed_password", user.password);
				Assert.AreEqual(Converters.DateTimeToUnix(new DateTime(2014, 11, 25)), user.last_logged);
			}
		}

		[TestMethod]
		public void RowIsDeletedByModel() {
			using (var context = CreateContext(MethodBase.GetCurrentMethod().Name)) {
				CreateUser(context);
				var user = context.Users.Select().ExecuteFetch();

				Assert.IsNotNull(user);

				context.Users.Delete(user);

				user = context.Users.Select().ExecuteFetch();

				Assert.IsNull(user, "User was not deleted.");
			}
		}

		[TestMethod]
		public void RowIsDeletedByModels() {
			using (var context = CreateContext(MethodBase.GetCurrentMethod().Name)) {
				CreateUser(context);
				CreateUser(context);
				var users = context.Users.Select().ExecuteFetchAll();

				Assert.IsNotNull(users);
				Assert.AreEqual(2, users.Length);

				context.Users.Delete(users);

				var user = context.Users.Select().ExecuteFetch();

				Assert.IsNull(user, "Users were not deleted.");

			}
		}

		[TestMethod]
		public void RowIsDeletedByRowId() {
			using (var context = CreateContext(MethodBase.GetCurrentMethod().Name)) {
				ulong id = CreateUser(context);

				Assert.AreNotEqual(0, id);

				context.Users.Delete(id);

				var user = context.Users.Select().ExecuteFetch();

				Assert.IsNull(user, "User was not deleted.");

			}
		}

		[TestMethod]
		public void RowIsDeletedByRowIds() {
			using (var context = CreateContext(MethodBase.GetCurrentMethod().Name)) {
				ulong[] ids = new ulong[2];
				ids[0] = CreateUser(context);
				ids[1] = CreateUser(context);

				Assert.AreNotEqual(0, ids[0]);
				Assert.AreNotEqual(0, ids[1]);

				context.Users.Delete(ids);

				var user = context.Users.Select().ExecuteFetch();

				Assert.IsNull(user, "Users were not deleted.");

			}
		}


		[TestMethod]
		public void RowIsUpdated() {
			using (var context = CreateContext(MethodBase.GetCurrentMethod().Name)) {
				CreateUser(context);
				var user = context.Users.Select().ExecuteFetch();
				user.username = "MyNewUsername";
				context.Users.Update(user);

				user = context.Users.Select().ExecuteFetch();

				Assert.AreEqual<string>("MyNewUsername", user.username);
			}
		}

		[TestMethod]
		public void RowsAreUpdated() {
			using (var context = CreateContext(MethodBase.GetCurrentMethod().Name)) {
				CreateUser(context);
				CreateUser(context);

				var users = context.Users.Select().ExecuteFetchAll();
				users[0].username = "MyNewUsernameFirst";
				users[1].username = "MyNewUsernameSecond";
				context.Users.Update(users);

				users = context.Users.Select().ExecuteFetchAll();

				Assert.AreEqual<string>("MyNewUsernameFirst", users[0].username);
				Assert.AreEqual<string>("MyNewUsernameSecond", users[1].username);
			}
		}

		[TestMethod]
		public void RowForeignKeyAssociationAccess() {
			ulong log_rowid = 0;
			using (var context = CreateContext(MethodBase.GetCurrentMethod().Name)) {
				CreateUser(context);

				var user = context.Users.Select().ExecuteFetch();

				log_rowid = context.Logs.Insert(new Logs() {
					text = "This is log item 1",
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

		[TestMethod]
		public void RowAssociationAccess() {
			var logger = new Performancer();

			using (var context = CreateContext(MethodBase.GetCurrentMethod().Name)) {
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

				Assert.IsNotNull(logs_assoc);
				Assert.AreEqual("This is log item 0", logs_assoc[0].text);
				Assert.AreEqual("This is log item 1", logs_assoc[1].text);
				logger.OutputTraceLog();
			}

		}

		[TestMethod]
		public void RowBaseTypesStoreAndRetrieve() {
			var logger = new Performancer();

			using (var context = CreateContext(MethodBase.GetCurrentMethod().Name)) {
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

				logger.OutputTraceLog();
			}
		}

		[TestMethod]
		public void TransactionRollbackAuto() {
			using (var context = CreateContext(MethodBase.GetCurrentMethod().Name)) {
				using (var transaction = context.BeginTransaction()) {
					CreateUser(context);
				}

				var user = context.Users.Select().ExecuteFetch();

				Assert.IsNull(user);

			}
		}

		[TestMethod]
		public void TransactionRollbackManual() {
			using (var context = CreateContext(MethodBase.GetCurrentMethod().Name)) {
				using (var transaction = context.BeginTransaction()) {
					CreateUser(context);
					transaction.Rollback();
				}

				var user = context.Users.Select().ExecuteFetch();

				Assert.IsNull(user);

			}
		}

		[TestMethod]
		public void TransactionCommit() {
			using (var context = CreateContext(MethodBase.GetCurrentMethod().Name)) {
				using (var transaction = context.BeginTransaction()) {
					CreateUser(context);
					transaction.Commit();
				}

				var user = context.Users.Select().ExecuteFetch();

				Assert.IsNotNull(user);

				context.Users.Delete(user);

			}
		}

		[TestMethod]
		public void TransactionCommitMultipleInserts() {
			using (var context = CreateContext(MethodBase.GetCurrentMethod().Name)) {
				using (var transaction = context.BeginTransaction()) {
					CreateUser(context);
					CreateUser(context);
					CreateUser(context);
					CreateUser(context);
					CreateUser(context);

					transaction.Commit();
				}

				var users = context.Users.Select().ExecuteFetchAll();

				Assert.AreEqual(5, users.Length);
			}
		}

	}
}
