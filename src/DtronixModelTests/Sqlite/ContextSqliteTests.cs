using System;
using System.Data.SQLite;
using System.IO;
using DtronixModel;
using System.Reflection;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DtronixModelTests.Sqlite
{
    public class ContextSqliteTests
    {
        private static string _sbSql;

        static ContextSqliteTests()
        {
            TestDatabaseContext.DatabaseType = DtronixModel.Context.TargetDb.Sqlite;
            _sbSql = File.ReadAllText("Sqlite/TestDatabase.sql");
        }

        private TestDatabaseContext CreateContext(string methodName)
        {
            var connection = new SQLiteConnection("Data Source=:memory:;Version=3;");
            connection.Open();
            var context = new TestDatabaseContext(connection);
            context.Query(_sbSql, null);

            return context;
        }

        private long CreateUser(TestDatabaseContext context, string append = null)
        {
            return context.Users.Insert(new Users
            {
                username = "user_name" + append,
                password = "my_hashed_password" + append,
                last_logged = new DateTimeOffset(new DateTime(2014, 11, 25), TimeSpan.Zero).ToUnixTimeSeconds()
            });
        }

        private async Task<long> CreateUserAsync(TestDatabaseContext context, string append = null)
        {
            return await context.Users.InsertAsync(new Users
            {
                username = "user_name" + append,
                password = "my_hashed_password" + append,
                last_logged = new DateTimeOffset(new DateTime(2014, 11, 25), TimeSpan.Zero).ToUnixTimeSeconds()
            });
        }

        [Test]
        public void SelectedRow()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context);
                var user = context.Users.Select().ExecuteFetch();
                Assert.NotNull(user);
            }
        }

        [Test]
        public async Task SelectedRowsAsync()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                await CreateUserAsync(context);
                await CreateUserAsync(context);
                var users = await context.Users.Select().ExecuteFetchAllAsync();

                Assert.AreEqual(2, users.Length);
            }
        }

        [Test]
        public void SelectedRows()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context);
                CreateUser(context);
                var users = context.Users.Select().ExecuteFetchAll();

                Assert.AreEqual(2, users.Length);
            }
        }

        [Test]
        public void SelectedSpecifiedRows()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context);
                var user = context.Users.Select("username, last_logged").ExecuteFetch();

                Assert.AreEqual("user_name", user.username);
                Assert.AreEqual(new DateTimeOffset(new DateTime(2014, 11, 25), TimeSpan.Zero),
                    DateTimeOffset.FromUnixTimeSeconds(user.last_logged));
                Assert.Null(user.password);
            }
        }

        [Test]
        public async Task SelectedSpecifiedRowsAsync()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                await CreateUserAsync(context);
                var user = await context.Users.Select("username, last_logged").ExecuteFetchAsync();

                Assert.AreEqual("user_name", user.username);
                Assert.AreEqual(new DateTimeOffset(new DateTime(2014, 11, 25), TimeSpan.Zero),
                    DateTimeOffset.FromUnixTimeSeconds(user.last_logged));
                Assert.Null(user.password);
            }
        }

        [Test]
        public void SelectedLimitCount()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context, "1");
                CreateUser(context, "2");
                CreateUser(context, "3");
                var users = context.Users.Select().Limit(2).ExecuteFetchAll();

                Assert.AreEqual(2, users.Length);
                Assert.AreEqual("user_name1", users[0].username);
                Assert.AreEqual("user_name2", users[1].username);
            }
        }

        [Test]
        public async Task SelectedLimitCountAsync()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                await CreateUserAsync(context, "1");
                await CreateUserAsync(context, "2");
                await CreateUserAsync(context, "3");
                var users = await context.Users.Select().Limit(2).ExecuteFetchAllAsync();

                Assert.AreEqual(2, users.Length);
                Assert.AreEqual("user_name1", users[0].username);
                Assert.AreEqual("user_name2", users[1].username);
            }
        }


        [Test]
        public void SelectedLimitCountStart()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
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

        [Test]
        public async Task SelectedLimitCountStartAsync()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                await CreateUserAsync(context, "1");
                await CreateUserAsync(context, "2");
                await CreateUserAsync(context, "3");
                await CreateUserAsync(context, "4");
                var users = await context.Users.Select().Limit(2, 1).ExecuteFetchAllAsync();

                Assert.AreEqual(2, users.Length);
                Assert.AreEqual("user_name2", users[0].username);
                Assert.AreEqual("user_name3", users[1].username);
            }
        }

        [Test]
        public void SelectedOrderByDescending()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
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

        [Test]
        public async Task SelectedOrderByDescendingAsync()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                await CreateUserAsync(context, "1");
                await CreateUserAsync(context, "2");
                await CreateUserAsync(context, "3");
                await CreateUserAsync(context, "4");
                var users = await context.Users.Select()
                    .OrderBy("username", SortDirection.Descending)
                    .ExecuteFetchAllAsync();

                Assert.AreEqual(4, users.Length);
                Assert.AreEqual("user_name4", users[0].username);
                Assert.AreEqual("user_name3", users[1].username);
                Assert.AreEqual("user_name2", users[2].username);
                Assert.AreEqual("user_name1", users[3].username);
            }
        }

        [Test]
        public void SelectedWhereModel()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context, "1");
                CreateUser(context, "2");

                var userWhere = context.Users.Select().ExecuteFetch();

                var users = context.Users.Select().Where(userWhere).ExecuteFetchAll();

                Assert.AreEqual(1, users.Length);
                Assert.AreEqual("user_name1", users[0].username);
            }
        }

        [Test]
        public async Task SelectedWhereModelAsync()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                await CreateUserAsync(context, "1");
                await CreateUserAsync(context, "2");

                var userWhere = await context.Users.Select().ExecuteFetchAsync();

                var users = await context.Users.Select().Where(userWhere).ExecuteFetchAllAsync();

                Assert.AreEqual(1, users.Length);
                Assert.AreEqual("user_name1", users[0].username);
            }
        }

        [Test]
        public void SelectedWhereModels()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context, "1");
                CreateUser(context, "2");
                CreateUser(context, "3");

                var usersWhere = context.Users.Select().Limit(2).ExecuteFetchAll();

                var users = context.Users.Select().Where(usersWhere).ExecuteFetchAll();

                Assert.AreEqual(2, users.Length);
                Assert.AreEqual("user_name1", users[0].username);
                Assert.AreEqual("user_name2", users[1].username);
            }
        }

        [Test]
        public async Task SelectedWhereModelsAsync()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                await CreateUserAsync(context, "1");
                await CreateUserAsync(context, "2");
                await CreateUserAsync(context, "3");

                var usersWhere = await context.Users.Select().Limit(2).ExecuteFetchAllAsync();

                var users = await context.Users.Select().Where(usersWhere).ExecuteFetchAllAsync();

                Assert.AreEqual(2, users.Length);
                Assert.AreEqual("user_name1", users[0].username);
                Assert.AreEqual("user_name2", users[1].username);
            }
        }

        [Test]
        public void SelectedWhereCustom()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context, "1");
                CreateUser(context, "2");
                CreateUser(context, "3");

                var users = context.Users.Select().Where("username = {0} AND password = {1}", "user_name1",
                    "my_hashed_password1").ExecuteFetchAll();

                Assert.AreEqual(1, users.Length);
                Assert.AreEqual("user_name1", users[0].username);
            }
        }

        [Test]
        public async Task SelectedWhereCustomAsync()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context, "1");
                CreateUser(context, "2");
                CreateUser(context, "3");

                var users = await context.Users.Select().Where("username = {0} AND password = {1}", "user_name1",
                    "my_hashed_password1").ExecuteFetchAllAsync();

                Assert.AreEqual(1, users.Length);
                Assert.AreEqual("user_name1", users[0].username);
            }
        }

        [Test]
        public void SelectedWhereIn()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context, "1");
                CreateUser(context, "2");
                CreateUser(context, "3");

                var users = context.Users.Select()
                    .WhereIn("username", new object[]{"user_name1", "user_name3"})
                    .ExecuteFetchAll();

                Assert.AreEqual(2, users.Length);
                Assert.AreEqual("user_name1", users[0].username);
                Assert.AreEqual("user_name3", users[1].username);
            }
        }

        [Test]
        public async Task SelectedWhereInAsync()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context, "1");
                CreateUser(context, "2");
                CreateUser(context, "3");

                var users = await context.Users.Select()
                    .WhereIn("username", new object[] { "user_name1", "user_name3" })
                    .ExecuteFetchAllAsync();

                Assert.AreEqual(2, users.Length);
                Assert.AreEqual("user_name1", users[0].username);
                Assert.AreEqual("user_name3", users[1].username);
            }
        }

        [Test]
        public void QueryTables()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context, "1");
                CreateUser(context, "2");
                CreateUser(context, "3");

                context.QueryRead(@"SELECT * FROM Users WHERE username LIKE {0} LIMIT 1", new[] {"%name2"}, reader =>
                {
                    int count = 0;
                    while (reader.Read())
                    {
                        Assert.AreEqual(1, ++count);
                        Assert.AreEqual("user_name2", reader.GetString(reader.GetOrdinal("username")));
                        Assert.AreEqual("my_hashed_password2", reader.GetString(reader.GetOrdinal("password")));
                    }
                });
            }
        }

        [Test]
        public async Task QueryTablesAsync()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                await CreateUserAsync(context, "1");
                await CreateUserAsync(context, "2");
                await CreateUserAsync(context, "3");

                await context.QueryReadAsync(@"SELECT * FROM Users WHERE username LIKE {0} LIMIT 1", new[] { "%name2" }, async (reader, ct) =>
                {
                    int count = 0;
                    while (await reader.ReadAsync())
                    {
                        Assert.AreEqual(1, ++count);
                        Assert.AreEqual("user_name2", reader.GetString(reader.GetOrdinal("username")));
                        Assert.AreEqual("my_hashed_password2", reader.GetString(reader.GetOrdinal("password")));
                    }
                });
            }
        }


        [Test]
        public void SelectedOrderByAscending()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
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

        [Test]
        public void SelectEmptyTable()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                var user = context.Users.Select().ExecuteFetch();
                var users = context.Users.Select().ExecuteFetchAll();

                Assert.Null(user);
                Assert.AreEqual(0, users.Length);
            }
        }

        [Test]
        public async Task SelectEmptyTableAsync()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                var user = await context.Users.Select().ExecuteFetchAsync();
                var users = await context.Users.Select().ExecuteFetchAllAsync();

                Assert.Null(user);
                Assert.AreEqual(0, users.Length);
            }
        }

        [Test]
        public void RowIsCreated()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context);
                var user = context.Users.Select().ExecuteFetch();

                Assert.AreNotEqual(0, user.rowid);
                Assert.AreEqual("user_name", user.username);
                Assert.AreEqual("my_hashed_password", user.password);
                Assert.AreEqual(new DateTimeOffset(new DateTime(2014, 11, 25), TimeSpan.Zero).ToUnixTimeSeconds(), user.last_logged);
            }
        }

        [Test]
        public async Task RowIsCreatedAsync()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                await CreateUserAsync(context);
                var user = await context.Users.Select().ExecuteFetchAsync();

                Assert.AreNotEqual(0, user.rowid);
                Assert.AreEqual("user_name", user.username);
                Assert.AreEqual("my_hashed_password", user.password);
                Assert.AreEqual(new DateTimeOffset(new DateTime(2014, 11, 25), TimeSpan.Zero).ToUnixTimeSeconds(), user.last_logged);
            }
        }

        [Test]
        public void RowIsDeletedByModel()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context);
                var user = context.Users.Select().ExecuteFetch();

                Assert.NotNull(user);

                context.Users.Delete(user);

                user = context.Users.Select().ExecuteFetch();

                Assert.Null(user);
            }
        }

        [Test]
        public async Task RowIsDeletedByModelAsync()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                await CreateUserAsync(context);
                var user = await context.Users.Select().ExecuteFetchAsync();

                Assert.NotNull(user);

                await context.Users.DeleteAsync(user);

                user = await context.Users.Select().ExecuteFetchAsync();

                Assert.Null(user);
            }
        }

        [Test]
        public void RowIsDeletedByModels()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context);
                CreateUser(context);
                var users = context.Users.Select().ExecuteFetchAll();

                Assert.NotNull(users);
                Assert.AreEqual(2, users.Length);

                context.Users.DeleteAsync(users);

                var user = context.Users.Select().ExecuteFetch();

                Assert.Null(user);

            }
        }

        [Test]
        public async Task RowIsDeletedByModelsAsync()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                await CreateUserAsync(context);
                await CreateUserAsync(context);
                var users = await context.Users.Select().ExecuteFetchAllAsync();

                Assert.NotNull(users);
                Assert.AreEqual(2, users.Length);

                await context.Users.DeleteAsync(users);

                var user = await context.Users.Select().ExecuteFetchAsync();

                Assert.Null(user);

            }
        }

        [Test]
        public void RowIsDeletedByRowId()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                long id = CreateUser(context);

                Assert.AreNotEqual(0, id);

                context.Users.Delete(id);

                var user = context.Users.Select().ExecuteFetch();

                Assert.Null(user);

            }
        }

        [Test]
        public async Task RowIsDeletedByRowIdAsync()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                long id = await CreateUserAsync(context);

                Assert.AreNotEqual(0, id);

                await context.Users.DeleteAsync(id);

                var user = await context.Users.Select().ExecuteFetchAsync();

                Assert.Null(user);

            }
        }

        [Test]
        public void RowIsDeletedByRowIds()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                long[] ids = new long[2];
                ids[0] = CreateUser(context);
                ids[1] = CreateUser(context);

                Assert.AreNotEqual(0, ids[0]);
                Assert.AreNotEqual(0, ids[1]);

                context.Users.Delete(ids);

                var user = context.Users.Select().ExecuteFetch();

                Assert.Null(user);

            }
        }

        [Test]
        public async Task RowIsDeletedByRowIdsAsync()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                long[] ids = new long[2];
                ids[0] = await CreateUserAsync(context);
                ids[1] = await CreateUserAsync(context);

                Assert.AreNotEqual(0, ids[0]);
                Assert.AreNotEqual(0, ids[1]);

                await context.Users.DeleteAsync(ids);

                var user = await context.Users.Select().ExecuteFetchAsync();

                Assert.Null(user);

            }
        }


        [Test]
        public void RowIsUpdated()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context);
                var user = context.Users.Select().ExecuteFetch();
                user.username = "MyNewUsername";
                context.Users.Update(user);

                user = context.Users.Select().ExecuteFetch();

                Assert.AreEqual("MyNewUsername", user.username);
            }
        }

        [Test]
        public async Task RowIsUpdatedAsync()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context);
                var user = await context.Users.Select().ExecuteFetchAsync();
                user.username = "MyNewUsername";
                await context.Users.UpdateAsync(user);

                user = await context.Users.Select().ExecuteFetchAsync();

                Assert.AreEqual("MyNewUsername", user.username);
            }
        }

        [Test]
        public void RowsAreUpdated()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context);
                CreateUser(context);

                var users = context.Users.Select().ExecuteFetchAll();
                users[0].username = "MyNewUsernameFirst";
                users[1].username = "MyNewUsernameSecond";
                context.Users.Update(users);

                users = context.Users.Select().ExecuteFetchAll();

                Assert.AreEqual("MyNewUsernameFirst", users[0].username);
                Assert.AreEqual("MyNewUsernameSecond", users[1].username);
            }
        }


        [Test]
        public async Task RowsAreUpdatedAsync()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                await CreateUserAsync(context);
                await CreateUserAsync(context);

                var users = await context.Users.Select().ExecuteFetchAllAsync();
                users[0].username = "MyNewUsernameFirst";
                users[1].username = "MyNewUsernameSecond";
                await context.Users.UpdateAsync(users);

                users = await context.Users.Select().ExecuteFetchAllAsync();

                Assert.AreEqual("MyNewUsernameFirst", users[0].username);
                Assert.AreEqual("MyNewUsernameSecond", users[1].username);
            }
        }


        [Test]
        public void RowForeignKeyAssociationAccess()
        {
            long logRowid = 0;
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context);

                var user = context.Users.Select().ExecuteFetch();

                logRowid = context.Logs.Insert(new Logs
                {
                    text = "This is log item 1",
                    Users_rowid = user.rowid
                });

                var log = context.Logs.Select().ExecuteFetch();

                var userFk = log.User;

                Assert.NotNull(userFk);
                Assert.AreEqual(user.rowid, userFk.rowid);
                Assert.AreEqual(user.password, userFk.password);
                Assert.AreEqual(user.username, userFk.username);
                Assert.AreEqual(user.last_logged, userFk.last_logged);
            }

        }

        [Test]
        public void RowAssociationAccess()
        {
            var logger = new Performancer();

            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                logger.Log("Connected to database.");

                CreateUser(context);
                logger.Log("Created user.");

                var user = context.Users.Select().ExecuteFetch();

                logger.Log("Fetched user.");
                Logs[] logs = new Logs[200];
                for (int i = 0; i < logs.Length; i++)
                {
                    logs[i] = new Logs
                    {
                        text = "This is log item " + i.ToString(),
                        Users_rowid = user.rowid
                    };
                }
                context.Logs.Insert(logs);
                logger.Log("Inserted logs.");

                var logsAssoc = user.Logs;
                logger.Log("Accessed user/log association.");

                Assert.NotNull(logsAssoc);
                Assert.AreEqual("This is log item 0", logsAssoc[0].text);
                Assert.AreEqual("This is log item 1", logsAssoc[1].text);
                logger.OutputTraceLog();
            }

        }

        [Test]
        public void RowBaseTypesStoreAndRetrieve()
        {
            var logger = new Performancer();

            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                logger.Log("Connected to database.");

                byte[] initialByteArray = new byte[]
                    {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 255, 254, 243, 252, 251, 250, 249, 248, 247, 246, 245};
                var dateTime = DateTimeOffset.Now;
                //int ch = (int) 'D';
                string t = dateTime.ToString();
                context.AllTypes.Insert(new AllTypes
                {
                    db_bool = true,
                    db_byte = 157,
                    db_sbyte = -24,
                    db_byte_array = initialByteArray,
                    db_date_time = dateTime,
                    db_decimal = 3456789.986543M,
                    db_double = 12345.54321D,
                    db_float = 123.321F,
                    db_int16 = 32767,
                    db_uint16 = 65535,
                    db_int32 = 2147483647,
                    db_uint32 = 4294967295,
                    db_int64 = 9223372036854775807,
                    db_uint64 = 18446744073709551615,
                    db_string = "Database String With \nNewline\nSpecial Chars: ♥♦♣♠",
                    db_enum = TestEnum.SecondEnumValue
                });
                logger.Log("Inserted new row into db.");

                var allTypes = context.AllTypes.Select().ExecuteFetch();
                logger.Log("Retrieved new row from db.");

                Assert.AreEqual(true, allTypes.db_bool);
                Assert.AreEqual(157, allTypes.db_byte.Value);
                Assert.AreEqual(-24, allTypes.db_sbyte);

                // Test the contents of the byte array.
                Assert.AreEqual(initialByteArray.Length, allTypes.db_byte_array.Length);
                for (int i = 0; i < initialByteArray.Length; i++)
                {
                    Assert.AreEqual(initialByteArray[i], allTypes.db_byte_array[i]);
                }

                Assert.AreEqual(dateTime, allTypes.db_date_time);
                Assert.AreEqual(3456789.986543M, allTypes.db_decimal.Value);
                Assert.AreEqual(12345.54321D, allTypes.db_double.Value);
                Assert.AreEqual(123.321F, allTypes.db_float.Value);
                Assert.AreEqual(32767, allTypes.db_int16.Value);
                Assert.AreEqual(65535, allTypes.db_uint16.Value);
                Assert.AreEqual(2147483647, allTypes.db_int32.Value);
                Assert.AreEqual(4294967295, allTypes.db_uint32.Value);
                Assert.AreEqual(9223372036854775807, allTypes.db_int64.Value);
                Assert.AreEqual(18446744073709551615, allTypes.db_uint64.Value);
                Assert.AreEqual(TestEnum.SecondEnumValue, allTypes.db_enum);
                Assert.AreEqual("Database String With \nNewline\nSpecial Chars: ♥♦♣♠", allTypes.db_string);
                logger.Log("Completed tests.");

                logger.OutputTraceLog();
            }
        }

        [Test]
        public void TransactionRollbackAuto()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                using (var transaction = context.BeginTransaction())
                {
                    CreateUser(context);
                }

                var user = context.Users.Select().ExecuteFetch();

                Assert.Null(user);

            }
        }

        [Test]
        public async Task TransactionRollbackAutoAsync()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                using (var transaction = context.BeginTransaction())
                {
                    await CreateUserAsync(context);
                }

                var user = await context.Users.Select().ExecuteFetchAsync();

                Assert.Null(user);

            }
        }

        [Test]
        public void TransactionRollbackManual()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                using (var transaction = context.BeginTransaction())
                {
                    CreateUser(context);
                    transaction.Rollback();
                }

                var user = context.Users.Select().ExecuteFetch();

                Assert.Null(user);

            }
        }

        [Test]
        public async Task TransactionRollbackManualAsync()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                using (var transaction = context.BeginTransaction())
                {
                    await CreateUserAsync(context);
                    transaction.Rollback();
                }

                var user = await context.Users.Select().ExecuteFetchAsync();

                Assert.Null(user);

            }
        }


        [Test]
        public void TransactionCommit()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                using (var transaction = context.BeginTransaction())
                {
                    CreateUser(context);
                    transaction.Commit();
                }

                var user = context.Users.Select().ExecuteFetch();

                Assert.NotNull(user);

                context.Users.Delete(user);

            }
        }

        [Test]
        public async Task TransactionCommitAsync()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                using (var transaction = context.BeginTransaction())
                {
                    await CreateUserAsync(context);
                    transaction.Commit();
                }

                var user = await context.Users.Select().ExecuteFetchAsync();

                Assert.NotNull(user);

                context.Users.Delete(user);

            }
        }

        [Test]
        public void TransactionCommitMultipleInserts()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                using (var transaction = context.BeginTransaction())
                {
                    for (int i = 0; i < 5; i++)
                        CreateUser(context);                

                    transaction.Commit();
                }

                var users = context.Users.Select().ExecuteFetchAll();

                Assert.AreEqual(5, users.Length);
            }
        }

        [Test]
        public async Task TransactionCommitMultipleInsertsAsync()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                using (var transaction = context.BeginTransaction())
                {
                    for (int i = 0; i < 5; i++)
                        await CreateUserAsync(context);

                    transaction.Commit();
                }

                var users = await context.Users.Select().ExecuteFetchAllAsync();

                Assert.AreEqual(5, users.Length);
            }
        }

        [Test]
        public void GetsOnlyModifiedChanges()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context);
                var user = context.Users.Select().ExecuteFetch();

                Assert.AreEqual(0, user.GetChangedValues().Count);

                user.username = "New Username";

                Assert.AreEqual(1, user.GetChangedValues().Count);
                Assert.AreEqual("New Username", user.GetChangedValues()["username"]);
            }
        }

        [Test]
        public void ClonesAllValues()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context);
                var user = context.Users.Select().ExecuteFetch();

                var cloned = new Users(user, false);

                Assert.AreEqual(user.username, cloned.username);
                Assert.AreEqual(user.password, cloned.password);
                Assert.AreEqual(user.last_logged, cloned.last_logged);
            }
        }

        [Test]
        public void ClonesOnlyChangedValues()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context);
                var user = context.Users.Select().ExecuteFetch();

                user.username = "New Username";

                var cloned = new Users(user, true);

                Assert.AreEqual(user.username, cloned.username);
                Assert.AreEqual(null, cloned.password);
                Assert.AreEqual(default(long), cloned.last_logged);
            }
        }

        [Test]
        public void InsertReturnsNewRowId()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                long id = CreateUser(context);
                var user = context.Users.Select().ExecuteFetch();

                Assert.AreEqual(id, user.rowid);
            }
        }

        [Test]
        public async Task InsertReturnsNewRowIdAsync()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                long id = await CreateUserAsync(context);
                var user = await context.Users.Select().ExecuteFetchAsync();

                Assert.AreEqual(id, user.rowid);
            }
        }

        [Test]
        public void InsertStoresDateTimeOffsetCorrectly()
        {

            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                DateTimeOffset nowDto = DateTimeOffset.Now;

                context.AllTypes.Insert(new AllTypes
                {
                    db_date_time = nowDto,
                });
                var allTypes = context.AllTypes.Select().ExecuteFetch();
                Assert.AreEqual(nowDto, allTypes.db_date_time);
            }
        }

        [Test]
        public void InsertStoresDateTimeCorrectly()
        {

            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                DateTimeOffset nowDto = DateTime.Now;

                context.AllTypes.Insert(new AllTypes
                {
                    db_date_time = nowDto,
                });

                var allTypes = context.AllTypes.Select().ExecuteFetch();
                Assert.AreEqual(nowDto, allTypes.db_date_time);
            }
        }

        [Test]
        public void PropertyChangeNotificationNotifiesEvent()
        {

            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                bool fired = false;
                var row = new AllTypes();

                row.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == "db_date_time")
                    {
                        fired = true;
                    }
                };

                Assert.False(fired);

                row.db_date_time = DateTime.Now;

                Assert.True(fired);
            }
        }

        [Test]
        public void ExecuteFetchAllSpecifiedQuery()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context);

                var users = context.Users.Select().ExecuteFetchAll("SELECT username FROM Users");

                Assert.AreEqual("user_name", users[0].username);
                Assert.AreEqual(null, users[0].password);
            }
        }

        [Test]
        public async Task ExecuteFetchAllSpecifiedQueryAsync()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context);

                var users = await context.Users.Select().ExecuteFetchAllAsync("SELECT username FROM Users");

                Assert.AreEqual("user_name", users[0].username);
                Assert.AreEqual(null, users[0].password);
            }
        }

        [Test]
        public void ExecuteFetchAllBindsSpecifiedQuery()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context);

                var users = context.Users.Select()
                    .ExecuteFetchAll("SELECT username FROM Users WHERE username = {0}", new object[] {"user_name"});

                Assert.AreEqual("user_name", users[0].username);
                Assert.AreEqual(null, users[0].password);
            }
        }

        [Test]
        public async Task ExecuteFetchAllBindsSpecifiedQueryAsync()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context);

                var users = await context.Users.Select()
                    .ExecuteFetchAllAsync("SELECT username FROM Users WHERE username = {0}", new object[] { "user_name" });

                Assert.AreEqual("user_name", users[0].username);
                Assert.AreEqual(null, users[0].password);
            }
        }

        [Test]
        public void ExecuteFetchAllOverridesPreviousMethods()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context);

                var users = context.Users.Select()
                    .Where("user_name = {0}", "1234")
                    .ExecuteFetchAll("SELECT username FROM Users");

                Assert.AreEqual("user_name", users[0].username);
                Assert.AreEqual(null, users[0].password);
            }
        }

        [Test]
        public async Task ExecuteFetchAllOverridesPreviousMethodsAsync()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context);

                var users = await context.Users.Select()
                    .Where("user_name = {0}", "1234")
                    .ExecuteFetchAllAsync("SELECT username FROM Users");

                Assert.AreEqual("user_name", users[0].username);
                Assert.AreEqual(null, users[0].password);
            }
        }

        [Test]
        public void TableRowIsChangeHasNoChanges()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context);
                var user = context.Users.Select().ExecuteFetch();

                Assert.AreEqual(false, user.IsChanged());
            }
        }

        [Test]
        public void TableRowIsChangeHasChanges()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context);
                var user = context.Users.Select().ExecuteFetch();

                user.username = "newUsername";

                Assert.AreEqual(true, user.IsChanged());
            }
        }

        [Test]
        public void TableRowResetsChangedFlags()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context);
                var user = context.Users.Select().ExecuteFetch();

                user.username = "newUsername";
                user.ResetChangedFlags();

                Assert.AreEqual(false, user.IsChanged());
                Assert.AreEqual(0, user.GetChangedValues().Count);
            }
        }
    }
}
