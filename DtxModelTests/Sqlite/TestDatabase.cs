
using System;
using System.Data.Common;
using System.Collections.Generic;
using DtxModel;

namespace DtxModelTests.Sqlite {

	public partial class TestDatabaseContext : Context {
		private static Func<DbConnection> _DefaultConnection = null;

		/// <summary>
		/// Set a default constructor to allow use of parameter-less context calling.
		/// </summary>
		public static Func<DbConnection> DefaultConnection {
			get { return _DefaultConnection; }
			set { _DefaultConnection = value; }
		}

		private static string _LastInsertIdQuery = null;

		/// <summary>
		/// Sets the query string to retrieve the last insert ID.
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
				if (_Users == null) {
					_Users = new Table<Users>(this);
				}

				return _Users;
			}
		}

		private Table<Logs> _Logs;

		public Table<Logs> Logs {
			get {
				if (_Logs == null) {
					_Logs = new Table<Logs>(this);
				}

				return _Logs;
			}
		}

		private Table<AllTypes> _AllTypes;

		public Table<AllTypes> AllTypes {
			get {
				if (_AllTypes == null) {
					_AllTypes = new Table<AllTypes>(this);
				}

				return _AllTypes;
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

	private Logs[] _Logs;
	public Logs[] Logs {
		get {
			if (_Logs == null) {
				try {
					_Logs = ((TestDatabaseContext)context).Logs.Select().WhereIn("Users_rowid", _rowid).ExecuteFetchAll();
				} catch {
					//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
					_Logs = null;
				}
			}
			return _Logs;
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
					case "username": _username = reader.GetValue(i) as string; break;
					case "password": _password = reader.GetValue(i) as string; break;
					case "last_logged": _last_logged = reader.GetInt64(i); break;
					default: additional_values.Add(reader.GetName(i), reader.GetValue(i)); break;
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

	private Users _User;
	public Users User {
		get {
			if (_User == null) {
				try {
					_User = ((TestDatabaseContext)context).Users.Select().WhereIn("rowid", _Users_rowid).ExecuteFetch();
				} catch {
					//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
					_User = null;
				}
			}
			return _User;
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
					case "text": _text = reader.GetValue(i) as string; break;
					default: additional_values.Add(reader.GetName(i), reader.GetValue(i)); break;
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

	[TableAttribute(Name = "AllTypes")]
	public partial class AllTypes : Model {

		private bool _idChanged = false;
		private Int64 _id;
		public Int64 id {
			get { return _id; }
			set {
				_id = value;
				_idChanged = true;
			}
		}

		private bool _db_int16Changed = false;
		private Int16 _db_int16;
		public Int16 db_int16 {
			get { return _db_int16; }
			set {
				_db_int16 = value;
				_db_int16Changed = true;
			}
		}

		private bool _db_int32Changed = false;
		private Int32 _db_int32;
		public Int32 db_int32 {
			get { return _db_int32; }
			set {
				_db_int32 = value;
				_db_int32Changed = true;
			}
		}

		private bool _db_int64Changed = false;
		private Int64 _db_int64;
		public Int64 db_int64 {
			get { return _db_int64; }
			set {
				_db_int64 = value;
				_db_int64Changed = true;
			}
		}

		private bool _db_byte_arrayChanged = false;
		private byte[] _db_byte_array;
		public byte[] db_byte_array {
			get { return _db_byte_array; }
			set {
				_db_byte_array = value;
				_db_byte_arrayChanged = true;
			}
		}

		private bool _db_byteChanged = false;
		private Byte _db_byte;
		public Byte db_byte {
			get { return _db_byte; }
			set {
				_db_byte = value;
				_db_byteChanged = true;
			}
		}

		private bool _db_date_timeChanged = false;
		private DateTime _db_date_time;
		public DateTime db_date_time {
			get { return _db_date_time; }
			set {
				_db_date_time = value;
				_db_date_timeChanged = true;
			}
		}

		private bool _db_decimalChanged = false;
		private Decimal _db_decimal;
		public Decimal db_decimal {
			get { return _db_decimal; }
			set {
				_db_decimal = value;
				_db_decimalChanged = true;
			}
		}

		private bool _db_floatChanged = false;
		private float _db_float;
		public float db_float {
			get { return _db_float; }
			set {
				_db_float = value;
				_db_floatChanged = true;
			}
		}

		private bool _db_doubleChanged = false;
		private Double _db_double;
		public Double db_double {
			get { return _db_double; }
			set {
				_db_double = value;
				_db_doubleChanged = true;
			}
		}

		private bool _db_boolChanged = false;
		private Boolean _db_bool;
		public Boolean db_bool {
			get { return _db_bool; }
			set {
				_db_bool = value;
				_db_boolChanged = true;
			}
		}

		private bool _db_stringChanged = false;
		private String _db_string;
		public String db_string {
			get { return _db_string; }
			set {
				_db_string = value;
				_db_stringChanged = true;
			}
		}

		private bool _db_charChanged = false;
		private Char _db_char;
		public Char db_char {
			get { return _db_char; }
			set {
				_db_char = value;
				_db_charChanged = true;
			}
		}

		public AllTypes() : this(null, null) { }

		public AllTypes(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }
			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "id": _id = reader.GetInt64(i); break;
					case "db_int16": _db_int16 = reader.GetInt16(i); break;
					case "db_int32": _db_int32 = reader.GetInt32(i); break;
					case "db_int64": _db_int64 = reader.GetInt64(i); break;
					case "db_byte_array": _db_byte_array = (reader.IsDBNull(i)) ? null : reader.GetFieldValue<byte[]>(i); break;
					case "db_byte": _db_byte = reader.GetByte(i); break;
					case "db_date_time": _db_date_time = reader.GetDateTime(i); break;
					case "db_decimal": _db_decimal = reader.GetDecimal(i); break;
					case "db_float": _db_float = reader.GetFloat(i); break;
					case "db_double": _db_double = reader.GetDouble(i); break;
					case "db_bool": _db_bool = reader.GetBoolean(i); break;
					case "db_string": _db_string = reader.GetValue(i) as string; break;
					case "db_char": _db_char = reader.GetChar(i); break;
					default: additional_values.Add(reader.GetName(i), reader.GetValue(i)); break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_db_int16Changed)
				changed.Add("db_int16", _db_int16);
			if (_db_int32Changed)
				changed.Add("db_int32", _db_int32);
			if (_db_int64Changed)
				changed.Add("db_int64", _db_int64);
			if (_db_byte_arrayChanged)
				changed.Add("db_byte_array", _db_byte_array);
			if (_db_byteChanged)
				changed.Add("db_byte", _db_byte);
			if (_db_date_timeChanged)
				changed.Add("db_date_time", _db_date_time);
			if (_db_decimalChanged)
				changed.Add("db_decimal", _db_decimal);
			if (_db_floatChanged)
				changed.Add("db_float", _db_float);
			if (_db_doubleChanged)
				changed.Add("db_double", _db_double);
			if (_db_boolChanged)
				changed.Add("db_bool", _db_bool);
			if (_db_stringChanged)
				changed.Add("db_string", _db_string);
			if (_db_charChanged)
				changed.Add("db_char", _db_char);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_db_int16,
				_db_int32,
				_db_int64,
				_db_byte_array,
				_db_byte,
				_db_date_time,
				_db_decimal,
				_db_float,
				_db_double,
				_db_bool,
				_db_string,
				_db_char,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"db_int16",
				"db_int32",
				"db_int64",
				"db_byte_array",
				"db_byte",
				"db_date_time",
				"db_decimal",
				"db_float",
				"db_double",
				"db_bool",
				"db_string",
				"db_char",
			};
		}

		public override string GetPKName() {
			return "id";
		}

		public override object GetPKValue() {
			return _id;
		}
	}
}