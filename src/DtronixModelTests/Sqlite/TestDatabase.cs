using System;
using System.Data.Common;
using System.Collections.Generic;
using System.Collections;
using DtronixModel;
using System.Runtime.Serialization;

namespace DtronixModelTests.Sqlite {


    public enum TestEnum : int {
        Unset = 1 << 0,
        Enum1 = 1 << 1,
        SecondEnumValue = 1 << 2,
    }

    public partial class TestDatabaseContext : Context {

        /// <summary>
        /// Set a default constructor to allow use of parameter-less context calling.
        /// </summary>
        public static Func<DbConnection> DefaultConnection { get; set; }

        /// <summary>
        /// Sets the query string to retrieve the last insert ID.
        /// </summary>
        public new static string LastInsertIdQuery { get; set; } = null;

        private static TargetDb _DatabaseType;

        /// <summary>
        /// Type of database this context will target.  Automatically sets proper database specific values.
        /// </summary>
        public static TargetDb DatabaseType
        {
            get { return _DatabaseType; }
            set 
            {
                _DatabaseType = value;
                switch (value) 
                {
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
        public TestDatabaseContext() : base(DefaultConnection, LastInsertIdQuery) { }

        /// <summary>
        /// Create a new context of this database's type with a specific connection.
        /// </summary>
        /// <param name="connection">Existing open database connection to use.</param>
        public TestDatabaseContext(DbConnection connection) : base(connection, LastInsertIdQuery) { }

        /// <summary>
        /// Sets the default connection creation method.
        /// </summary>
        /// <param name="defaultConnection">Method to be called on each context creation and return the new connection.</param>
        /// <param name="targetDb">Type of DB this is connecting to.</param>
        public override void SetDefaultConnection(Func<DbConnection> defaultConnection, TargetDb targetDb)
        {
           DefaultConnection = defaultConnection;
           DatabaseType = targetDb;
        }
    }


    [Table(Name = "Users")]
    [DataContract]
    public partial class Users : TableRow, System.ComponentModel.INotifyPropertyChanged
 {

        /// <summary>
        /// Implementation for INotifyPropertyChanged.
        /// </summary>
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Bit array which contains the flags for each table column.
        /// </summary>
        [DataMember(Order = 4)]
        public BitArray ChangedFlags { get; set; }

        /// <summary>
        /// Values which are returned but not part of this table.
        /// </summary>
        [DataMember(Order = 5)]
        public Dictionary<string, object> AdditionalValues { get; set; }

        /// <summary>
        /// Column name.
        /// </summary>
        public const string rowidColumn = "rowid";

        /// <summary>
        /// Backing field for the rowid property.
        /// </summary>
        private Int64 _rowid;

        [DataMember(Order = 0)]
        public Int64 rowid
        {
            get => _rowid;
            set
            {
                _rowid = value;
                ChangedFlags.Set(0, true);
                PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(rowid)));
            }
        }

        /// <summary>
        /// Column name.
        /// </summary>
        public const string usernameColumn = "username";

        /// <summary>
        /// Backing field for the username property.
        /// </summary>
        private String _username;

        [DataMember(Order = 1)]
        public String username
        {
            get => _username;
            set
            {
                _username = value;
                ChangedFlags.Set(1, true);
                PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(username)));
            }
        }

        /// <summary>
        /// Column name.
        /// </summary>
        public const string passwordColumn = "password";

        /// <summary>
        /// Backing field for the password property.
        /// </summary>
        private String _password;

        [DataMember(Order = 2)]
        public String password
        {
            get => _password;
            set
            {
                _password = value;
                ChangedFlags.Set(2, true);
                PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(password)));
            }
        }

        /// <summary>
        /// Column name.
        /// </summary>
        public const string last_loggedColumn = "last_logged";

        /// <summary>
        /// Backing field for the last_logged property.
        /// </summary>
        private Int64 _last_logged;

        [DataMember(Order = 3)]
        public Int64 last_logged
        {
            get => _last_logged;
            set
            {
                _last_logged = value;
                ChangedFlags.Set(3, true);
                PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(last_logged)));
            }
        }

        private Logs[] _Logs;
        [IgnoreDataMember]
        public Logs[] Logs
        {
            get 
            {
                if (_Logs != null)
                    return _Logs;
                
                try 
                {
                    _Logs = ((TestDatabaseContext)Context).Logs.Select().Where("Users_rowid = {0}", _rowid).ExecuteFetchAll();
                }
                catch 
                {
                    //Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
                    _Logs = null;
                }
                return _Logs;
            }
        }

        /// <summary>
        /// Clones a Users row.
        /// </summary>
        /// <param name="source">Source Users row to clone from.</param>
        /// <param name="onlyChanged">True to only clone the changes from the source. False to clone all the values regardless of changed or unchanged.</param>
        public Users(Users source, bool onlyChanged = false)
        { 
            _rowid = source._rowid;
            if (onlyChanged == false || source.ChangedFlags.Get(0))
                _rowid = source._rowid;
            if (onlyChanged == false || source.ChangedFlags.Get(1))
                _username = source._username;
            if (onlyChanged == false || source.ChangedFlags.Get(2))
                _password = source._password;
            if (onlyChanged == false || source.ChangedFlags.Get(3))
                _last_logged = source._last_logged;

            ChangedFlags = new BitArray(source.ChangedFlags);
        }

        /// <summary>
        /// Creates a empty Users row. Use this for creating a new row and inserting into the database.
        /// </summary>
        public Users() : this(null, null) { }

        /// <summary>
        /// Creates a Users row and reads the row information from the table into this row.
        /// </summary>
        /// <param name="reader">Instance of a live data reader for this row's table.</param>
        /// <param name="context">The current context of the database.</param>
        public Users(DbDataReader reader, Context context)
        {
            ChangedFlags = new BitArray(4);
            Read(reader, context);
        }

        /// <summary>
        /// Creates a Users row and with the specified Id.
        /// Useful when creating a new matching row on a remote connection.
        /// </summary>
        /// <param name="id">Id to set the row to.</param>
        public Users(Int64 id)
        {
            ChangedFlags = new BitArray(4);
            _rowid = id;
        }

        /// <summary>
        /// Reads the row information from the table into this row.
        /// </summary>
        /// <param name="reader">Instance of a live data reader for this row's table.</param>
        /// <param name="context">The current context of the database.</param>
        public override void Read(DbDataReader reader, Context context) {
            Context = context;
            if (reader == null)
                return;

            var length = reader.FieldCount;
            for (var i = 0; i < length; i++)
            {
                switch (reader.GetName(i))
                {
                    case "rowid":
                        _rowid = reader.GetInt64(i);
                        break;
                    case "username":
                        _username = reader.GetValue(i) as string;
                        break;
                    case "password":
                        _password = reader.GetValue(i) as string;
                        break;
                    case "last_logged":
                        _last_logged = reader.GetInt64(i);
                        break;
                    default: 
                        if(AdditionalValues == null)
                            AdditionalValues = new Dictionary<string, object>();

                        AdditionalValues.Add(reader.GetName(i), reader.GetValue(i)); 
                        break;
                }
            }
        }

        /// <summary>
        /// Gets all the instance values in the row which have been changed.
        /// </summary>
        /// <returns>Dictionary with the keys of the column names and values of the properties.</returns>
        public override Dictionary<string, object> GetChangedValues()
        {
            var changed = new Dictionary<string, object>();
            if (ChangedFlags.Get(1))
                changed.Add("username", _username);
            if (ChangedFlags.Get(2))
                changed.Add("password", _password);
            if (ChangedFlags.Get(3))
                changed.Add("last_logged", _last_logged);

            return changed;
        }

        /// <summary>
        /// Returns true if any of the values have been modified from the properties.
        /// </summary>
        /// <returns>True if the row has any modified values.</returns>
        public override bool IsChanged()
        {
            var length = ChangedFlags.Length;
            for(var i = 0; i < length; i++){
                if(ChangedFlags.Get(i))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Return all the instance values for the entire row.
        /// </summary>
        /// <returns>An object array with all the values of this row.</returns>
        public override object[] GetAllValues()
        {
            return new object[] {
                _username,
                _password,
                _last_logged,
            };
        }

        /// <summary>
        /// Returns all the columns in this row.
        /// </summary>
        /// <returns>A string array with all the columns in this row.</returns>
        public override string[] GetColumns()
        {
            return new [] {
                "username",
                "password",
                "last_logged",
            };
        }

        /// <summary>
        /// Returns all the columns types.
        /// </summary>
        /// <returns>A type array with all the columns in this row.</returns>
        public override Type[] GetColumnTypes()
        {
            return new [] {
                typeof(String),
                typeof(String),
                typeof(Int64),
            };
        }

        /// <summary>
        /// Gets the name of the row primary key.
        /// </summary>
        /// <returns>The name of the primary key</returns>
        public override string GetPKName()
        {
            return "rowid"; 
        }

        /// <summary>
        /// Gets the value of the primary key.
        /// </summary>
        /// <returns>The value of the primary key.</returns>
        public override object GetPKValue()
        {
            return rowid; 
        }
    }

    [Table(Name = "Logs")]
    [DataContract]
    public partial class Logs : TableRow, System.ComponentModel.INotifyPropertyChanged
 {

        /// <summary>
        /// Implementation for INotifyPropertyChanged.
        /// </summary>
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Bit array which contains the flags for each table column.
        /// </summary>
        [DataMember(Order = 3)]
        public BitArray ChangedFlags { get; set; }

        /// <summary>
        /// Values which are returned but not part of this table.
        /// </summary>
        [DataMember(Order = 4)]
        public Dictionary<string, object> AdditionalValues { get; set; }

        /// <summary>
        /// Column name.
        /// </summary>
        public const string rowidColumn = "rowid";

        /// <summary>
        /// Backing field for the rowid property.
        /// </summary>
        private Int64 _rowid;

        [DataMember(Order = 0)]
        public Int64 rowid
        {
            get => _rowid;
            set
            {
                _rowid = value;
                ChangedFlags.Set(0, true);
                PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(rowid)));
            }
        }

        /// <summary>
        /// Column name.
        /// User which created this log item.
        /// </summary>
        public const string Users_rowidColumn = "Users_rowid";

        /// <summary>
        /// Backing field for the Users_rowid property.
        /// </summary>
        private Int64 _Users_rowid;

        /// <summary>
        /// User which created this log item.
        /// </summary>
        [DataMember(Order = 1)]
        public Int64 Users_rowid
        {
            get => _Users_rowid;
            set
            {
                _Users_rowid = value;
                ChangedFlags.Set(1, true);
                PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(Users_rowid)));
            }
        }

        /// <summary>
        /// Column name.
        /// </summary>
        public const string textColumn = "text";

        /// <summary>
        /// Backing field for the text property.
        /// </summary>
        private String _text;

        [DataMember(Order = 2)]
        public String text
        {
            get => _text;
            set
            {
                _text = value;
                ChangedFlags.Set(2, true);
                PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(text)));
            }
        }

        private Users _User;
        [IgnoreDataMember]
        public Users User
        {
            get 
            {
                if (_User != null)
                    return _User;
                
                try 
                {
                    _User = ((TestDatabaseContext)Context).Users.Select().Where("rowid = {0}", _Users_rowid).ExecuteFetch();
                }
                catch 
                {
                    //Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
                    _User = null;
                }
                return _User;
            }
        }

        /// <summary>
        /// Clones a Logs row.
        /// </summary>
        /// <param name="source">Source Logs row to clone from.</param>
        /// <param name="onlyChanged">True to only clone the changes from the source. False to clone all the values regardless of changed or unchanged.</param>
        public Logs(Logs source, bool onlyChanged = false)
        { 
            _rowid = source._rowid;
            if (onlyChanged == false || source.ChangedFlags.Get(0))
                _rowid = source._rowid;
            if (onlyChanged == false || source.ChangedFlags.Get(1))
                _Users_rowid = source._Users_rowid;
            if (onlyChanged == false || source.ChangedFlags.Get(2))
                _text = source._text;

            ChangedFlags = new BitArray(source.ChangedFlags);
        }

        /// <summary>
        /// Creates a empty Logs row. Use this for creating a new row and inserting into the database.
        /// </summary>
        public Logs() : this(null, null) { }

        /// <summary>
        /// Creates a Logs row and reads the row information from the table into this row.
        /// </summary>
        /// <param name="reader">Instance of a live data reader for this row's table.</param>
        /// <param name="context">The current context of the database.</param>
        public Logs(DbDataReader reader, Context context)
        {
            ChangedFlags = new BitArray(3);
            Read(reader, context);
        }

        /// <summary>
        /// Creates a Logs row and with the specified Id.
        /// Useful when creating a new matching row on a remote connection.
        /// </summary>
        /// <param name="id">Id to set the row to.</param>
        public Logs(Int64 id)
        {
            ChangedFlags = new BitArray(3);
            _rowid = id;
        }

        /// <summary>
        /// Reads the row information from the table into this row.
        /// </summary>
        /// <param name="reader">Instance of a live data reader for this row's table.</param>
        /// <param name="context">The current context of the database.</param>
        public override void Read(DbDataReader reader, Context context) {
            Context = context;
            if (reader == null)
                return;

            var length = reader.FieldCount;
            for (var i = 0; i < length; i++)
            {
                switch (reader.GetName(i))
                {
                    case "rowid":
                        _rowid = reader.GetInt64(i);
                        break;
                    case "Users_rowid":
                        _Users_rowid = reader.GetInt64(i);
                        break;
                    case "text":
                        _text = reader.GetValue(i) as string;
                        break;
                    default: 
                        if(AdditionalValues == null)
                            AdditionalValues = new Dictionary<string, object>();

                        AdditionalValues.Add(reader.GetName(i), reader.GetValue(i)); 
                        break;
                }
            }
        }

        /// <summary>
        /// Gets all the instance values in the row which have been changed.
        /// </summary>
        /// <returns>Dictionary with the keys of the column names and values of the properties.</returns>
        public override Dictionary<string, object> GetChangedValues()
        {
            var changed = new Dictionary<string, object>();
            if (ChangedFlags.Get(1))
                changed.Add("Users_rowid", _Users_rowid);
            if (ChangedFlags.Get(2))
                changed.Add("text", _text);

            return changed;
        }

        /// <summary>
        /// Returns true if any of the values have been modified from the properties.
        /// </summary>
        /// <returns>True if the row has any modified values.</returns>
        public override bool IsChanged()
        {
            var length = ChangedFlags.Length;
            for(var i = 0; i < length; i++){
                if(ChangedFlags.Get(i))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Return all the instance values for the entire row.
        /// </summary>
        /// <returns>An object array with all the values of this row.</returns>
        public override object[] GetAllValues()
        {
            return new object[] {
                _Users_rowid,
                _text,
            };
        }

        /// <summary>
        /// Returns all the columns in this row.
        /// </summary>
        /// <returns>A string array with all the columns in this row.</returns>
        public override string[] GetColumns()
        {
            return new [] {
                "Users_rowid",
                "text",
            };
        }

        /// <summary>
        /// Returns all the columns types.
        /// </summary>
        /// <returns>A type array with all the columns in this row.</returns>
        public override Type[] GetColumnTypes()
        {
            return new [] {
                typeof(Int64),
                typeof(String),
            };
        }

        /// <summary>
        /// Gets the name of the row primary key.
        /// </summary>
        /// <returns>The name of the primary key</returns>
        public override string GetPKName()
        {
            return "rowid"; 
        }

        /// <summary>
        /// Gets the value of the primary key.
        /// </summary>
        /// <returns>The value of the primary key.</returns>
        public override object GetPKValue()
        {
            return rowid; 
        }
    }

    [Table(Name = "AllTypes")]
    [DataContract]
    public partial class AllTypes : TableRow, System.ComponentModel.INotifyPropertyChanged
 {

        /// <summary>
        /// Implementation for INotifyPropertyChanged.
        /// </summary>
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Bit array which contains the flags for each table column.
        /// </summary>
        [DataMember(Order = 17)]
        public BitArray ChangedFlags { get; set; }

        /// <summary>
        /// Values which are returned but not part of this table.
        /// </summary>
        [DataMember(Order = 18)]
        public Dictionary<string, object> AdditionalValues { get; set; }

        /// <summary>
        /// Column name.
        /// </summary>
        public const string idColumn = "id";

        /// <summary>
        /// Backing field for the id property.
        /// </summary>
        private Int64 _id;

        [DataMember(Order = 0)]
        public Int64 id
        {
            get => _id;
            set
            {
                _id = value;
                ChangedFlags.Set(0, true);
                PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(id)));
            }
        }

        /// <summary>
        /// Column name.
        /// </summary>
        public const string db_int16Column = "db_int16";

        /// <summary>
        /// Backing field for the db_int16 property.
        /// </summary>
        private Int16? _db_int16;

        [DataMember(Order = 1)]
        public Int16? db_int16
        {
            get => _db_int16;
            set
            {
                _db_int16 = value;
                ChangedFlags.Set(1, true);
                PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(db_int16)));
            }
        }

        /// <summary>
        /// Column name.
        /// </summary>
        public const string db_int32Column = "db_int32";

        /// <summary>
        /// Backing field for the db_int32 property.
        /// </summary>
        private Int32? _db_int32;

        [DataMember(Order = 2)]
        public Int32? db_int32
        {
            get => _db_int32;
            set
            {
                _db_int32 = value;
                ChangedFlags.Set(2, true);
                PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(db_int32)));
            }
        }

        /// <summary>
        /// Column name.
        /// </summary>
        public const string db_int64Column = "db_int64";

        /// <summary>
        /// Backing field for the db_int64 property.
        /// </summary>
        private Int64? _db_int64;

        [DataMember(Order = 3)]
        public Int64? db_int64
        {
            get => _db_int64;
            set
            {
                _db_int64 = value;
                ChangedFlags.Set(3, true);
                PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(db_int64)));
            }
        }

        /// <summary>
        /// Column name.
        /// </summary>
        public const string db_uint16Column = "db_uint16";

        /// <summary>
        /// Backing field for the db_uint16 property.
        /// </summary>
        private UInt16? _db_uint16;

        [DataMember(Order = 4)]
        public UInt16? db_uint16
        {
            get => _db_uint16;
            set
            {
                _db_uint16 = value;
                ChangedFlags.Set(4, true);
                PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(db_uint16)));
            }
        }

        /// <summary>
        /// Column name.
        /// </summary>
        public const string db_uint32Column = "db_uint32";

        /// <summary>
        /// Backing field for the db_uint32 property.
        /// </summary>
        private UInt32? _db_uint32;

        [DataMember(Order = 5)]
        public UInt32? db_uint32
        {
            get => _db_uint32;
            set
            {
                _db_uint32 = value;
                ChangedFlags.Set(5, true);
                PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(db_uint32)));
            }
        }

        /// <summary>
        /// Column name.
        /// </summary>
        public const string db_uint64Column = "db_uint64";

        /// <summary>
        /// Backing field for the db_uint64 property.
        /// </summary>
        private UInt64? _db_uint64;

        [DataMember(Order = 6)]
        public UInt64? db_uint64
        {
            get => _db_uint64;
            set
            {
                _db_uint64 = value;
                ChangedFlags.Set(6, true);
                PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(db_uint64)));
            }
        }

        /// <summary>
        /// Column name.
        /// </summary>
        public const string db_byte_arrayColumn = "db_byte_array";

        /// <summary>
        /// Backing field for the db_byte_array property.
        /// </summary>
        private byte[] _db_byte_array;

        [DataMember(Order = 7)]
        public byte[] db_byte_array
        {
            get => _db_byte_array;
            set
            {
                _db_byte_array = value;
                ChangedFlags.Set(7, true);
                PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(db_byte_array)));
            }
        }

        /// <summary>
        /// Column name.
        /// </summary>
        public const string db_byteColumn = "db_byte";

        /// <summary>
        /// Backing field for the db_byte property.
        /// </summary>
        private Byte? _db_byte;

        [DataMember(Order = 8)]
        public Byte? db_byte
        {
            get => _db_byte;
            set
            {
                _db_byte = value;
                ChangedFlags.Set(8, true);
                PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(db_byte)));
            }
        }

        /// <summary>
        /// Column name.
        /// </summary>
        public const string db_sbyteColumn = "db_sbyte";

        /// <summary>
        /// Backing field for the db_sbyte property.
        /// </summary>
        private SByte _db_sbyte;

        [DataMember(Order = 9)]
        public SByte db_sbyte
        {
            get => _db_sbyte;
            set
            {
                _db_sbyte = value;
                ChangedFlags.Set(9, true);
                PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(db_sbyte)));
            }
        }

        /// <summary>
        /// Column name.
        /// </summary>
        public const string db_date_timeColumn = "db_date_time";

        /// <summary>
        /// Backing field for the db_date_time property.
        /// </summary>
        private DateTimeOffset? _db_date_time;

        [DataMember(Order = 10)]
        public DateTimeOffset? db_date_time
        {
            get => _db_date_time;
            set
            {
                _db_date_time = value;
                ChangedFlags.Set(10, true);
                PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(db_date_time)));
            }
        }

        /// <summary>
        /// Column name.
        /// </summary>
        public const string db_decimalColumn = "db_decimal";

        /// <summary>
        /// Backing field for the db_decimal property.
        /// </summary>
        private Decimal? _db_decimal;

        [DataMember(Order = 11)]
        public Decimal? db_decimal
        {
            get => _db_decimal;
            set
            {
                _db_decimal = value;
                ChangedFlags.Set(11, true);
                PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(db_decimal)));
            }
        }

        /// <summary>
        /// Column name.
        /// </summary>
        public const string db_floatColumn = "db_float";

        /// <summary>
        /// Backing field for the db_float property.
        /// </summary>
        private float? _db_float;

        [DataMember(Order = 12)]
        public float? db_float
        {
            get => _db_float;
            set
            {
                _db_float = value;
                ChangedFlags.Set(12, true);
                PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(db_float)));
            }
        }

        /// <summary>
        /// Column name.
        /// </summary>
        public const string db_doubleColumn = "db_double";

        /// <summary>
        /// Backing field for the db_double property.
        /// </summary>
        private Double? _db_double;

        [DataMember(Order = 13)]
        public Double? db_double
        {
            get => _db_double;
            set
            {
                _db_double = value;
                ChangedFlags.Set(13, true);
                PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(db_double)));
            }
        }

        /// <summary>
        /// Column name.
        /// </summary>
        public const string db_boolColumn = "db_bool";

        /// <summary>
        /// Backing field for the db_bool property.
        /// </summary>
        private Boolean? _db_bool;

        [DataMember(Order = 14)]
        public Boolean? db_bool
        {
            get => _db_bool;
            set
            {
                _db_bool = value;
                ChangedFlags.Set(14, true);
                PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(db_bool)));
            }
        }

        /// <summary>
        /// Column name.
        /// </summary>
        public const string db_stringColumn = "db_string";

        /// <summary>
        /// Backing field for the db_string property.
        /// </summary>
        private String _db_string;

        [DataMember(Order = 15)]
        public String db_string
        {
            get => _db_string;
            set
            {
                _db_string = value;
                ChangedFlags.Set(15, true);
                PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(db_string)));
            }
        }

        /// <summary>
        /// Column name.
        /// </summary>
        public const string db_enumColumn = "db_enum";

        /// <summary>
        /// Backing field for the db_enum property.
        /// </summary>
        private TestEnum _db_enum;

        [DataMember(Order = 16)]
        public TestEnum db_enum
        {
            get => _db_enum;
            set
            {
                _db_enum = value;
                ChangedFlags.Set(16, true);
                PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(db_enum)));
            }
        }

        /// <summary>
        /// Clones a AllTypes row.
        /// </summary>
        /// <param name="source">Source AllTypes row to clone from.</param>
        /// <param name="onlyChanged">True to only clone the changes from the source. False to clone all the values regardless of changed or unchanged.</param>
        public AllTypes(AllTypes source, bool onlyChanged = false)
        { 
            _id = source._id;
            if (onlyChanged == false || source.ChangedFlags.Get(0))
                _id = source._id;
            if (onlyChanged == false || source.ChangedFlags.Get(1))
                _db_int16 = source._db_int16;
            if (onlyChanged == false || source.ChangedFlags.Get(2))
                _db_int32 = source._db_int32;
            if (onlyChanged == false || source.ChangedFlags.Get(3))
                _db_int64 = source._db_int64;
            if (onlyChanged == false || source.ChangedFlags.Get(4))
                _db_uint16 = source._db_uint16;
            if (onlyChanged == false || source.ChangedFlags.Get(5))
                _db_uint32 = source._db_uint32;
            if (onlyChanged == false || source.ChangedFlags.Get(6))
                _db_uint64 = source._db_uint64;
            if (onlyChanged == false || source.ChangedFlags.Get(7))
                _db_byte_array = source._db_byte_array;
            if (onlyChanged == false || source.ChangedFlags.Get(8))
                _db_byte = source._db_byte;
            if (onlyChanged == false || source.ChangedFlags.Get(9))
                _db_sbyte = source._db_sbyte;
            if (onlyChanged == false || source.ChangedFlags.Get(10))
                _db_date_time = source._db_date_time;
            if (onlyChanged == false || source.ChangedFlags.Get(11))
                _db_decimal = source._db_decimal;
            if (onlyChanged == false || source.ChangedFlags.Get(12))
                _db_float = source._db_float;
            if (onlyChanged == false || source.ChangedFlags.Get(13))
                _db_double = source._db_double;
            if (onlyChanged == false || source.ChangedFlags.Get(14))
                _db_bool = source._db_bool;
            if (onlyChanged == false || source.ChangedFlags.Get(15))
                _db_string = source._db_string;
            if (onlyChanged == false || source.ChangedFlags.Get(16))
                _db_enum = source._db_enum;

            ChangedFlags = new BitArray(source.ChangedFlags);
        }

        /// <summary>
        /// Creates a empty AllTypes row. Use this for creating a new row and inserting into the database.
        /// </summary>
        public AllTypes() : this(null, null) { }

        /// <summary>
        /// Creates a AllTypes row and reads the row information from the table into this row.
        /// </summary>
        /// <param name="reader">Instance of a live data reader for this row's table.</param>
        /// <param name="context">The current context of the database.</param>
        public AllTypes(DbDataReader reader, Context context)
        {
            ChangedFlags = new BitArray(17);
            Read(reader, context);
        }

        /// <summary>
        /// Creates a AllTypes row and with the specified Id.
        /// Useful when creating a new matching row on a remote connection.
        /// </summary>
        /// <param name="id">Id to set the row to.</param>
        public AllTypes(Int64 id)
        {
            ChangedFlags = new BitArray(17);
            _id = id;
        }

        /// <summary>
        /// Reads the row information from the table into this row.
        /// </summary>
        /// <param name="reader">Instance of a live data reader for this row's table.</param>
        /// <param name="context">The current context of the database.</param>
        public override void Read(DbDataReader reader, Context context) {
            Context = context;
            if (reader == null)
                return;

            var length = reader.FieldCount;
            for (var i = 0; i < length; i++)
            {
                switch (reader.GetName(i))
                {
                    case "id":
                        _id = reader.GetInt64(i);
                        break;
                    case "db_int16":
                        _db_int16 = reader.IsDBNull(i) ? default(Int16?) : reader.GetInt16(i);
                        break;
                    case "db_int32":
                        _db_int32 = reader.IsDBNull(i) ? default(Int32?) : reader.GetInt32(i);
                        break;
                    case "db_int64":
                        _db_int64 = reader.IsDBNull(i) ? default(Int64?) : reader.GetInt64(i);
                        break;
                    case "db_uint16":
                        _db_uint16 = reader.IsDBNull(i) ? default : (UInt16)(long)reader.GetValue(i);
                        break;
                    case "db_uint32":
                        _db_uint32 = reader.IsDBNull(i) ? default : (UInt32)(long)reader.GetValue(i);
                        break;
                    case "db_uint64":
                        _db_uint64 = reader.IsDBNull(i) ? default : (UInt64)(long)reader.GetValue(i);
                        break;
                    case "db_byte_array":
                        _db_byte_array = reader.IsDBNull(i) ? null : reader.GetFieldValue<byte[]>(i);
                        break;
                    case "db_byte":
                        _db_byte = reader.IsDBNull(i) ? default(Byte?) : reader.GetByte(i);
                        break;
                    case "db_sbyte":
                        _db_sbyte = reader.IsDBNull(i) ? default : (SByte)reader.GetByte(i);
                        break;
                    case "db_date_time":
                        _db_date_time = reader.IsDBNull(i) ? default(DateTimeOffset?) : reader.GetDateTime(i);
                        break;
                    case "db_decimal":
                        _db_decimal = reader.IsDBNull(i) ? default(Decimal?) : reader.GetDecimal(i);
                        break;
                    case "db_float":
                        _db_float = reader.IsDBNull(i) ? default(float?) : reader.GetFloat(i);
                        break;
                    case "db_double":
                        _db_double = reader.IsDBNull(i) ? default(Double?) : reader.GetDouble(i);
                        break;
                    case "db_bool":
                        _db_bool = reader.IsDBNull(i) ? default(Boolean?) : reader.GetBoolean(i);
                        break;
                    case "db_string":
                        _db_string = reader.IsDBNull(i) ? default(String) : reader.GetString(i);
                        break;
                    case "db_enum":
                        _db_enum = (TestEnum)reader.GetInt32(i);
                        break;
                    default: 
                        if(AdditionalValues == null)
                            AdditionalValues = new Dictionary<string, object>();

                        AdditionalValues.Add(reader.GetName(i), reader.GetValue(i)); 
                        break;
                }
            }
        }

        /// <summary>
        /// Gets all the instance values in the row which have been changed.
        /// </summary>
        /// <returns>Dictionary with the keys of the column names and values of the properties.</returns>
        public override Dictionary<string, object> GetChangedValues()
        {
            var changed = new Dictionary<string, object>();
            if (ChangedFlags.Get(1))
                changed.Add("db_int16", _db_int16);
            if (ChangedFlags.Get(2))
                changed.Add("db_int32", _db_int32);
            if (ChangedFlags.Get(3))
                changed.Add("db_int64", _db_int64);
            if (ChangedFlags.Get(4))
                changed.Add("db_uint16", _db_uint16);
            if (ChangedFlags.Get(5))
                changed.Add("db_uint32", _db_uint32);
            if (ChangedFlags.Get(6))
                changed.Add("db_uint64", _db_uint64);
            if (ChangedFlags.Get(7))
                changed.Add("db_byte_array", _db_byte_array);
            if (ChangedFlags.Get(8))
                changed.Add("db_byte", _db_byte);
            if (ChangedFlags.Get(9))
                changed.Add("db_sbyte", _db_sbyte);
            if (ChangedFlags.Get(10))
                changed.Add("db_date_time", _db_date_time);
            if (ChangedFlags.Get(11))
                changed.Add("db_decimal", _db_decimal);
            if (ChangedFlags.Get(12))
                changed.Add("db_float", _db_float);
            if (ChangedFlags.Get(13))
                changed.Add("db_double", _db_double);
            if (ChangedFlags.Get(14))
                changed.Add("db_bool", _db_bool);
            if (ChangedFlags.Get(15))
                changed.Add("db_string", _db_string);
            if (ChangedFlags.Get(16))
                changed.Add("db_enum", _db_enum);

            return changed;
        }

        /// <summary>
        /// Returns true if any of the values have been modified from the properties.
        /// </summary>
        /// <returns>True if the row has any modified values.</returns>
        public override bool IsChanged()
        {
            var length = ChangedFlags.Length;
            for(var i = 0; i < length; i++){
                if(ChangedFlags.Get(i))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Return all the instance values for the entire row.
        /// </summary>
        /// <returns>An object array with all the values of this row.</returns>
        public override object[] GetAllValues()
        {
            return new object[] {
                _db_int16,
                _db_int32,
                _db_int64,
                _db_uint16,
                _db_uint32,
                _db_uint64,
                _db_byte_array,
                _db_byte,
                _db_sbyte,
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
        /// Returns all the columns in this row.
        /// </summary>
        /// <returns>A string array with all the columns in this row.</returns>
        public override string[] GetColumns()
        {
            return new [] {
                "db_int16",
                "db_int32",
                "db_int64",
                "db_uint16",
                "db_uint32",
                "db_uint64",
                "db_byte_array",
                "db_byte",
                "db_sbyte",
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
        /// Returns all the columns types.
        /// </summary>
        /// <returns>A type array with all the columns in this row.</returns>
        public override Type[] GetColumnTypes()
        {
            return new [] {
                typeof(Int16?),
                typeof(Int32?),
                typeof(Int64?),
                typeof(UInt16?),
                typeof(UInt32?),
                typeof(UInt64?),
                typeof(byte[]),
                typeof(Byte?),
                typeof(SByte),
                typeof(DateTimeOffset?),
                typeof(Decimal?),
                typeof(float?),
                typeof(Double?),
                typeof(Boolean?),
                typeof(String),
                typeof(TestEnum),
            };
        }

        /// <summary>
        /// Gets the name of the row primary key.
        /// </summary>
        /// <returns>The name of the primary key</returns>
        public override string GetPKName()
        {
            return "id"; 
        }

        /// <summary>
        /// Gets the value of the primary key.
        /// </summary>
        /// <returns>The value of the primary key.</returns>
        public override object GetPKValue()
        {
            return id; 
        }
    }
}
