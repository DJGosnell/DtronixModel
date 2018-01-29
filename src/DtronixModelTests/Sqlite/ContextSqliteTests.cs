using System;
using System.Data.SQLite;
using System.IO;
using DtronixModel;
using System.Reflection;
using Xunit;

namespace DtronixModelTests.Sqlite
{
    public class ContextSqliteTests
    {
        static ContextSqliteTests()
        {
            TestDatabaseContext.DatabaseType = DtronixModel.Context.TargetDb.Sqlite;
        }

        private TestDatabaseContext CreateContext(string method_name)
        {
            var connection = new SQLiteConnection("Data Source=:memory:;Version=3;");
            connection.Open();

            var context = new TestDatabaseContext(connection);
            context.Query(Utilities.GetFileContents("Sqlite.TestDatabase.sql"), null);

            return context;
        }

        private long CreateUser(TestDatabaseContext context, string append = null)
        {
            return context.Users.Insert(new Users
            {
                username = "user_name" + append,
                password = "my_hashed_password" + append,
                last_logged = Converters.ToUnixTimeSeconds(new DateTimeOffset(2014, 11, 25, 0, 0, 0, TimeSpan.Zero))
            });
        }

        [Fact]
        public void SelectedRow()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context);
                var user = context.Users.Select().ExecuteFetch();
                Assert.NotNull(user);
            }
        }

        [Fact]
        public void SelectedRows()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context);
                CreateUser(context);
                var users = context.Users.Select().ExecuteFetchAll();

                Assert.Equal(2, users.Length);
            }
        }

        [Fact]
        public void SelectedSpecifiedRows()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context);
                var user = context.Users.Select("username, last_logged").ExecuteFetch();

                Assert.Equal("user_name", user.username);
                Assert.Equal(new DateTimeOffset(new DateTime(2014, 11, 25), TimeSpan.Zero),
                    Converters.FromUnixTimeSeconds(user.last_logged));
                Assert.Null(user.password);
            }
        }

        [Fact]
        public void SelectedLimitCount()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context, "1");
                CreateUser(context, "2");
                CreateUser(context, "3");
                var users = context.Users.Select().Limit(2).ExecuteFetchAll();

                Assert.Equal(2, users.Length);
                Assert.Equal("user_name1", users[0].username);
                Assert.Equal("user_name2", users[1].username);
            }
        }


        [Fact]
        public void SelectedLimitCountStart()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context, "1");
                CreateUser(context, "2");
                CreateUser(context, "3");
                CreateUser(context, "4");
                var users = context.Users.Select().Limit(2, 1).ExecuteFetchAll();

                Assert.Equal(2, users.Length);
                Assert.Equal("user_name2", users[0].username);
                Assert.Equal("user_name3", users[1].username);
            }
        }

        [Fact]
        public void SelectedOrderByDescending()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context, "1");
                CreateUser(context, "2");
                CreateUser(context, "3");
                CreateUser(context, "4");
                var users = context.Users.Select().OrderBy("username", SortDirection.Descending).ExecuteFetchAll();

                Assert.Equal(4, users.Length);
                Assert.Equal("user_name4", users[0].username);
                Assert.Equal("user_name3", users[1].username);
                Assert.Equal("user_name2", users[2].username);
                Assert.Equal("user_name1", users[3].username);
            }
        }

        [Fact]
        public void SelectedWhereModel()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context, "1");
                CreateUser(context, "2");

                var user_where = context.Users.Select().ExecuteFetch();

                var users = context.Users.Select().Where(user_where).ExecuteFetchAll();

                Assert.Equal(1, users.Length);
                Assert.Equal("user_name1", users[0].username);
            }
        }

        [Fact]
        public void SelectedWhereModels()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context, "1");
                CreateUser(context, "2");
                CreateUser(context, "3");

                var users_where = context.Users.Select().Limit(2).ExecuteFetchAll();

                var users = context.Users.Select().Where(users_where).ExecuteFetchAll();

                Assert.Equal(2, users.Length);
                Assert.Equal("user_name1", users[0].username);
                Assert.Equal("user_name2", users[1].username);
            }
        }

        [Fact]
        public void SelectedWhereCustom()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context, "1");
                CreateUser(context, "2");
                CreateUser(context, "3");

                var users = context.Users.Select().Where("username = {0} AND password = {1}", "user_name1",
                    "my_hashed_password1").ExecuteFetchAll();

                Assert.Equal(1, users.Length);
                Assert.Equal("user_name1", users[0].username);
            }
        }

        [Fact]
        public void SelectedWhereIn()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context, "1");
                CreateUser(context, "2");
                CreateUser(context, "3");

                var users = context.Users.Select().WhereIn("username", new object[]{"user_name1", "user_name3"}).ExecuteFetchAll();

                Assert.Equal(2, users.Length);
                Assert.Equal("user_name1", users[0].username);
                Assert.Equal("user_name3", users[1].username);
            }
        }

        [Fact]
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
                        Assert.Equal(1, ++count);
                        Assert.Equal("user_name2", reader.GetString(reader.GetOrdinal("username")));
                        Assert.Equal("my_hashed_password2", reader.GetString(reader.GetOrdinal("password")));
                    }
                });
            }
        }


        [Fact]
        public void SelectedOrderByAscending()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context, "1");
                CreateUser(context, "2");
                CreateUser(context, "3");
                CreateUser(context, "4");
                var users = context.Users.Select().OrderBy("username", SortDirection.Ascending).ExecuteFetchAll();

                Assert.Equal(4, users.Length);
                Assert.Equal("user_name1", users[0].username);
                Assert.Equal("user_name2", users[1].username);
                Assert.Equal("user_name3", users[2].username);
                Assert.Equal("user_name4", users[3].username);
            }
        }

        [Fact]
        public void SelectEmptyTable()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                var user = context.Users.Select().ExecuteFetch();
                var users = context.Users.Select().ExecuteFetchAll();

                Assert.Null(user);
                Assert.Equal(0, users.Length);
            }
        }

        [Fact]
        public void RowIsCreated()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context);
                var user = context.Users.Select().ExecuteFetch();

                Assert.NotEqual(0, user.rowid);
                Assert.Equal("user_name", user.username);
                Assert.Equal("my_hashed_password", user.password);
                Assert.Equal(Converters.ToUnixTimeSeconds(new DateTime(2014, 11, 25)), user.last_logged);
            }
        }

        [Fact]
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

        [Fact]
        public void RowIsDeletedByModels()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context);
                CreateUser(context);
                var users = context.Users.Select().ExecuteFetchAll();

                Assert.NotNull(users);
                Assert.Equal(2, users.Length);

                context.Users.Delete(users);

                var user = context.Users.Select().ExecuteFetch();

                Assert.Null(user);

            }
        }

        [Fact]
        public void RowIsDeletedByRowId()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                long id = CreateUser(context);

                Assert.NotEqual(0, id);

                context.Users.Delete(id);

                var user = context.Users.Select().ExecuteFetch();

                Assert.Null(user);

            }
        }

        [Fact]
        public void RowIsDeletedByRowIds()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                long[] ids = new long[2];
                ids[0] = CreateUser(context);
                ids[1] = CreateUser(context);

                Assert.NotEqual(0, ids[0]);
                Assert.NotEqual(0, ids[1]);

                context.Users.Delete(ids);

                var user = context.Users.Select().ExecuteFetch();

                Assert.Null(user);

            }
        }


        [Fact]
        public void RowIsUpdated()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context);
                var user = context.Users.Select().ExecuteFetch();
                user.username = "MyNewUsername";
                context.Users.Update(user);

                user = context.Users.Select().ExecuteFetch();

                Assert.Equal<string>("MyNewUsername", user.username);
            }
        }

        [Fact]
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

                Assert.Equal<string>("MyNewUsernameFirst", users[0].username);
                Assert.Equal<string>("MyNewUsernameSecond", users[1].username);
            }
        }

        [Fact]
        public void RowForeignKeyAssociationAccess()
        {
            long log_rowid = 0;
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context);

                var user = context.Users.Select().ExecuteFetch();

                log_rowid = context.Logs.Insert(new Logs
                {
                    text = "This is log item 1",
                    Users_rowid = user.rowid
                });

                var log = context.Logs.Select().ExecuteFetch();

                var user_fk = log.User;

                Assert.NotNull(user_fk);
                Assert.Equal(user.rowid, user_fk.rowid);
                Assert.Equal(user.password, user_fk.password);
                Assert.Equal(user.username, user_fk.username);
                Assert.Equal(user.last_logged, user_fk.last_logged);
            }

        }

        [Fact]
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

                var logs_assoc = user.Logs;
                logger.Log("Accessed user/log association.");

                Assert.NotNull(logs_assoc);
                Assert.Equal("This is log item 0", logs_assoc[0].text);
                Assert.Equal("This is log item 1", logs_assoc[1].text);
                logger.OutputTraceLog();
            }

        }

        [Fact]
        public void RowBaseTypesStoreAndRetrieve()
        {
            var logger = new Performancer();

            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                logger.Log("Connected to database.");

                byte[] initial_byte_array = new byte[]
                    {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 255, 254, 243, 252, 251, 250, 249, 248, 247, 246, 245};
                var date_time = DateTimeOffset.Now;
                int ch = (int) 'D';
                string t = date_time.ToString();
                context.AllTypes.Insert(new AllTypes
                {
                    db_bool = true,
                    db_byte = 157,
                    db_byte_array = initial_byte_array,
                    db_date_time = date_time,
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

                var all_types = context.AllTypes.Select().ExecuteFetch();
                logger.Log("Retrieved new row from db.");

                Assert.Equal(true, all_types.db_bool);
                Assert.Equal(157, all_types.db_byte.Value);

                // Test the contents of the byte array.
                Assert.Equal(initial_byte_array.Length, all_types.db_byte_array.Length);
                for (int i = 0; i < initial_byte_array.Length; i++)
                {
                    Assert.Equal(initial_byte_array[i], all_types.db_byte_array[i]);
                }

                Assert.Equal(date_time, all_types.db_date_time);
                Assert.Equal(3456789.986543M, all_types.db_decimal.Value);
                Assert.Equal(12345.54321D, all_types.db_double.Value);
                Assert.Equal(123.321F, all_types.db_float.Value);
                Assert.Equal(32767, all_types.db_int16.Value);
                Assert.Equal(65535, all_types.db_uint16.Value);
                Assert.Equal(2147483647, all_types.db_int32.Value);
                Assert.Equal(4294967295, all_types.db_uint32.Value);
                Assert.Equal(9223372036854775807, all_types.db_int64.Value);
                Assert.Equal(18446744073709551615, all_types.db_uint64.Value);
                Assert.Equal(TestEnum.SecondEnumValue, all_types.db_enum);
                Assert.Equal("Database String With \nNewline\nSpecial Chars: ♥♦♣♠", all_types.db_string);
                logger.Log("Completed tests.");

                logger.OutputTraceLog();
            }
        }

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
        public void TransactionCommitMultipleInserts()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                using (var transaction = context.BeginTransaction())
                {
                    CreateUser(context);
                    CreateUser(context);
                    CreateUser(context);
                    CreateUser(context);
                    CreateUser(context);

                    transaction.Commit();
                }

                var users = context.Users.Select().ExecuteFetchAll();

                Assert.Equal(5, users.Length);
            }
        }

        [Fact]
        public void GetsOnlyModifiedChanges()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context);
                var user = context.Users.Select().ExecuteFetch();

                Assert.Equal(0, user.GetChangedValues().Count);

                user.username = "New Username";

                Assert.Equal(1, user.GetChangedValues().Count);
                Assert.Equal("New Username", user.GetChangedValues()["username"]);
            }
        }

        [Fact]
        public void ClonesAllValues()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context);
                var user = context.Users.Select().ExecuteFetch();

                var cloned = new Users(user, false);

                Assert.Equal(user.username, cloned.username);
                Assert.Equal(user.password, cloned.password);
                Assert.Equal(user.last_logged, cloned.last_logged);
            }
        }

        [Fact]
        public void ClonesOnlyChangedValues()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context);
                var user = context.Users.Select().ExecuteFetch();

                user.username = "New Username";

                var cloned = new Users(user, true);

                Assert.Equal(user.username, cloned.username);
                Assert.Equal(null, cloned.password);
                Assert.Equal(default(long), cloned.last_logged);
            }
        }

        [Fact]
        public void InsertReturnsNewRowId()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                long id = CreateUser(context);
                var user = context.Users.Select().ExecuteFetch();

                Assert.Equal(id, user.rowid);
            }
        }

        [Fact]
        public void InsertStoresDateTimeOffsetCorrectly()
        {

            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                DateTimeOffset now_dto = DateTimeOffset.Now;

                context.AllTypes.Insert(new AllTypes
                {
                    db_date_time = now_dto,
                });
                var all_types = context.AllTypes.Select().ExecuteFetch();
                Assert.Equal(now_dto, all_types.db_date_time);
            }
        }

        [Fact]
        public void InsertStoresDateTimeCorrectly()
        {

            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                DateTime now_dto = DateTime.Now;

                context.AllTypes.Insert(new AllTypes
                {
                    db_date_time = now_dto,
                });

                var all_types = context.AllTypes.Select().ExecuteFetch();
                Assert.Equal(now_dto, all_types.db_date_time);
            }
        }

        [Fact]
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

        [Fact]
        public void ExecuteFetchAllSpecifiedQuery()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context);

                var users = context.Users.Select().ExecuteFetchAll("SELECT username FROM Users");

                Assert.Equal("user_name", users[0].username);
                Assert.Equal(null, users[0].password);
            }
        }

        [Fact]
        public void ExecuteFetchAllBindsSpecifiedQuery()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context);

                var users = context.Users.Select()
                    .ExecuteFetchAll("SELECT username FROM Users WHERE username = {0}", new object[] {"user_name"});

                Assert.Equal("user_name", users[0].username);
                Assert.Equal(null, users[0].password);
            }
        }

        [Fact]
        public void ExecuteFetchAllOverridesPreviousMethods()
        {
            using (var context = CreateContext(MethodBase.GetCurrentMethod().Name))
            {
                CreateUser(context);

                var users = context.Users.Select()
                    .Where("user_name = {0}", "1234")
                    .ExecuteFetchAll("SELECT username FROM Users");

                Assert.Equal("user_name", users[0].username);
                Assert.Equal(null, users[0].password);
            }
        }
    }
}
