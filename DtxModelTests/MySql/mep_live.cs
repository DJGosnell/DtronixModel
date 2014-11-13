using System;
using System.Data.Common;
using System.Collections.Generic;
using DtxModel;

namespace DtxModelTests.MySql {

	public partial class mep_liveContext : Context {
		private static Func<DbConnection> _default_connection = null;

		/// <summary>
		/// Set a default constructor to allow use of parameterless context calling.
		/// </summary>
		public static Func<DbConnection> DefaultConnection {
			get { return _default_connection; }
			set { _default_connection = value; }
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

		private Table<Settings> _Settings;

		public Table<Settings> Settings {
			get {
				if(_Settings == null) {
					_Settings = new Table<Settings>(this);
				}

				return _Settings;
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

		private Table<Permissions> _Permissions;

		public Table<Permissions> Permissions {
			get {
				if(_Permissions == null) {
					_Permissions = new Table<Permissions>(this);
				}

				return _Permissions;
			}
		}

		private Table<Projects> _Projects;

		public Table<Projects> Projects {
			get {
				if(_Projects == null) {
					_Projects = new Table<Projects>(this);
				}

				return _Projects;
			}
		}

		private Table<Companies> _Companies;

		public Table<Companies> Companies {
			get {
				if(_Companies == null) {
					_Companies = new Table<Companies>(this);
				}

				return _Companies;
			}
		}

		private Table<ProjectCategories> _ProjectCategories;

		public Table<ProjectCategories> ProjectCategories {
			get {
				if(_ProjectCategories == null) {
					_ProjectCategories = new Table<ProjectCategories>(this);
				}

				return _ProjectCategories;
			}
		}

		private Table<ProjectPictures> _ProjectPictures;

		public Table<ProjectPictures> ProjectPictures {
			get {
				if(_ProjectPictures == null) {
					_ProjectPictures = new Table<ProjectPictures>(this);
				}

				return _ProjectPictures;
			}
		}

		private Table<Contacts> _Contacts;

		public Table<Contacts> Contacts {
			get {
				if(_Contacts == null) {
					_Contacts = new Table<Contacts>(this);
				}

				return _Contacts;
			}
		}

		private Table<Messages> _Messages;

		public Table<Messages> Messages {
			get {
				if(_Messages == null) {
					_Messages = new Table<Messages>(this);
				}

				return _Messages;
			}
		}

		private Table<ProjectsContactsList> _ProjectsContactsList;

		public Table<ProjectsContactsList> ProjectsContactsList {
			get {
				if(_ProjectsContactsList == null) {
					_ProjectsContactsList = new Table<ProjectsContactsList>(this);
				}

				return _ProjectsContactsList;
			}
		}

		private Table<Addresses> _Addresses;

		public Table<Addresses> Addresses {
			get {
				if(_Addresses == null) {
					_Addresses = new Table<Addresses>(this);
				}

				return _Addresses;
			}
		}

		private Table<ConstructionDetails> _ConstructionDetails;

		public Table<ConstructionDetails> ConstructionDetails {
			get {
				if(_ConstructionDetails == null) {
					_ConstructionDetails = new Table<ConstructionDetails>(this);
				}

				return _ConstructionDetails;
			}
		}

		private Table<ProjectTasks> _ProjectTasks;

		public Table<ProjectTasks> ProjectTasks {
			get {
				if(_ProjectTasks == null) {
					_ProjectTasks = new Table<ProjectTasks>(this);
				}

				return _ProjectTasks;
			}
		}

		private Table<ProjectTaskTypes> _ProjectTaskTypes;

		public Table<ProjectTaskTypes> ProjectTaskTypes {
			get {
				if(_ProjectTaskTypes == null) {
					_ProjectTaskTypes = new Table<ProjectTaskTypes>(this);
				}

				return _ProjectTaskTypes;
			}
		}

		private Table<ProjectTaskStatuses> _ProjectTaskStatuses;

		public Table<ProjectTaskStatuses> ProjectTaskStatuses {
			get {
				if(_ProjectTaskStatuses == null) {
					_ProjectTaskStatuses = new Table<ProjectTaskStatuses>(this);
				}

				return _ProjectTaskStatuses;
			}
		}

		private Table<ProjectTasksUsersInvolved> _ProjectTasksUsersInvolved;

		public Table<ProjectTasksUsersInvolved> ProjectTasksUsersInvolved {
			get {
				if(_ProjectTasksUsersInvolved == null) {
					_ProjectTasksUsersInvolved = new Table<ProjectTasksUsersInvolved>(this);
				}

				return _ProjectTasksUsersInvolved;
			}
		}

		/// <summary>
		/// Create a new context of this database's type.  Can only be used if a default connection is specified.
		/// </summary>
		public mep_liveContext() : base(_default_connection) { }

		/// <summary>
		/// Create a new context of this database's type with a specific connection.
		/// </summary>
		/// <param name="connection">Existing open database connection to use.</param>
		public mep_liveContext(DbConnection connection) : base(connection) { }
	}

	[TableAttribute(Name = "Users")]
	public partial class Users : Model {
		private bool _idChanged = false;
		private Int32 _id;
		/// <summary>
		/// Row identifier.
		/// </summary>
		public Int32 id {
			get { return _id; }
			set {
				_id = value;
				_idChanged = true;
			}
		}

		private bool _usernameChanged = false;
		private String _username;
		/// <summary>
		/// Username for the user.
		/// </summary>
		public String username {
			get { return _username; }
			set {
				_username = value;
				_usernameChanged = true;
			}
		}

		private bool _Permissions_idChanged = false;
		private Int32 _Permissions_id;
		/// <summary>
		/// Permission ID associated with this account.
		/// </summary>
		public Int32 Permissions_id {
			get { return _Permissions_id; }
			set {
				_Permissions_id = value;
				_Permissions_idChanged = true;
			}
		}

		private bool _Contacts_idChanged = false;
		private Int32 _Contacts_id;
		public Int32 Contacts_id {
			get { return _Contacts_id; }
			set {
				_Contacts_id = value;
				_Contacts_idChanged = true;
			}
		}

		private bool _passwordChanged = false;
		private String _password;
		/// <summary>
		/// MD5 hashed password.
		/// </summary>
		public String password {
			get { return _password; }
			set {
				_password = value;
				_passwordChanged = true;
			}
		}

		private bool _date_registeredChanged = false;
		private Int64 _date_registered;
		/// <summary>
		/// Date when the user registered on the application.
		/// </summary>
		public Int64 date_registered {
			get { return _date_registered; }
			set {
				_date_registered = value;
				_date_registeredChanged = true;
			}
		}

		private bool _activation_codeChanged = false;
		private String _activation_code;
		/// <summary>
		/// Code that is used when the account needs activation. Empty if no activation is required.
		/// </summary>
		public String activation_code {
			get { return _activation_code; }
			set {
				_activation_code = value;
				_activation_codeChanged = true;
			}
		}

		private bool _bannedChanged = false;
		private Int16 _banned;
		/// <summary>
		/// True if the user is banned, false otherwise.
		/// </summary>
		public Int16 banned {
			get { return _banned; }
			set {
				_banned = value;
				_bannedChanged = true;
			}
		}

		private bool _ban_reasonChanged = false;
		private String _ban_reason;
		/// <summary>
		/// Reason that the user was banned.
		/// </summary>
		public String ban_reason {
			get { return _ban_reason; }
			set {
				_ban_reason = value;
				_ban_reasonChanged = true;
			}
		}

		private bool _last_onlineChanged = false;
		private Int32 _last_online;
		/// <summary>
		/// Time the user was last active on this application.
		/// </summary>
		public Int32 last_online {
			get { return _last_online; }
			set {
				_last_online = value;
				_last_onlineChanged = true;
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
					case "id": _id = reader.GetInt32(i); break;
					case "username": _username = reader.GetValue(i) as String; break;
					case "Permissions_id": _Permissions_id = reader.GetInt32(i); break;
					case "Contacts_id": _Contacts_id = reader.GetInt32(i); break;
					case "password": _password = reader.GetValue(i) as String; break;
					case "date_registered": _date_registered = reader.GetInt64(i); break;
					case "activation_code": _activation_code = reader.GetValue(i) as String; break;
					case "banned": _banned = reader.GetInt16(i); break;
					case "ban_reason": _ban_reason = reader.GetValue(i) as String; break;
					case "last_online": _last_online = reader.GetInt32(i); break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_usernameChanged)
				changed.Add("username", _username);
			if (_Permissions_idChanged)
				changed.Add("Permissions_id", _Permissions_id);
			if (_Contacts_idChanged)
				changed.Add("Contacts_id", _Contacts_id);
			if (_passwordChanged)
				changed.Add("password", _password);
			if (_date_registeredChanged)
				changed.Add("date_registered", _date_registered);
			if (_activation_codeChanged)
				changed.Add("activation_code", _activation_code);
			if (_bannedChanged)
				changed.Add("banned", _banned);
			if (_ban_reasonChanged)
				changed.Add("ban_reason", _ban_reason);
			if (_last_onlineChanged)
				changed.Add("last_online", _last_online);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_username,
				_Permissions_id,
				_Contacts_id,
				_password,
				_date_registered,
				_activation_code,
				_banned,
				_ban_reason,
				_last_online,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"username",
				"Permissions_id",
				"Contacts_id",
				"password",
				"date_registered",
				"activation_code",
				"banned",
				"ban_reason",
				"last_online",
			};
		}

		public override string GetPKName() {
			return "id";
		}

		public override object GetPKValue() {
			return _id;
		}

	}

	[TableAttribute(Name = "Settings")]
	public partial class Settings : Model {
		private bool _idChanged = false;
		private Int32 _id;
		/// <summary>
		/// Row identifier.
		/// </summary>
		public Int32 id {
			get { return _id; }
			set {
				_id = value;
				_idChanged = true;
			}
		}

		private bool _propertyChanged = false;
		private String _property;
		/// <summary>
		/// Key that references the same row's value.
		/// </summary>
		public String property {
			get { return _property; }
			set {
				_property = value;
				_propertyChanged = true;
			}
		}

		private bool _valueChanged = false;
		private String _value;
		/// <summary>
		/// Stored setting value.
		/// </summary>
		public String value {
			get { return _value; }
			set {
				_value = value;
				_valueChanged = true;
			}
		}

		private bool _descriptionChanged = false;
		private String _description;
		/// <summary>
		/// Short description about the setting.
		/// </summary>
		public String description {
			get { return _description; }
			set {
				_description = value;
				_descriptionChanged = true;
			}
		}

		public Settings() : this(null, null) { }

		public Settings(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "id": _id = reader.GetInt32(i); break;
					case "property": _property = reader.GetValue(i) as String; break;
					case "value": _value = reader.GetValue(i) as String; break;
					case "description": _description = reader.GetValue(i) as String; break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_propertyChanged)
				changed.Add("property", _property);
			if (_valueChanged)
				changed.Add("value", _value);
			if (_descriptionChanged)
				changed.Add("description", _description);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_property,
				_value,
				_description,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"property",
				"value",
				"description",
			};
		}

		public override string GetPKName() {
			return "id";
		}

		public override object GetPKValue() {
			return _id;
		}

	}

	[TableAttribute(Name = "Logs")]
	public partial class Logs : Model {
		private bool _idChanged = false;
		private Int32 _id;
		/// <summary>
		/// Row identifier.
		/// </summary>
		public Int32 id {
			get { return _id; }
			set {
				_id = value;
				_idChanged = true;
			}
		}

		private bool _Users_idChanged = false;
		private Int32 _Users_id;
		/// <summary>
		/// User ID that this log item occured on.
		/// </summary>
		public Int32 Users_id {
			get { return _Users_id; }
			set {
				_Users_id = value;
				_Users_idChanged = true;
			}
		}

		private bool _timeChanged = false;
		private Int64 _time;
		/// <summary>
		/// Time of log entry.
		/// </summary>
		public Int64 time {
			get { return _time; }
			set {
				_time = value;
				_timeChanged = true;
			}
		}

		private bool _typeChanged = false;
		private Int16 _type;
		/// <summary>
		/// Type ID of the error that occured.
		/// </summary>
		public Int16 type {
			get { return _type; }
			set {
				_type = value;
				_typeChanged = true;
			}
		}

		private bool _ipChanged = false;
		private Int64 _ip;
		/// <summary>
		/// IP address of the client that this log item occured on.
		/// </summary>
		public Int64 ip {
			get { return _ip; }
			set {
				_ip = value;
				_ipChanged = true;
			}
		}

		private bool _messageChanged = false;
		private String _message;
		/// <summary>
		/// Full message text of the error and or backtrace.
		/// </summary>
		public String message {
			get { return _message; }
			set {
				_message = value;
				_messageChanged = true;
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
					case "id": _id = reader.GetInt32(i); break;
					case "Users_id": _Users_id = reader.GetInt32(i); break;
					case "time": _time = (reader.IsDBNull(i)) ? default(Int64) : reader.GetInt64(i); break;
					case "type": _type = (reader.IsDBNull(i)) ? default(Int16) : reader.GetInt16(i); break;
					case "ip": _ip = (reader.IsDBNull(i)) ? default(Int64) : reader.GetInt64(i); break;
					case "message": _message = reader.GetValue(i) as String; break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_Users_idChanged)
				changed.Add("Users_id", _Users_id);
			if (_timeChanged)
				changed.Add("time", _time);
			if (_typeChanged)
				changed.Add("type", _type);
			if (_ipChanged)
				changed.Add("ip", _ip);
			if (_messageChanged)
				changed.Add("message", _message);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_Users_id,
				_time,
				_type,
				_ip,
				_message,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"Users_id",
				"time",
				"type",
				"ip",
				"message",
			};
		}

		public override string GetPKName() {
			return "id";
		}

		public override object GetPKValue() {
			return _id;
		}

	}

	[TableAttribute(Name = "Permissions")]
	public partial class Permissions : Model {
		private bool _idChanged = false;
		private Int32 _id;
		/// <summary>
		/// Row identifier.
		/// </summary>
		public Int32 id {
			get { return _id; }
			set {
				_id = value;
				_idChanged = true;
			}
		}

		private bool _nameChanged = false;
		private String _name;
		/// <summary>
		/// Internal name associated with this permission set.
		/// </summary>
		public String name {
			get { return _name; }
			set {
				_name = value;
				_nameChanged = true;
			}
		}

		private bool _base_permissions_idChanged = false;
		private Int32 _base_permissions_id;
		/// <summary>
		/// Permission set that serves as a base for this one.
		/// </summary>
		public Int32 base_permissions_id {
			get { return _base_permissions_id; }
			set {
				_base_permissions_id = value;
				_base_permissions_idChanged = true;
			}
		}

		private bool _can_registerChanged = false;
		private Boolean _can_register;
		/// <summary>
		/// True for enabling registration.
		/// </summary>
		public Boolean can_register {
			get { return _can_register; }
			set {
				_can_register = value;
				_can_registerChanged = true;
			}
		}

		private bool _can_loginChanged = false;
		private Boolean _can_login;
		/// <summary>
		/// True to allow user login.
		/// </summary>
		public Boolean can_login {
			get { return _can_login; }
			set {
				_can_login = value;
				_can_loginChanged = true;
			}
		}

		private bool _can_edit_usersChanged = false;
		private Boolean _can_edit_users;
		/// <summary>
		/// True to allow user to edit other user's settings.
		/// </summary>
		public Boolean can_edit_users {
			get { return _can_edit_users; }
			set {
				_can_edit_users = value;
				_can_edit_usersChanged = true;
			}
		}

		private bool _can_edit_settingsChanged = false;
		private Boolean _can_edit_settings;
		/// <summary>
		/// True to allow user to edit application settings.
		/// </summary>
		public Boolean can_edit_settings {
			get { return _can_edit_settings; }
			set {
				_can_edit_settings = value;
				_can_edit_settingsChanged = true;
			}
		}

		private bool _can_manage_imagesChanged = false;
		private Boolean _can_manage_images;
		/// <summary>
		/// True to allow image upload and management.
		/// </summary>
		public Boolean can_manage_images {
			get { return _can_manage_images; }
			set {
				_can_manage_images = value;
				_can_manage_imagesChanged = true;
			}
		}

		private bool _access_xflowChanged = false;
		private Boolean _access_xflow;
		/// <summary>
		/// True to allow access to x-flow for projects.
		/// </summary>
		public Boolean access_xflow {
			get { return _access_xflow; }
			set {
				_access_xflow = value;
				_access_xflowChanged = true;
			}
		}

		public Permissions() : this(null, null) { }

		public Permissions(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "id": _id = reader.GetInt32(i); break;
					case "name": _name = reader.GetValue(i) as String; break;
					case "base_permissions_id": _base_permissions_id = (reader.IsDBNull(i)) ? default(Int32) : reader.GetInt32(i); break;
					case "can_register": _can_register = (reader.IsDBNull(i)) ? default(Boolean) : reader.GetBoolean(i); break;
					case "can_login": _can_login = (reader.IsDBNull(i)) ? default(Boolean) : reader.GetBoolean(i); break;
					case "can_edit_users": _can_edit_users = (reader.IsDBNull(i)) ? default(Boolean) : reader.GetBoolean(i); break;
					case "can_edit_settings": _can_edit_settings = (reader.IsDBNull(i)) ? default(Boolean) : reader.GetBoolean(i); break;
					case "can_manage_images": _can_manage_images = (reader.IsDBNull(i)) ? default(Boolean) : reader.GetBoolean(i); break;
					case "access_xflow": _access_xflow = (reader.IsDBNull(i)) ? default(Boolean) : reader.GetBoolean(i); break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_nameChanged)
				changed.Add("name", _name);
			if (_base_permissions_idChanged)
				changed.Add("base_permissions_id", _base_permissions_id);
			if (_can_registerChanged)
				changed.Add("can_register", _can_register);
			if (_can_loginChanged)
				changed.Add("can_login", _can_login);
			if (_can_edit_usersChanged)
				changed.Add("can_edit_users", _can_edit_users);
			if (_can_edit_settingsChanged)
				changed.Add("can_edit_settings", _can_edit_settings);
			if (_can_manage_imagesChanged)
				changed.Add("can_manage_images", _can_manage_images);
			if (_access_xflowChanged)
				changed.Add("access_xflow", _access_xflow);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_name,
				_base_permissions_id,
				_can_register,
				_can_login,
				_can_edit_users,
				_can_edit_settings,
				_can_manage_images,
				_access_xflow,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"name",
				"base_permissions_id",
				"can_register",
				"can_login",
				"can_edit_users",
				"can_edit_settings",
				"can_manage_images",
				"access_xflow",
			};
		}

		public override string GetPKName() {
			return "id";
		}

		public override object GetPKValue() {
			return _id;
		}

	}

	[TableAttribute(Name = "Projects")]
	public partial class Projects : Model {
		private bool _idChanged = false;
		private Int32 _id;
		/// <summary>
		/// Project id.
		/// </summary>
		public Int32 id {
			get { return _id; }
			set {
				_id = value;
				_idChanged = true;
			}
		}

		private bool _Companies_id_architectChanged = false;
		private Int32 _Companies_id_architect;
		/// <summary>
		/// ID of the architect that designed the building.
		/// </summary>
		public Int32 Companies_id_architect {
			get { return _Companies_id_architect; }
			set {
				_Companies_id_architect = value;
				_Companies_id_architectChanged = true;
			}
		}

		private bool _ProjectCategories_idChanged = false;
		private Int32 _ProjectCategories_id;
		/// <summary>
		/// Category of the project.
		/// </summary>
		public Int32 ProjectCategories_id {
			get { return _ProjectCategories_id; }
			set {
				_ProjectCategories_id = value;
				_ProjectCategories_idChanged = true;
			}
		}

		private bool _Addresses_idChanged = false;
		private Int32 _Addresses_id;
		public Int32 Addresses_id {
			get { return _Addresses_id; }
			set {
				_Addresses_id = value;
				_Addresses_idChanged = true;
			}
		}

		private bool _nameChanged = false;
		private String _name;
		/// <summary>
		/// Name of the project.
		/// </summary>
		public String name {
			get { return _name; }
			set {
				_name = value;
				_nameChanged = true;
			}
		}

		private bool _statusChanged = false;
		private Int32 _status;
		/// <summary>
		/// Status of the project. (Ongoing, Postponed, Complete, etc...)
		/// </summary>
		public Int32 status {
			get { return _status; }
			set {
				_status = value;
				_statusChanged = true;
			}
		}

		private bool _directoryChanged = false;
		private String _directory;
		/// <summary>
		/// Location the project is located on the server.
		/// </summary>
		public String directory {
			get { return _directory; }
			set {
				_directory = value;
				_directoryChanged = true;
			}
		}

		private bool _date_startedChanged = false;
		private Int64 _date_started;
		/// <summary>
		/// Date when the project started.
		/// </summary>
		public Int64 date_started {
			get { return _date_started; }
			set {
				_date_started = value;
				_date_startedChanged = true;
			}
		}

		private bool _date_completedChanged = false;
		private Int64 _date_completed;
		/// <summary>
		/// Date when the project was completed.
		/// </summary>
		public Int64 date_completed {
			get { return _date_completed; }
			set {
				_date_completed = value;
				_date_completedChanged = true;
			}
		}

		private bool _detailsChanged = false;
		private String _details;
		/// <summary>
		/// Short description of the project.
		/// </summary>
		public String details {
			get { return _details; }
			set {
				_details = value;
				_detailsChanged = true;
			}
		}

		public Projects() : this(null, null) { }

		public Projects(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "id": _id = reader.GetInt32(i); break;
					case "Companies_id_architect": _Companies_id_architect = (reader.IsDBNull(i)) ? default(Int32) : reader.GetInt32(i); break;
					case "ProjectCategories_id": _ProjectCategories_id = (reader.IsDBNull(i)) ? default(Int32) : reader.GetInt32(i); break;
					case "Addresses_id": _Addresses_id = (reader.IsDBNull(i)) ? default(Int32) : reader.GetInt32(i); break;
					case "name": _name = reader.GetValue(i) as String; break;
					case "status": _status = (reader.IsDBNull(i)) ? default(Int32) : reader.GetInt32(i); break;
					case "directory": _directory = reader.GetValue(i) as String; break;
					case "date_started": _date_started = (reader.IsDBNull(i)) ? default(Int64) : reader.GetInt64(i); break;
					case "date_completed": _date_completed = (reader.IsDBNull(i)) ? default(Int64) : reader.GetInt64(i); break;
					case "details": _details = reader.GetValue(i) as String; break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_Companies_id_architectChanged)
				changed.Add("Companies_id_architect", _Companies_id_architect);
			if (_ProjectCategories_idChanged)
				changed.Add("ProjectCategories_id", _ProjectCategories_id);
			if (_Addresses_idChanged)
				changed.Add("Addresses_id", _Addresses_id);
			if (_nameChanged)
				changed.Add("name", _name);
			if (_statusChanged)
				changed.Add("status", _status);
			if (_directoryChanged)
				changed.Add("directory", _directory);
			if (_date_startedChanged)
				changed.Add("date_started", _date_started);
			if (_date_completedChanged)
				changed.Add("date_completed", _date_completed);
			if (_detailsChanged)
				changed.Add("details", _details);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_Companies_id_architect,
				_ProjectCategories_id,
				_Addresses_id,
				_name,
				_status,
				_directory,
				_date_started,
				_date_completed,
				_details,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"Companies_id_architect",
				"ProjectCategories_id",
				"Addresses_id",
				"name",
				"status",
				"directory",
				"date_started",
				"date_completed",
				"details",
			};
		}

		public override string GetPKName() {
			return "id";
		}

		public override object GetPKValue() {
			return _id;
		}

	}

	[TableAttribute(Name = "Companies")]
	public partial class Companies : Model {
		private bool _idChanged = false;
		private Int32 _id;
		/// <summary>
		/// Row identifier.
		/// </summary>
		public Int32 id {
			get { return _id; }
			set {
				_id = value;
				_idChanged = true;
			}
		}

		private bool _nameChanged = false;
		private String _name;
		/// <summary>
		/// Name of the architectural firm.
		/// </summary>
		public String name {
			get { return _name; }
			set {
				_name = value;
				_nameChanged = true;
			}
		}

		private bool _urlChanged = false;
		private String _url;
		/// <summary>
		/// Internet acddress of the architect.
		/// </summary>
		public String url {
			get { return _url; }
			set {
				_url = value;
				_urlChanged = true;
			}
		}

		private bool _notesChanged = false;
		private String _notes;
		public String notes {
			get { return _notes; }
			set {
				_notes = value;
				_notesChanged = true;
			}
		}

		private bool _Addresses_idChanged = false;
		private Int32 _Addresses_id;
		public Int32 Addresses_id {
			get { return _Addresses_id; }
			set {
				_Addresses_id = value;
				_Addresses_idChanged = true;
			}
		}

		public Companies() : this(null, null) { }

		public Companies(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "id": _id = reader.GetInt32(i); break;
					case "name": _name = reader.GetValue(i) as String; break;
					case "url": _url = reader.GetValue(i) as String; break;
					case "notes": _notes = reader.GetValue(i) as String; break;
					case "Addresses_id": _Addresses_id = (reader.IsDBNull(i)) ? default(Int32) : reader.GetInt32(i); break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_nameChanged)
				changed.Add("name", _name);
			if (_urlChanged)
				changed.Add("url", _url);
			if (_notesChanged)
				changed.Add("notes", _notes);
			if (_Addresses_idChanged)
				changed.Add("Addresses_id", _Addresses_id);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_name,
				_url,
				_notes,
				_Addresses_id,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"name",
				"url",
				"notes",
				"Addresses_id",
			};
		}

		public override string GetPKName() {
			return "id";
		}

		public override object GetPKValue() {
			return _id;
		}

	}

	[TableAttribute(Name = "ProjectCategories")]
	public partial class ProjectCategories : Model {
		private bool _idChanged = false;
		private Int32 _id;
		/// <summary>
		/// Row identifier.
		/// </summary>
		public Int32 id {
			get { return _id; }
			set {
				_id = value;
				_idChanged = true;
			}
		}

		private bool _nameChanged = false;
		private String _name;
		/// <summary>
		/// Name of project category
		/// </summary>
		public String name {
			get { return _name; }
			set {
				_name = value;
				_nameChanged = true;
			}
		}

		public ProjectCategories() : this(null, null) { }

		public ProjectCategories(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "id": _id = reader.GetInt32(i); break;
					case "name": _name = reader.GetValue(i) as String; break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_nameChanged)
				changed.Add("name", _name);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_name,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"name",
			};
		}

		public override string GetPKName() {
			return "id";
		}

		public override object GetPKValue() {
			return _id;
		}

	}

	[TableAttribute(Name = "ProjectPictures")]
	public partial class ProjectPictures : Model {
		private bool _idChanged = false;
		private Int32 _id;
		/// <summary>
		/// Image identifier.
		/// </summary>
		public Int32 id {
			get { return _id; }
			set {
				_id = value;
				_idChanged = true;
			}
		}

		private bool _Projects_idChanged = false;
		private Int32 _Projects_id;
		/// <summary>
		/// Id of the project this picture belongs to.
		/// </summary>
		public Int32 Projects_id {
			get { return _Projects_id; }
			set {
				_Projects_id = value;
				_Projects_idChanged = true;
			}
		}

		private bool _descriptionChanged = false;
		private String _description;
		/// <summary>
		/// Short description of the picture.
		/// </summary>
		public String description {
			get { return _description; }
			set {
				_description = value;
				_descriptionChanged = true;
			}
		}

		private bool _displayChanged = false;
		private Boolean _display;
		/// <summary>
		/// True if the picture is to be displayed; false otherwise.
		/// </summary>
		public Boolean display {
			get { return _display; }
			set {
				_display = value;
				_displayChanged = true;
			}
		}

		private bool _featuredChanged = false;
		private Boolean _featured;
		/// <summary>
		/// True if the picture is to be featured on the main rotation.
		/// </summary>
		public Boolean featured {
			get { return _featured; }
			set {
				_featured = value;
				_featuredChanged = true;
			}
		}

		public ProjectPictures() : this(null, null) { }

		public ProjectPictures(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "id": _id = reader.GetInt32(i); break;
					case "Projects_id": _Projects_id = reader.GetInt32(i); break;
					case "description": _description = reader.GetValue(i) as String; break;
					case "display": _display = reader.GetBoolean(i); break;
					case "featured": _featured = reader.GetBoolean(i); break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_Projects_idChanged)
				changed.Add("Projects_id", _Projects_id);
			if (_descriptionChanged)
				changed.Add("description", _description);
			if (_displayChanged)
				changed.Add("display", _display);
			if (_featuredChanged)
				changed.Add("featured", _featured);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_Projects_id,
				_description,
				_display,
				_featured,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"Projects_id",
				"description",
				"display",
				"featured",
			};
		}

		public override string GetPKName() {
			return "id";
		}

		public override object GetPKValue() {
			return _id;
		}

	}

	[TableAttribute(Name = "Contacts")]
	public partial class Contacts : Model {
		private bool _idChanged = false;
		private Int32 _id;
		public Int32 id {
			get { return _id; }
			set {
				_id = value;
				_idChanged = true;
			}
		}

		private bool _Addresses_idChanged = false;
		private Int32 _Addresses_id;
		public Int32 Addresses_id {
			get { return _Addresses_id; }
			set {
				_Addresses_id = value;
				_Addresses_idChanged = true;
			}
		}

		private bool _Companies_idChanged = false;
		private Int32 _Companies_id;
		public Int32 Companies_id {
			get { return _Companies_id; }
			set {
				_Companies_id = value;
				_Companies_idChanged = true;
			}
		}

		private bool _first_nameChanged = false;
		private String _first_name;
		public String first_name {
			get { return _first_name; }
			set {
				_first_name = value;
				_first_nameChanged = true;
			}
		}

		private bool _last_nameChanged = false;
		private String _last_name;
		public String last_name {
			get { return _last_name; }
			set {
				_last_name = value;
				_last_nameChanged = true;
			}
		}

		private bool _displayChanged = false;
		private String _display;
		/// <summary>
		/// Combination of the first and last names.
		/// </summary>
		public String display {
			get { return _display; }
			set {
				_display = value;
				_displayChanged = true;
			}
		}

		private bool _nicknameChanged = false;
		private String _nickname;
		public String nickname {
			get { return _nickname; }
			set {
				_nickname = value;
				_nicknameChanged = true;
			}
		}

		private bool _titleChanged = false;
		private String _title;
		public String title {
			get { return _title; }
			set {
				_title = value;
				_titleChanged = true;
			}
		}

		private bool _email1Changed = false;
		private String _email1;
		public String email1 {
			get { return _email1; }
			set {
				_email1 = value;
				_email1Changed = true;
			}
		}

		private bool _email2Changed = false;
		private String _email2;
		public String email2 {
			get { return _email2; }
			set {
				_email2 = value;
				_email2Changed = true;
			}
		}

		private bool _websiteChanged = false;
		private String _website;
		public String website {
			get { return _website; }
			set {
				_website = value;
				_websiteChanged = true;
			}
		}

		private bool _phone_workChanged = false;
		private String _phone_work;
		public String phone_work {
			get { return _phone_work; }
			set {
				_phone_work = value;
				_phone_workChanged = true;
			}
		}

		private bool _phone_homeChanged = false;
		private String _phone_home;
		public String phone_home {
			get { return _phone_home; }
			set {
				_phone_home = value;
				_phone_homeChanged = true;
			}
		}

		private bool _phone_cellChanged = false;
		private String _phone_cell;
		public String phone_cell {
			get { return _phone_cell; }
			set {
				_phone_cell = value;
				_phone_cellChanged = true;
			}
		}

		private bool _faxChanged = false;
		private String _fax;
		public String fax {
			get { return _fax; }
			set {
				_fax = value;
				_faxChanged = true;
			}
		}

		private bool _notesChanged = false;
		private String _notes;
		/// <summary>
		/// Additional notes about this contact.
		/// </summary>
		public String notes {
			get { return _notes; }
			set {
				_notes = value;
				_notesChanged = true;
			}
		}

		public Contacts() : this(null, null) { }

		public Contacts(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "id": _id = reader.GetInt32(i); break;
					case "Addresses_id": _Addresses_id = (reader.IsDBNull(i)) ? default(Int32) : reader.GetInt32(i); break;
					case "Companies_id": _Companies_id = (reader.IsDBNull(i)) ? default(Int32) : reader.GetInt32(i); break;
					case "first_name": _first_name = reader.GetValue(i) as String; break;
					case "last_name": _last_name = reader.GetValue(i) as String; break;
					case "display": _display = reader.GetValue(i) as String; break;
					case "nickname": _nickname = reader.GetValue(i) as String; break;
					case "title": _title = reader.GetValue(i) as String; break;
					case "email1": _email1 = reader.GetValue(i) as String; break;
					case "email2": _email2 = reader.GetValue(i) as String; break;
					case "website": _website = reader.GetValue(i) as String; break;
					case "phone_work": _phone_work = reader.GetValue(i) as String; break;
					case "phone_home": _phone_home = reader.GetValue(i) as String; break;
					case "phone_cell": _phone_cell = reader.GetValue(i) as String; break;
					case "fax": _fax = reader.GetValue(i) as String; break;
					case "notes": _notes = reader.GetValue(i) as String; break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_Addresses_idChanged)
				changed.Add("Addresses_id", _Addresses_id);
			if (_Companies_idChanged)
				changed.Add("Companies_id", _Companies_id);
			if (_first_nameChanged)
				changed.Add("first_name", _first_name);
			if (_last_nameChanged)
				changed.Add("last_name", _last_name);
			if (_displayChanged)
				changed.Add("display", _display);
			if (_nicknameChanged)
				changed.Add("nickname", _nickname);
			if (_titleChanged)
				changed.Add("title", _title);
			if (_email1Changed)
				changed.Add("email1", _email1);
			if (_email2Changed)
				changed.Add("email2", _email2);
			if (_websiteChanged)
				changed.Add("website", _website);
			if (_phone_workChanged)
				changed.Add("phone_work", _phone_work);
			if (_phone_homeChanged)
				changed.Add("phone_home", _phone_home);
			if (_phone_cellChanged)
				changed.Add("phone_cell", _phone_cell);
			if (_faxChanged)
				changed.Add("fax", _fax);
			if (_notesChanged)
				changed.Add("notes", _notes);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_Addresses_id,
				_Companies_id,
				_first_name,
				_last_name,
				_display,
				_nickname,
				_title,
				_email1,
				_email2,
				_website,
				_phone_work,
				_phone_home,
				_phone_cell,
				_fax,
				_notes,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"Addresses_id",
				"Companies_id",
				"first_name",
				"last_name",
				"display",
				"nickname",
				"title",
				"email1",
				"email2",
				"website",
				"phone_work",
				"phone_home",
				"phone_cell",
				"fax",
				"notes",
			};
		}

		public override string GetPKName() {
			return "id";
		}

		public override object GetPKValue() {
			return _id;
		}

	}

	[TableAttribute(Name = "Messages")]
	public partial class Messages : Model {
		private bool _idChanged = false;
		private Int32 _id;
		public Int32 id {
			get { return _id; }
			set {
				_id = value;
				_idChanged = true;
			}
		}

		private bool _Users_idChanged = false;
		private Int32 _Users_id;
		public Int32 Users_id {
			get { return _Users_id; }
			set {
				_Users_id = value;
				_Users_idChanged = true;
			}
		}

		private bool _Projects_idChanged = false;
		private Int32 _Projects_id;
		public Int32 Projects_id {
			get { return _Projects_id; }
			set {
				_Projects_id = value;
				_Projects_idChanged = true;
			}
		}

		private bool _Contacts_id_toChanged = false;
		private Int32 _Contacts_id_to;
		public Int32 Contacts_id_to {
			get { return _Contacts_id_to; }
			set {
				_Contacts_id_to = value;
				_Contacts_id_toChanged = true;
			}
		}

		private bool _Contacts_id_fromChanged = false;
		private Int32 _Contacts_id_from;
		public Int32 Contacts_id_from {
			get { return _Contacts_id_from; }
			set {
				_Contacts_id_from = value;
				_Contacts_id_fromChanged = true;
			}
		}

		private bool _MessageChanged = false;
		private String _Message;
		public String Message {
			get { return _Message; }
			set {
				_Message = value;
				_MessageChanged = true;
			}
		}

		private bool _typeChanged = false;
		private Int32 _type;
		/// <summary>
		/// In, Out, Internal
		/// </summary>
		public Int32 type {
			get { return _type; }
			set {
				_type = value;
				_typeChanged = true;
			}
		}

		public Messages() : this(null, null) { }

		public Messages(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "id": _id = reader.GetInt32(i); break;
					case "Users_id": _Users_id = reader.GetInt32(i); break;
					case "Projects_id": _Projects_id = reader.GetInt32(i); break;
					case "Contacts_id_to": _Contacts_id_to = reader.GetInt32(i); break;
					case "Contacts_id_from": _Contacts_id_from = reader.GetInt32(i); break;
					case "Message": _Message = reader.GetValue(i) as String; break;
					case "type": _type = (reader.IsDBNull(i)) ? default(Int32) : reader.GetInt32(i); break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_Users_idChanged)
				changed.Add("Users_id", _Users_id);
			if (_Projects_idChanged)
				changed.Add("Projects_id", _Projects_id);
			if (_Contacts_id_toChanged)
				changed.Add("Contacts_id_to", _Contacts_id_to);
			if (_Contacts_id_fromChanged)
				changed.Add("Contacts_id_from", _Contacts_id_from);
			if (_MessageChanged)
				changed.Add("Message", _Message);
			if (_typeChanged)
				changed.Add("type", _type);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_Users_id,
				_Projects_id,
				_Contacts_id_to,
				_Contacts_id_from,
				_Message,
				_type,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"Users_id",
				"Projects_id",
				"Contacts_id_to",
				"Contacts_id_from",
				"Message",
				"type",
			};
		}

		public override string GetPKName() {
			return "id";
		}

		public override object GetPKValue() {
			return _id;
		}

	}

	[TableAttribute(Name = "ProjectsContactsList")]
	public partial class ProjectsContactsList : Model {
		private bool _idChanged = false;
		private Int32 _id;
		public Int32 id {
			get { return _id; }
			set {
				_id = value;
				_idChanged = true;
			}
		}

		private bool _typeChanged = false;
		private String _type;
		/// <summary>
		/// Name of this relationship. (GC, PC, Architect, Civil, etc...)
		/// </summary>
		public String type {
			get { return _type; }
			set {
				_type = value;
				_typeChanged = true;
			}
		}

		private bool _Projects_idChanged = false;
		private Int32 _Projects_id;
		public Int32 Projects_id {
			get { return _Projects_id; }
			set {
				_Projects_id = value;
				_Projects_idChanged = true;
			}
		}

		private bool _Contacts_idChanged = false;
		private Int32 _Contacts_id;
		public Int32 Contacts_id {
			get { return _Contacts_id; }
			set {
				_Contacts_id = value;
				_Contacts_idChanged = true;
			}
		}

		public ProjectsContactsList() : this(null, null) { }

		public ProjectsContactsList(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "id": _id = reader.GetInt32(i); break;
					case "type": _type = reader.GetValue(i) as String; break;
					case "Projects_id": _Projects_id = reader.GetInt32(i); break;
					case "Contacts_id": _Contacts_id = reader.GetInt32(i); break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_typeChanged)
				changed.Add("type", _type);
			if (_Projects_idChanged)
				changed.Add("Projects_id", _Projects_id);
			if (_Contacts_idChanged)
				changed.Add("Contacts_id", _Contacts_id);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_type,
				_Projects_id,
				_Contacts_id,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"type",
				"Projects_id",
				"Contacts_id",
			};
		}

		public override string GetPKName() {
			return "id";
		}

		public override object GetPKValue() {
			return _id;
		}

	}

	[TableAttribute(Name = "Addresses")]
	public partial class Addresses : Model {
		private bool _idChanged = false;
		private Int32 _id;
		public Int32 id {
			get { return _id; }
			set {
				_id = value;
				_idChanged = true;
			}
		}

		private bool _addressChanged = false;
		private String _address;
		public String address {
			get { return _address; }
			set {
				_address = value;
				_addressChanged = true;
			}
		}

		private bool _address2Changed = false;
		private String _address2;
		public String address2 {
			get { return _address2; }
			set {
				_address2 = value;
				_address2Changed = true;
			}
		}

		private bool _cityChanged = false;
		private String _city;
		public String city {
			get { return _city; }
			set {
				_city = value;
				_cityChanged = true;
			}
		}

		private bool _stateChanged = false;
		private String _state;
		public String state {
			get { return _state; }
			set {
				_state = value;
				_stateChanged = true;
			}
		}

		private bool _zipChanged = false;
		private String _zip;
		public String zip {
			get { return _zip; }
			set {
				_zip = value;
				_zipChanged = true;
			}
		}

		public Addresses() : this(null, null) { }

		public Addresses(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "id": _id = reader.GetInt32(i); break;
					case "address": _address = reader.GetValue(i) as String; break;
					case "address2": _address2 = reader.GetValue(i) as String; break;
					case "city": _city = reader.GetValue(i) as String; break;
					case "state": _state = reader.GetValue(i) as String; break;
					case "zip": _zip = reader.GetValue(i) as String; break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_addressChanged)
				changed.Add("address", _address);
			if (_address2Changed)
				changed.Add("address2", _address2);
			if (_cityChanged)
				changed.Add("city", _city);
			if (_stateChanged)
				changed.Add("state", _state);
			if (_zipChanged)
				changed.Add("zip", _zip);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_address,
				_address2,
				_city,
				_state,
				_zip,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"address",
				"address2",
				"city",
				"state",
				"zip",
			};
		}

		public override string GetPKName() {
			return "id";
		}

		public override object GetPKValue() {
			return _id;
		}

	}

	[TableAttribute(Name = "ConstructionDetails")]
	public partial class ConstructionDetails : Model {
		private bool _idChanged = false;
		private Int32 _id;
		public Int32 id {
			get { return _id; }
			set {
				_id = value;
				_idChanged = true;
			}
		}

		private bool _Projects_idChanged = false;
		private Int32 _Projects_id;
		public Int32 Projects_id {
			get { return _Projects_id; }
			set {
				_Projects_id = value;
				_Projects_idChanged = true;
			}
		}

		private bool _Users_idChanged = false;
		private Int32 _Users_id;
		public Int32 Users_id {
			get { return _Users_id; }
			set {
				_Users_id = value;
				_Users_idChanged = true;
			}
		}

		private bool _dateChanged = false;
		private Int64 _date;
		public Int64 date {
			get { return _date; }
			set {
				_date = value;
				_dateChanged = true;
			}
		}

		private bool _typeChanged = false;
		private String _type;
		public String type {
			get { return _type; }
			set {
				_type = value;
				_typeChanged = true;
			}
		}

		private bool _value_stringChanged = false;
		private String _value_string;
		public String value_string {
			get { return _value_string; }
			set {
				_value_string = value;
				_value_stringChanged = true;
			}
		}

		private bool _value_booleanChanged = false;
		private Boolean _value_boolean;
		/// <summary>
		/// If this is a true or false detail, this replaces the value.
		/// </summary>
		public Boolean value_boolean {
			get { return _value_boolean; }
			set {
				_value_boolean = value;
				_value_booleanChanged = true;
			}
		}

		private bool _noteChanged = false;
		private String _note;
		public String note {
			get { return _note; }
			set {
				_note = value;
				_noteChanged = true;
			}
		}

		public ConstructionDetails() : this(null, null) { }

		public ConstructionDetails(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "id": _id = reader.GetInt32(i); break;
					case "Projects_id": _Projects_id = reader.GetInt32(i); break;
					case "Users_id": _Users_id = reader.GetInt32(i); break;
					case "date": _date = reader.GetInt64(i); break;
					case "type": _type = reader.GetValue(i) as String; break;
					case "value_string": _value_string = reader.GetValue(i) as String; break;
					case "value_boolean": _value_boolean = (reader.IsDBNull(i)) ? default(Boolean) : reader.GetBoolean(i); break;
					case "note": _note = reader.GetValue(i) as String; break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_Projects_idChanged)
				changed.Add("Projects_id", _Projects_id);
			if (_Users_idChanged)
				changed.Add("Users_id", _Users_id);
			if (_dateChanged)
				changed.Add("date", _date);
			if (_typeChanged)
				changed.Add("type", _type);
			if (_value_stringChanged)
				changed.Add("value_string", _value_string);
			if (_value_booleanChanged)
				changed.Add("value_boolean", _value_boolean);
			if (_noteChanged)
				changed.Add("note", _note);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_Projects_id,
				_Users_id,
				_date,
				_type,
				_value_string,
				_value_boolean,
				_note,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"Projects_id",
				"Users_id",
				"date",
				"type",
				"value_string",
				"value_boolean",
				"note",
			};
		}

		public override string GetPKName() {
			return "id";
		}

		public override object GetPKValue() {
			return _id;
		}

	}

	[TableAttribute(Name = "ProjectTasks")]
	public partial class ProjectTasks : Model {
		private bool _idChanged = false;
		private Int32 _id;
		public Int32 id {
			get { return _id; }
			set {
				_id = value;
				_idChanged = true;
			}
		}

		private bool _Projects_idChanged = false;
		private Int32 _Projects_id;
		public Int32 Projects_id {
			get { return _Projects_id; }
			set {
				_Projects_id = value;
				_Projects_idChanged = true;
			}
		}

		private bool _ProjectTaskTypes_idChanged = false;
		private Int32 _ProjectTaskTypes_id;
		public Int32 ProjectTaskTypes_id {
			get { return _ProjectTaskTypes_id; }
			set {
				_ProjectTaskTypes_id = value;
				_ProjectTaskTypes_idChanged = true;
			}
		}

		private bool _ProjectTaskStatuses_idChanged = false;
		private Int32 _ProjectTaskStatuses_id;
		public Int32 ProjectTaskStatuses_id {
			get { return _ProjectTaskStatuses_id; }
			set {
				_ProjectTaskStatuses_id = value;
				_ProjectTaskStatuses_idChanged = true;
			}
		}

		private bool _priorityChanged = false;
		private Int16 _priority;
		public Int16 priority {
			get { return _priority; }
			set {
				_priority = value;
				_priorityChanged = true;
			}
		}

		private bool _statusChanged = false;
		private Boolean _status;
		public Boolean status {
			get { return _status; }
			set {
				_status = value;
				_statusChanged = true;
			}
		}

		private bool _taskChanged = false;
		private String _task;
		public String task {
			get { return _task; }
			set {
				_task = value;
				_taskChanged = true;
			}
		}

		private bool _date_addedChanged = false;
		private Int64 _date_added;
		public Int64 date_added {
			get { return _date_added; }
			set {
				_date_added = value;
				_date_addedChanged = true;
			}
		}

		private bool _date_completedChanged = false;
		private Int64 _date_completed;
		public Int64 date_completed {
			get { return _date_completed; }
			set {
				_date_completed = value;
				_date_completedChanged = true;
			}
		}

		public ProjectTasks() : this(null, null) { }

		public ProjectTasks(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "id": _id = reader.GetInt32(i); break;
					case "Projects_id": _Projects_id = reader.GetInt32(i); break;
					case "ProjectTaskTypes_id": _ProjectTaskTypes_id = reader.GetInt32(i); break;
					case "ProjectTaskStatuses_id": _ProjectTaskStatuses_id = reader.GetInt32(i); break;
					case "priority": _priority = (reader.IsDBNull(i)) ? default(Int16) : reader.GetInt16(i); break;
					case "status": _status = (reader.IsDBNull(i)) ? default(Boolean) : reader.GetBoolean(i); break;
					case "task": _task = reader.GetValue(i) as String; break;
					case "date_added": _date_added = reader.GetInt64(i); break;
					case "date_completed": _date_completed = (reader.IsDBNull(i)) ? default(Int64) : reader.GetInt64(i); break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_Projects_idChanged)
				changed.Add("Projects_id", _Projects_id);
			if (_ProjectTaskTypes_idChanged)
				changed.Add("ProjectTaskTypes_id", _ProjectTaskTypes_id);
			if (_ProjectTaskStatuses_idChanged)
				changed.Add("ProjectTaskStatuses_id", _ProjectTaskStatuses_id);
			if (_priorityChanged)
				changed.Add("priority", _priority);
			if (_statusChanged)
				changed.Add("status", _status);
			if (_taskChanged)
				changed.Add("task", _task);
			if (_date_addedChanged)
				changed.Add("date_added", _date_added);
			if (_date_completedChanged)
				changed.Add("date_completed", _date_completed);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_Projects_id,
				_ProjectTaskTypes_id,
				_ProjectTaskStatuses_id,
				_priority,
				_status,
				_task,
				_date_added,
				_date_completed,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"Projects_id",
				"ProjectTaskTypes_id",
				"ProjectTaskStatuses_id",
				"priority",
				"status",
				"task",
				"date_added",
				"date_completed",
			};
		}

		public override string GetPKName() {
			return "id";
		}

		public override object GetPKValue() {
			return _id;
		}

	}

	[TableAttribute(Name = "ProjectTaskTypes")]
	public partial class ProjectTaskTypes : Model {
		private bool _idChanged = false;
		private Int32 _id;
		public Int32 id {
			get { return _id; }
			set {
				_id = value;
				_idChanged = true;
			}
		}

		private bool _typeChanged = false;
		private String _type;
		public String type {
			get { return _type; }
			set {
				_type = value;
				_typeChanged = true;
			}
		}

		public ProjectTaskTypes() : this(null, null) { }

		public ProjectTaskTypes(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "id": _id = reader.GetInt32(i); break;
					case "type": _type = reader.GetValue(i) as String; break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_typeChanged)
				changed.Add("type", _type);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_type,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"type",
			};
		}

		public override string GetPKName() {
			return "id";
		}

		public override object GetPKValue() {
			return _id;
		}

	}

	[TableAttribute(Name = "ProjectTaskStatuses")]
	public partial class ProjectTaskStatuses : Model {
		private bool _idChanged = false;
		private Int32 _id;
		public Int32 id {
			get { return _id; }
			set {
				_id = value;
				_idChanged = true;
			}
		}

		private bool _statusChanged = false;
		private String _status;
		public String status {
			get { return _status; }
			set {
				_status = value;
				_statusChanged = true;
			}
		}

		public ProjectTaskStatuses() : this(null, null) { }

		public ProjectTaskStatuses(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "id": _id = reader.GetInt32(i); break;
					case "status": _status = reader.GetValue(i) as String; break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_statusChanged)
				changed.Add("status", _status);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_status,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"status",
			};
		}

		public override string GetPKName() {
			return "id";
		}

		public override object GetPKValue() {
			return _id;
		}

	}

	[TableAttribute(Name = "ProjectTasksUsersInvolved")]
	public partial class ProjectTasksUsersInvolved : Model {
		private bool _idChanged = false;
		private Int32 _id;
		public Int32 id {
			get { return _id; }
			set {
				_id = value;
				_idChanged = true;
			}
		}

		private bool _Users_idChanged = false;
		private Int32 _Users_id;
		public Int32 Users_id {
			get { return _Users_id; }
			set {
				_Users_id = value;
				_Users_idChanged = true;
			}
		}

		private bool _ProjectTasks_idChanged = false;
		private Int32 _ProjectTasks_id;
		public Int32 ProjectTasks_id {
			get { return _ProjectTasks_id; }
			set {
				_ProjectTasks_id = value;
				_ProjectTasks_idChanged = true;
			}
		}

		public ProjectTasksUsersInvolved() : this(null, null) { }

		public ProjectTasksUsersInvolved(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "id": _id = reader.GetInt32(i); break;
					case "Users_id": _Users_id = reader.GetInt32(i); break;
					case "ProjectTasks_id": _ProjectTasks_id = reader.GetInt32(i); break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_Users_idChanged)
				changed.Add("Users_id", _Users_id);
			if (_ProjectTasks_idChanged)
				changed.Add("ProjectTasks_id", _ProjectTasks_id);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_Users_id,
				_ProjectTasks_id,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"Users_id",
				"ProjectTasks_id",
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