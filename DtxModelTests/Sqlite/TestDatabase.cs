using System;
using System.Data.Common;
using System.Collections.Generic;
using DtxModel;

namespace DtxModelTests.Sqlite {

	public partial class TestDatabaseContext : Context {
		private static Func<DbConnection> _DefaultConnection = null;

		/// <summary>
		/// Set a default constructor to allow use of parameterless context calling.
		/// </summary>
		public static Func<DbConnection> DefaultConnection {
			get { return _DefaultConnection; }
			set { _DefaultConnection = value; }
		}

		private static string _LastInsertIdQuery = null;

		/// <summary>
		/// Sets the querty string to retrieve the last insert ID.
		/// </summary>
		public static new string LastInsertIdQuery {
			get { return _LastInsertIdQuery; }
			set { _LastInsertIdQuery = value; }
		}

		private static TargetDb _DatabaseType;

		/// <summary>
		/// Type of database this context will target.  Automatically sets proper database specific values.
		/// </summary>
		public static TargetDb DatabaseType {
			get { return _DatabaseType; }
			set {
				_DatabaseType = value;
				switch (value) {
					case TargetDb.MySql:
						LastInsertIdQuery = "SELECT last_insert_id()";
						break;
					case TargetDb.Sqlite:
						LastInsertIdQuery = "SELECT last_insert_rowid()";
						break;
					case TargetDb.Other:
						break;
				}
			}
		}
		private Table<Users> _Users;

		public Table<Users> Users {
			get {
				if(_Users == null) {
					_Users = new Table<Users>(this);
				}

				return _Users;
			}
		}

		private Table<Logs> _Logs;

		public Table<Logs> Logs {
			get {
				if(_Logs == null) {
					_Logs = new Table<Logs>(this);
				}

				return _Logs;
			}
		}

		/// <summary>
		/// Create a new context of this database's type.  Can only be used if a default connection is specified.
		/// </summary>
		public TestDatabaseContext() : base(_DefaultConnection, _LastInsertIdQuery) { }

		/// <summary>
		/// Create a new context of this database's type with a specific connection.
		/// </summary>
		/// <param name="connection">Existing open database connection to use.</param>
		public TestDatabaseContext(DbConnection connection) : base(connection, _LastInsertIdQuery) { }
	}

	[TableAttribute(Name = "Users")]
	public partial class Users : Model {
		private bool _rowidChanged = false;
		private Int64 _rowid;
		public Int64 rowid {
			get { return _rowid; }
			set {
				_rowid = value;
				_rowidChanged = true;
			}
		}

		private bool _usernameChanged = false;
		private String _username;
		public String username {
			get { return _username; }
			set {
				_username = value;
				_usernameChanged = true;
			}
		}

		private bool _passwordChanged = false;
		private String _password;
		public String password {
			get { return _password; }
			set {
				_password = value;
				_passwordChanged = true;
			}
		}

		private bool _last_loggedChanged = false;
		private Int64 _last_logged;
		public Int64 last_logged {
			get { return _last_logged; }
			set {
				_last_logged = value;
				_last_loggedChanged = true;
			}
		}

		public Users() : this(null, null) { }

		public Users(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "rowid": _rowid = reader.GetInt64(i); break;
					case "username": _username = reader.GetValue(i) as String; break;
					case "password": _password = reader.GetValue(i) as String; break;
					case "last_logged": _last_logged = reader.GetInt64(i); break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_usernameChanged)
				changed.Add("username", _username);
			if (_passwordChanged)
				changed.Add("password", _password);
			if (_last_loggedChanged)
				changed.Add("last_logged", _last_logged);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_username,
				_password,
				_last_logged,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"username",
				"password",
				"last_logged",
			};
		}

		public override string GetPKName() {
			return "rowid";
		}

		public override object GetPKValue() {
			return _rowid;
		}

	}

	[TableAttribute(Name = "Logs")]
	public partial class Logs : Model {
		private bool _rowidChanged = false;
		private Int64 _rowid;
		public Int64 rowid {
			get { return _rowid; }
			set {
				_rowid = value;
				_rowidChanged = true;
			}
		}

		private bool _Users_rowidChanged = false;
		private Int64 _Users_rowid;
		/// <summary>
		/// User which created this log item.
		/// </summary>
		public Int64 Users_rowid {
			get { return _Users_rowid; }
			set {
				_Users_rowid = value;
				_Users_rowidChanged = true;
			}
		}

		private bool _textChanged = false;
		private String _text;
		public String text {
			get { return _text; }
			set {
				_text = value;
				_textChanged = true;
			}
		}

		public Logs() : this(null, null) { }

		public Logs(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "rowid": _rowid = reader.GetInt64(i); break;
					case "Users_rowid": _Users_rowid = reader.GetInt64(i); break;
					case "text": _text = reader.GetValue(i) as String; break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_Users_rowidChanged)
				changed.Add("Users_rowid", _Users_rowid);
			if (_textChanged)
				changed.Add("text", _text);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_Users_rowid,
				_text,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"Users_rowid",
				"text",
			};
		}

		public override string GetPKName() {
			return "rowid";
		}

		public override object GetPKValue() {
			return _rowid;
		}

	}
}