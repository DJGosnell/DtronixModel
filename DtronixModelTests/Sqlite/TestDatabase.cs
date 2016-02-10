using System;
using System.Data.Common;
using System.Collections.Generic;
using System.Collections;
using DtronixModel;

namespace DtronixModelTests.Sqlite {

	public enum TestEnum : int {
		Unset = 1 << 0,
		Enum1 = 1 << 1,
		SecondEnumValue = 1 << 2,
	}

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
	public partial class Users : Model, System.ComponentModel.INotifyPropertyChanged {

		/// <summary>
		/// Implementation for INotifyPropertyChanged.
		/// </summary>
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
		private Int64 _rowid;
		public Int64 rowid {
			get { return _rowid; }
			set {
				if(_rowid.Equals(value) == false){
					PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(rowid)));
				}
				_rowid = value;
				changed_flags.Set(0, true);
			}
		}

		private String _username;
		public String username {
			get { return _username; }
			set {
				if(_username.Equals(value) == false){
					PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(username)));
				}
				_username = value;
				changed_flags.Set(1, true);
			}
		}

		private String _password;
		public String password {
			get { return _password; }
			set {
				if(_password.Equals(value) == false){
					PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(password)));
				}
				_password = value;
				changed_flags.Set(2, true);
			}
		}

		private Int64 _last_logged;
		public Int64 last_logged {
			get { return _last_logged; }
			set {
				if(_last_logged.Equals(value) == false){
					PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(last_logged)));
				}
				_last_logged = value;
				changed_flags.Set(3, true);
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
		
		/// <summary>
		/// Clones a Users model.
		/// </summary>
		/// <param name="source">Source Users model to clone from.</param>
		/// <param name="only_changes">True to only clone the changes from the source. False to clone all the values regardless of changed or unchanged.</param>
		public Users(Users source, bool only_changes = false) { 
			_rowid = source._rowid;
			if (only_changes == false || source.changed_flags.Get(0))
				_rowid = source._rowid;
			if (only_changes == false || source.changed_flags.Get(1))
				_username = source._username;
			if (only_changes == false || source.changed_flags.Get(2))
				_password = source._password;
			if (only_changes == false || source.changed_flags.Get(3))
				_last_logged = source._last_logged;
			changed_flags = new BitArray(source.changed_flags);
		}
		
		/// <summary>
		/// Creates a empty Users model. Use this for creating a new row and inserting into the database.
		/// </summary>
		public Users() : this(null, null) { }

		/// <summary>
		/// Creates a Users model and reads the row information from the table into this model.
		/// </summary>
		/// <param name="reader">Instance of a live data reader for this model's table.</param>
		/// <param name="context">The current context of the database.</param>
		public Users(DbDataReader reader, Context context) {
			changed_flags = new BitArray(4);
			Read(reader, context);
		}

		/// <summary>
		/// Reads the row information from the table into this model.
		/// </summary>
		/// <param name="reader">Instance of a live data reader for this model's table.</param>
		/// <param name="context">The current context of the database.</param>
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
					default: 
						if(additional_values == null) {
							additional_values = new Dictionary<string, object>();
						}
						additional_values.Add(reader.GetName(i), reader.GetValue(i)); 
						break;
				}
			}
		}

		/// <summary>
		/// Gets all the instance values in the model which have been changed.
		/// </summary>
		/// <returns>Dictionary with the keys of the column names and values of the properties.</returns>
		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (changed_flags.Get(1))
				changed.Add("username", _username);
			if (changed_flags.Get(2))
				changed.Add("password", _password);
			if (changed_flags.Get(3))
				changed.Add("last_logged", _last_logged);

			return changed;
		}

		/// <summary>
		/// Return all the instance values for the entire model.
		/// </summary>
		/// <returns>An object array with all the values of this model.</returns>
		public override object[] GetAllValues() {
			return new object[] {
				_username,
				_password,
				_last_logged,
			};
		}

		/// <summary>
		/// Returns all the columns in this model.
		/// </summary>
		/// <returns>A string array with all the columns in this model.</returns>
		public override string[] GetColumns() {
			return new string[] {
				"username",
				"password",
				"last_logged",
			};
		}

		/// <summary>
		/// Gets the name of the model primary key.
		/// </summary>
		/// <returns>The name of the primary key</returns>
		public override string GetPKName() {
			return "rowid";
		}

		/// <summary>
		/// Gets the value of the primary key.
		/// </summary>
		/// <returns>The value of the primary key.</returns>
		public override object GetPKValue() {
			return _rowid;
		}
	}

	[TableAttribute(Name = "Logs")]
	public partial class Logs : Model, System.ComponentModel.INotifyPropertyChanged {

		/// <summary>
		/// Implementation for INotifyPropertyChanged.
		/// </summary>
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
		private Int64 _rowid;
		public Int64 rowid {
			get { return _rowid; }
			set {
				if(_rowid.Equals(value) == false){
					PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(rowid)));
				}
				_rowid = value;
				changed_flags.Set(0, true);
			}
		}

		private Int64 _Users_rowid;
		/// <summary>
		/// User which created this log item.
		/// </summary>
		public Int64 Users_rowid {
			get { return _Users_rowid; }
			set {
				if(_Users_rowid.Equals(value) == false){
					PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(Users_rowid)));
				}
				_Users_rowid = value;
				changed_flags.Set(1, true);
			}
		}

		private String _text;
		public String text {
			get { return _text; }
			set {
				if(_text.Equals(value) == false){
					PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(text)));
				}
				_text = value;
				changed_flags.Set(2, true);
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
		
		/// <summary>
		/// Clones a Logs model.
		/// </summary>
		/// <param name="source">Source Logs model to clone from.</param>
		/// <param name="only_changes">True to only clone the changes from the source. False to clone all the values regardless of changed or unchanged.</param>
		public Logs(Logs source, bool only_changes = false) { 
			_rowid = source._rowid;
			if (only_changes == false || source.changed_flags.Get(0))
				_rowid = source._rowid;
			if (only_changes == false || source.changed_flags.Get(1))
				_Users_rowid = source._Users_rowid;
			if (only_changes == false || source.changed_flags.Get(2))
				_text = source._text;
			changed_flags = new BitArray(source.changed_flags);
		}
		
		/// <summary>
		/// Creates a empty Logs model. Use this for creating a new row and inserting into the database.
		/// </summary>
		public Logs() : this(null, null) { }

		/// <summary>
		/// Creates a Logs model and reads the row information from the table into this model.
		/// </summary>
		/// <param name="reader">Instance of a live data reader for this model's table.</param>
		/// <param name="context">The current context of the database.</param>
		public Logs(DbDataReader reader, Context context) {
			changed_flags = new BitArray(3);
			Read(reader, context);
		}

		/// <summary>
		/// Reads the row information from the table into this model.
		/// </summary>
		/// <param name="reader">Instance of a live data reader for this model's table.</param>
		/// <param name="context">The current context of the database.</param>
		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }
			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "rowid": _rowid = reader.GetInt64(i); break;
					case "Users_rowid": _Users_rowid = reader.GetInt64(i); break;
					case "text": _text = reader.GetValue(i) as string; break;
					default: 
						if(additional_values == null) {
							additional_values = new Dictionary<string, object>();
						}
						additional_values.Add(reader.GetName(i), reader.GetValue(i)); 
						break;
				}
			}
		}

		/// <summary>
		/// Gets all the instance values in the model which have been changed.
		/// </summary>
		/// <returns>Dictionary with the keys of the column names and values of the properties.</returns>
		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (changed_flags.Get(1))
				changed.Add("Users_rowid", _Users_rowid);
			if (changed_flags.Get(2))
				changed.Add("text", _text);

			return changed;
		}

		/// <summary>
		/// Return all the instance values for the entire model.
		/// </summary>
		/// <returns>An object array with all the values of this model.</returns>
		public override object[] GetAllValues() {
			return new object[] {
				_Users_rowid,
				_text,
			};
		}

		/// <summary>
		/// Returns all the columns in this model.
		/// </summary>
		/// <returns>A string array with all the columns in this model.</returns>
		public override string[] GetColumns() {
			return new string[] {
				"Users_rowid",
				"text",
			};
		}

		/// <summary>
		/// Gets the name of the model primary key.
		/// </summary>
		/// <returns>The name of the primary key</returns>
		public override string GetPKName() {
			return "rowid";
		}

		/// <summary>
		/// Gets the value of the primary key.
		/// </summary>
		/// <returns>The value of the primary key.</returns>
		public override object GetPKValue() {
			return _rowid;
		}
	}

	[TableAttribute(Name = "AllTypes")]
	public partial class AllTypes : Model, System.ComponentModel.INotifyPropertyChanged {

		/// <summary>
		/// Implementation for INotifyPropertyChanged.
		/// </summary>
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
		private Int64 _id;
		public Int64 id {
			get { return _id; }
			set {
				if(_id.Equals(value) == false){
					PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(id)));
				}
				_id = value;
				changed_flags.Set(0, true);
			}
		}

		private Int16? _db_int16;
		public Int16? db_int16 {
			get { return _db_int16; }
			set {
				if(_db_int16.Equals(value) == false){
					PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(db_int16)));
				}
				_db_int16 = value;
				changed_flags.Set(1, true);
			}
		}

		private Int32? _db_int32;
		public Int32? db_int32 {
			get { return _db_int32; }
			set {
				if(_db_int32.Equals(value) == false){
					PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(db_int32)));
				}
				_db_int32 = value;
				changed_flags.Set(2, true);
			}
		}

		private Int64? _db_int64;
		public Int64? db_int64 {
			get { return _db_int64; }
			set {
				if(_db_int64.Equals(value) == false){
					PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(db_int64)));
				}
				_db_int64 = value;
				changed_flags.Set(3, true);
			}
		}

		private byte[] _db_byte_array;
		public byte[] db_byte_array {
			get { return _db_byte_array; }
			set {
				if(_db_byte_array.Equals(value) == false){
					PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(db_byte_array)));
				}
				_db_byte_array = value;
				changed_flags.Set(4, true);
			}
		}

		private Byte? _db_byte;
		public Byte? db_byte {
			get { return _db_byte; }
			set {
				if(_db_byte.Equals(value) == false){
					PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(db_byte)));
				}
				_db_byte = value;
				changed_flags.Set(5, true);
			}
		}

		private DateTimeOffset? _db_date_time;
		public DateTimeOffset? db_date_time {
			get { return _db_date_time; }
			set {
				if(_db_date_time.Equals(value) == false){
					PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(db_date_time)));
				}
				_db_date_time = value;
				changed_flags.Set(6, true);
			}
		}

		private Decimal? _db_decimal;
		public Decimal? db_decimal {
			get { return _db_decimal; }
			set {
				if(_db_decimal.Equals(value) == false){
					PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(db_decimal)));
				}
				_db_decimal = value;
				changed_flags.Set(7, true);
			}
		}

		private float? _db_float;
		public float? db_float {
			get { return _db_float; }
			set {
				if(_db_float.Equals(value) == false){
					PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(db_float)));
				}
				_db_float = value;
				changed_flags.Set(8, true);
			}
		}

		private Double? _db_double;
		public Double? db_double {
			get { return _db_double; }
			set {
				if(_db_double.Equals(value) == false){
					PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(db_double)));
				}
				_db_double = value;
				changed_flags.Set(9, true);
			}
		}

		private Boolean? _db_bool;
		public Boolean? db_bool {
			get { return _db_bool; }
			set {
				if(_db_bool.Equals(value) == false){
					PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(db_bool)));
				}
				_db_bool = value;
				changed_flags.Set(10, true);
			}
		}

		private String _db_string;
		public String db_string {
			get { return _db_string; }
			set {
				if(_db_string.Equals(value) == false){
					PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(db_string)));
				}
				_db_string = value;
				changed_flags.Set(11, true);
			}
		}

		private TestEnum _db_enum;
		public TestEnum db_enum {
			get { return _db_enum; }
			set {
				if(_db_enum.Equals(value) == false){
					PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(db_enum)));
				}
				_db_enum = value;
				changed_flags.Set(12, true);
			}
		}

		/// <summary>
		/// Clones a AllTypes model.
		/// </summary>
		/// <param name="source">Source AllTypes model to clone from.</param>
		/// <param name="only_changes">True to only clone the changes from the source. False to clone all the values regardless of changed or unchanged.</param>
		public AllTypes(AllTypes source, bool only_changes = false) { 
			_id = source._id;
			if (only_changes == false || source.changed_flags.Get(0))
				_id = source._id;
			if (only_changes == false || source.changed_flags.Get(1))
				_db_int16 = source._db_int16;
			if (only_changes == false || source.changed_flags.Get(2))
				_db_int32 = source._db_int32;
			if (only_changes == false || source.changed_flags.Get(3))
				_db_int64 = source._db_int64;
			if (only_changes == false || source.changed_flags.Get(4))
				_db_byte_array = source._db_byte_array;
			if (only_changes == false || source.changed_flags.Get(5))
				_db_byte = source._db_byte;
			if (only_changes == false || source.changed_flags.Get(6))
				_db_date_time = source._db_date_time;
			if (only_changes == false || source.changed_flags.Get(7))
				_db_decimal = source._db_decimal;
			if (only_changes == false || source.changed_flags.Get(8))
				_db_float = source._db_float;
			if (only_changes == false || source.changed_flags.Get(9))
				_db_double = source._db_double;
			if (only_changes == false || source.changed_flags.Get(10))
				_db_bool = source._db_bool;
			if (only_changes == false || source.changed_flags.Get(11))
				_db_string = source._db_string;
			if (only_changes == false || source.changed_flags.Get(12))
				_db_enum = source._db_enum;
			changed_flags = new BitArray(source.changed_flags);
		}
		
		/// <summary>
		/// Creates a empty AllTypes model. Use this for creating a new row and inserting into the database.
		/// </summary>
		public AllTypes() : this(null, null) { }

		/// <summary>
		/// Creates a AllTypes model and reads the row information from the table into this model.
		/// </summary>
		/// <param name="reader">Instance of a live data reader for this model's table.</param>
		/// <param name="context">The current context of the database.</param>
		public AllTypes(DbDataReader reader, Context context) {
			changed_flags = new BitArray(13);
			Read(reader, context);
		}

		/// <summary>
		/// Reads the row information from the table into this model.
		/// </summary>
		/// <param name="reader">Instance of a live data reader for this model's table.</param>
		/// <param name="context">The current context of the database.</param>
		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }
			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "id": _id = reader.GetInt64(i); break;
					case "db_int16": _db_int16 = (reader.IsDBNull(i)) ? default(Int16?) : reader.GetInt16(i); break;
					case "db_int32": _db_int32 = (reader.IsDBNull(i)) ? default(Int32?) : reader.GetInt32(i); break;
					case "db_int64": _db_int64 = (reader.IsDBNull(i)) ? default(Int64?) : reader.GetInt64(i); break;
					case "db_byte_array": _db_byte_array = (reader.IsDBNull(i)) ? null : reader.GetFieldValue<byte[]>(i); break;
					case "db_byte": _db_byte = (reader.IsDBNull(i)) ? default(Byte?) : reader.GetByte(i); break;
					case "db_date_time": _db_date_time = (reader.IsDBNull(i)) ? default(DateTimeOffset?) : reader.GetDateTime(i); break;
					case "db_decimal": _db_decimal = (reader.IsDBNull(i)) ? default(Decimal?) : reader.GetDecimal(i); break;
					case "db_float": _db_float = (reader.IsDBNull(i)) ? default(float?) : reader.GetFloat(i); break;
					case "db_double": _db_double = (reader.IsDBNull(i)) ? default(Double?) : reader.GetDouble(i); break;
					case "db_bool": _db_bool = (reader.IsDBNull(i)) ? default(Boolean?) : reader.GetBoolean(i); break;
					case "db_string": _db_string = (reader.IsDBNull(i)) ? default(String) : reader.GetString(i); break;
					case "db_enum": _db_enum = (TestEnum)reader.GetInt32(i); break;
					default: 
						if(additional_values == null) {
							additional_values = new Dictionary<string, object>();
						}
						additional_values.Add(reader.GetName(i), reader.GetValue(i)); 
						break;
				}
			}
		}

		/// <summary>
		/// Gets all the instance values in the model which have been changed.
		/// </summary>
		/// <returns>Dictionary with the keys of the column names and values of the properties.</returns>
		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (changed_flags.Get(1))
				changed.Add("db_int16", _db_int16);
			if (changed_flags.Get(2))
				changed.Add("db_int32", _db_int32);
			if (changed_flags.Get(3))
				changed.Add("db_int64", _db_int64);
			if (changed_flags.Get(4))
				changed.Add("db_byte_array", _db_byte_array);
			if (changed_flags.Get(5))
				changed.Add("db_byte", _db_byte);
			if (changed_flags.Get(6))
				changed.Add("db_date_time", _db_date_time);
			if (changed_flags.Get(7))
				changed.Add("db_decimal", _db_decimal);
			if (changed_flags.Get(8))
				changed.Add("db_float", _db_float);
			if (changed_flags.Get(9))
				changed.Add("db_double", _db_double);
			if (changed_flags.Get(10))
				changed.Add("db_bool", _db_bool);
			if (changed_flags.Get(11))
				changed.Add("db_string", _db_string);
			if (changed_flags.Get(12))
				changed.Add("db_enum", _db_enum);

			return changed;
		}

		/// <summary>
		/// Return all the instance values for the entire model.
		/// </summary>
		/// <returns>An object array with all the values of this model.</returns>
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
				_db_enum,
			};
		}

		/// <summary>
		/// Returns all the columns in this model.
		/// </summary>
		/// <returns>A string array with all the columns in this model.</returns>
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
				"db_enum",
			};
		}

		/// <summary>
		/// Gets the name of the model primary key.
		/// </summary>
		/// <returns>The name of the primary key</returns>
		public override string GetPKName() {
			return "id";
		}

		/// <summary>
		/// Gets the value of the primary key.
		/// </summary>
		/// <returns>The value of the primary key.</returns>
		public override object GetPKValue() {
			return _id;
		}
	}
}
