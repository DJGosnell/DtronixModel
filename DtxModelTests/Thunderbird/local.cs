using System;
using System.Data.Common;
using System.Collections.Generic;
using DtxModel;

namespace DtxModelTests.Models {

	public partial class ThunderbirdCalendarContext : Context {
		private static Func<DbConnection> _default_connection = null;

		/// <summary>
		/// Set a default constructor to allow use of parameterless context calling.
		/// </summary>
		public static Func<DbConnection> DefaultConnection {
			get { return _default_connection; }
			set { _default_connection = value; }
		}

		private Table<cal_calendar_schema_version> _cal_calendar_schema_version;

		public Table<cal_calendar_schema_version> cal_calendar_schema_version {
			get {
				if(_cal_calendar_schema_version == null) {
					_cal_calendar_schema_version = new Table<cal_calendar_schema_version>(this);
				}

				return _cal_calendar_schema_version;
			}
		}

		private Table<cal_properties> _cal_properties;

		public Table<cal_properties> cal_properties {
			get {
				if(_cal_properties == null) {
					_cal_properties = new Table<cal_properties>(this);
				}

				return _cal_properties;
			}
		}

		private Table<cal_events> _cal_events;

		public Table<cal_events> cal_events {
			get {
				if(_cal_events == null) {
					_cal_events = new Table<cal_events>(this);
				}

				return _cal_events;
			}
		}

		private Table<cal_todos> _cal_todos;

		public Table<cal_todos> cal_todos {
			get {
				if(_cal_todos == null) {
					_cal_todos = new Table<cal_todos>(this);
				}

				return _cal_todos;
			}
		}

		private Table<cal_tz_version> _cal_tz_version;

		public Table<cal_tz_version> cal_tz_version {
			get {
				if(_cal_tz_version == null) {
					_cal_tz_version = new Table<cal_tz_version>(this);
				}

				return _cal_tz_version;
			}
		}

		private Table<cal_metadata> _cal_metadata;

		public Table<cal_metadata> cal_metadata {
			get {
				if(_cal_metadata == null) {
					_cal_metadata = new Table<cal_metadata>(this);
				}

				return _cal_metadata;
			}
		}

		private Table<cal_alarms> _cal_alarms;

		public Table<cal_alarms> cal_alarms {
			get {
				if(_cal_alarms == null) {
					_cal_alarms = new Table<cal_alarms>(this);
				}

				return _cal_alarms;
			}
		}

		private Table<cal_attachments> _cal_attachments;

		public Table<cal_attachments> cal_attachments {
			get {
				if(_cal_attachments == null) {
					_cal_attachments = new Table<cal_attachments>(this);
				}

				return _cal_attachments;
			}
		}

		private Table<cal_relations> _cal_relations;

		public Table<cal_relations> cal_relations {
			get {
				if(_cal_relations == null) {
					_cal_relations = new Table<cal_relations>(this);
				}

				return _cal_relations;
			}
		}

		private Table<cal_attendees> _cal_attendees;

		public Table<cal_attendees> cal_attendees {
			get {
				if(_cal_attendees == null) {
					_cal_attendees = new Table<cal_attendees>(this);
				}

				return _cal_attendees;
			}
		}

		private Table<cal_recurrence> _cal_recurrence;

		public Table<cal_recurrence> cal_recurrence {
			get {
				if(_cal_recurrence == null) {
					_cal_recurrence = new Table<cal_recurrence>(this);
				}

				return _cal_recurrence;
			}
		}

		/// <summary>
		/// Create a new context of this database's type.  Can only be used if a default connection is specified.
		/// </summary>
		public ThunderbirdCalendarContext() : base(_default_connection) { }

		/// <summary>
		/// Create a new context of this database's type with a specific connection.
		/// </summary>
		/// <param name="connection">Existing open database connection to use.</param>
		public ThunderbirdCalendarContext(DbConnection connection) : base(connection) { }
	}

	[TableAttribute(Name = "cal_calendar_schema_version")]
	public partial class cal_calendar_schema_version : Model {
		private Int64 _rowid;
		/// <summary>
		/// Auto generated SQLite rowid column.
		/// </summary>
		public Int64 rowid {
			get { return _rowid; }
		}

		private bool _versionChanged = false;
		private Int64 _version;
		public Int64 version {
			get { return _version; }
			set {
				_version = value;
				_versionChanged = true;
			}
		}

		public cal_calendar_schema_version() : this(null, null) { }

		public cal_calendar_schema_version(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "rowid": _rowid = (reader.IsDBNull(i)) ? default(Int64) : reader.GetFieldValue<Int64>(i); break;
					case "version": _version = (reader.IsDBNull(i)) ? default(Int64) : reader.GetFieldValue<Int64>(i); break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_versionChanged)
				changed.Add("version", _version);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_version,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"version",
			};
		}

		public override string GetPKName() {
			return "rowid";
		}

		public override object GetPKValue() {
			return _rowid;
		}

	}

	[TableAttribute(Name = "cal_properties")]
	public partial class cal_properties : Model {
		private Int64 _rowid;
		/// <summary>
		/// Auto generated SQLite rowid column.
		/// </summary>
		public Int64 rowid {
			get { return _rowid; }
		}

		private bool _item_idChanged = false;
		private String _item_id;
		public String item_id {
			get { return _item_id; }
			set {
				_item_id = value;
				_item_idChanged = true;
			}
		}

		private bool _keyChanged = false;
		private String _key;
		public String key {
			get { return _key; }
			set {
				_key = value;
				_keyChanged = true;
			}
		}

		private bool _valueChanged = false;
		private byte[] _value;
		public byte[] value {
			get { return _value; }
			set {
				_value = value;
				_valueChanged = true;
			}
		}

		private bool _recurrence_idChanged = false;
		private Int64 _recurrence_id;
		public Int64 recurrence_id {
			get { return _recurrence_id; }
			set {
				_recurrence_id = value;
				_recurrence_idChanged = true;
			}
		}

		private bool _recurrence_id_tzChanged = false;
		private String _recurrence_id_tz;
		public String recurrence_id_tz {
			get { return _recurrence_id_tz; }
			set {
				_recurrence_id_tz = value;
				_recurrence_id_tzChanged = true;
			}
		}

		private bool _cal_idChanged = false;
		private String _cal_id;
		public String cal_id {
			get { return _cal_id; }
			set {
				_cal_id = value;
				_cal_idChanged = true;
			}
		}

		public cal_properties() : this(null, null) { }

		public cal_properties(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "rowid": _rowid = (reader.IsDBNull(i)) ? default(Int64) : reader.GetFieldValue<Int64>(i); break;
					case "item_id": _item_id = reader.GetValue(i) as String; break;
					case "key": _key = reader.GetValue(i) as String; break;
					case "value": _value = reader.GetValue(i) as Byte[]; break;
					case "recurrence_id": _recurrence_id = (reader.IsDBNull(i)) ? default(Int64) : reader.GetFieldValue<Int64>(i); break;
					case "recurrence_id_tz": _recurrence_id_tz = reader.GetValue(i) as String; break;
					case "cal_id": _cal_id = reader.GetValue(i) as String; break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_item_idChanged)
				changed.Add("item_id", _item_id);
			if (_keyChanged)
				changed.Add("key", _key);
			if (_valueChanged)
				changed.Add("value", _value);
			if (_recurrence_idChanged)
				changed.Add("recurrence_id", _recurrence_id);
			if (_recurrence_id_tzChanged)
				changed.Add("recurrence_id_tz", _recurrence_id_tz);
			if (_cal_idChanged)
				changed.Add("cal_id", _cal_id);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_item_id,
				_key,
				_value,
				_recurrence_id,
				_recurrence_id_tz,
				_cal_id,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"item_id",
				"key",
				"value",
				"recurrence_id",
				"recurrence_id_tz",
				"cal_id",
			};
		}

		public override string GetPKName() {
			return "rowid";
		}

		public override object GetPKValue() {
			return _rowid;
		}

	}

	[TableAttribute(Name = "cal_events")]
	public partial class cal_events : Model {
		private Int64 _rowid;
		/// <summary>
		/// Auto generated SQLite rowid column.
		/// </summary>
		public Int64 rowid {
			get { return _rowid; }
		}

		private bool _cal_idChanged = false;
		private String _cal_id;
		public String cal_id {
			get { return _cal_id; }
			set {
				_cal_id = value;
				_cal_idChanged = true;
			}
		}

		private bool _idChanged = false;
		private String _id;
		public String id {
			get { return _id; }
			set {
				_id = value;
				_idChanged = true;
			}
		}

		private bool _time_createdChanged = false;
		private Int64 _time_created;
		public Int64 time_created {
			get { return _time_created; }
			set {
				_time_created = value;
				_time_createdChanged = true;
			}
		}

		private bool _last_modifiedChanged = false;
		private Int64 _last_modified;
		public Int64 last_modified {
			get { return _last_modified; }
			set {
				_last_modified = value;
				_last_modifiedChanged = true;
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

		private bool _priorityChanged = false;
		private Int64 _priority;
		public Int64 priority {
			get { return _priority; }
			set {
				_priority = value;
				_priorityChanged = true;
			}
		}

		private bool _privacyChanged = false;
		private String _privacy;
		public String privacy {
			get { return _privacy; }
			set {
				_privacy = value;
				_privacyChanged = true;
			}
		}

		private bool _ical_statusChanged = false;
		private String _ical_status;
		public String ical_status {
			get { return _ical_status; }
			set {
				_ical_status = value;
				_ical_statusChanged = true;
			}
		}

		private bool _flagsChanged = false;
		private Int64 _flags;
		public Int64 flags {
			get { return _flags; }
			set {
				_flags = value;
				_flagsChanged = true;
			}
		}

		private bool _event_startChanged = false;
		private Int64 _event_start;
		public Int64 event_start {
			get { return _event_start; }
			set {
				_event_start = value;
				_event_startChanged = true;
			}
		}

		private bool _event_endChanged = false;
		private Int64 _event_end;
		public Int64 event_end {
			get { return _event_end; }
			set {
				_event_end = value;
				_event_endChanged = true;
			}
		}

		private bool _event_stampChanged = false;
		private Int64 _event_stamp;
		public Int64 event_stamp {
			get { return _event_stamp; }
			set {
				_event_stamp = value;
				_event_stampChanged = true;
			}
		}

		private bool _event_start_tzChanged = false;
		private String _event_start_tz;
		public String event_start_tz {
			get { return _event_start_tz; }
			set {
				_event_start_tz = value;
				_event_start_tzChanged = true;
			}
		}

		private bool _event_end_tzChanged = false;
		private String _event_end_tz;
		public String event_end_tz {
			get { return _event_end_tz; }
			set {
				_event_end_tz = value;
				_event_end_tzChanged = true;
			}
		}

		private bool _recurrence_idChanged = false;
		private Int64 _recurrence_id;
		public Int64 recurrence_id {
			get { return _recurrence_id; }
			set {
				_recurrence_id = value;
				_recurrence_idChanged = true;
			}
		}

		private bool _recurrence_id_tzChanged = false;
		private String _recurrence_id_tz;
		public String recurrence_id_tz {
			get { return _recurrence_id_tz; }
			set {
				_recurrence_id_tz = value;
				_recurrence_id_tzChanged = true;
			}
		}

		private bool _alarm_last_ackChanged = false;
		private Int64 _alarm_last_ack;
		public Int64 alarm_last_ack {
			get { return _alarm_last_ack; }
			set {
				_alarm_last_ack = value;
				_alarm_last_ackChanged = true;
			}
		}

		private bool _offline_journalChanged = false;
		private Int64 _offline_journal;
		public Int64 offline_journal {
			get { return _offline_journal; }
			set {
				_offline_journal = value;
				_offline_journalChanged = true;
			}
		}

		public cal_events() : this(null, null) { }

		public cal_events(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "rowid": _rowid = (reader.IsDBNull(i)) ? default(Int64) : reader.GetFieldValue<Int64>(i); break;
					case "cal_id": _cal_id = reader.GetValue(i) as String; break;
					case "id": _id = reader.GetValue(i) as String; break;
					case "time_created": _time_created = (reader.IsDBNull(i)) ? default(Int64) : reader.GetFieldValue<Int64>(i); break;
					case "last_modified": _last_modified = (reader.IsDBNull(i)) ? default(Int64) : reader.GetFieldValue<Int64>(i); break;
					case "title": _title = reader.GetValue(i) as String; break;
					case "priority": _priority = (reader.IsDBNull(i)) ? default(Int64) : reader.GetFieldValue<Int64>(i); break;
					case "privacy": _privacy = reader.GetValue(i) as String; break;
					case "ical_status": _ical_status = reader.GetValue(i) as String; break;
					case "flags": _flags = (reader.IsDBNull(i)) ? default(Int64) : reader.GetFieldValue<Int64>(i); break;
					case "event_start": _event_start = (reader.IsDBNull(i)) ? default(Int64) : reader.GetFieldValue<Int64>(i); break;
					case "event_end": _event_end = (reader.IsDBNull(i)) ? default(Int64) : reader.GetFieldValue<Int64>(i); break;
					case "event_stamp": _event_stamp = (reader.IsDBNull(i)) ? default(Int64) : reader.GetFieldValue<Int64>(i); break;
					case "event_start_tz": _event_start_tz = reader.GetValue(i) as String; break;
					case "event_end_tz": _event_end_tz = reader.GetValue(i) as String; break;
					case "recurrence_id": _recurrence_id = (reader.IsDBNull(i)) ? default(Int64) : reader.GetFieldValue<Int64>(i); break;
					case "recurrence_id_tz": _recurrence_id_tz = reader.GetValue(i) as String; break;
					case "alarm_last_ack": _alarm_last_ack = (reader.IsDBNull(i)) ? default(Int64) : reader.GetFieldValue<Int64>(i); break;
					case "offline_journal": _offline_journal = (reader.IsDBNull(i)) ? default(Int64) : reader.GetFieldValue<Int64>(i); break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_cal_idChanged)
				changed.Add("cal_id", _cal_id);
			if (_idChanged)
				changed.Add("id", _id);
			if (_time_createdChanged)
				changed.Add("time_created", _time_created);
			if (_last_modifiedChanged)
				changed.Add("last_modified", _last_modified);
			if (_titleChanged)
				changed.Add("title", _title);
			if (_priorityChanged)
				changed.Add("priority", _priority);
			if (_privacyChanged)
				changed.Add("privacy", _privacy);
			if (_ical_statusChanged)
				changed.Add("ical_status", _ical_status);
			if (_flagsChanged)
				changed.Add("flags", _flags);
			if (_event_startChanged)
				changed.Add("event_start", _event_start);
			if (_event_endChanged)
				changed.Add("event_end", _event_end);
			if (_event_stampChanged)
				changed.Add("event_stamp", _event_stamp);
			if (_event_start_tzChanged)
				changed.Add("event_start_tz", _event_start_tz);
			if (_event_end_tzChanged)
				changed.Add("event_end_tz", _event_end_tz);
			if (_recurrence_idChanged)
				changed.Add("recurrence_id", _recurrence_id);
			if (_recurrence_id_tzChanged)
				changed.Add("recurrence_id_tz", _recurrence_id_tz);
			if (_alarm_last_ackChanged)
				changed.Add("alarm_last_ack", _alarm_last_ack);
			if (_offline_journalChanged)
				changed.Add("offline_journal", _offline_journal);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_cal_id,
				_id,
				_time_created,
				_last_modified,
				_title,
				_priority,
				_privacy,
				_ical_status,
				_flags,
				_event_start,
				_event_end,
				_event_stamp,
				_event_start_tz,
				_event_end_tz,
				_recurrence_id,
				_recurrence_id_tz,
				_alarm_last_ack,
				_offline_journal,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"cal_id",
				"id",
				"time_created",
				"last_modified",
				"title",
				"priority",
				"privacy",
				"ical_status",
				"flags",
				"event_start",
				"event_end",
				"event_stamp",
				"event_start_tz",
				"event_end_tz",
				"recurrence_id",
				"recurrence_id_tz",
				"alarm_last_ack",
				"offline_journal",
			};
		}

		public override string GetPKName() {
			return "rowid";
		}

		public override object GetPKValue() {
			return _rowid;
		}

	}

	[TableAttribute(Name = "cal_todos")]
	public partial class cal_todos : Model {
		private Int64 _rowid;
		/// <summary>
		/// Auto generated SQLite rowid column.
		/// </summary>
		public Int64 rowid {
			get { return _rowid; }
		}

		private bool _cal_idChanged = false;
		private String _cal_id;
		public String cal_id {
			get { return _cal_id; }
			set {
				_cal_id = value;
				_cal_idChanged = true;
			}
		}

		private bool _idChanged = false;
		private String _id;
		public String id {
			get { return _id; }
			set {
				_id = value;
				_idChanged = true;
			}
		}

		private bool _time_createdChanged = false;
		private Int64 _time_created;
		public Int64 time_created {
			get { return _time_created; }
			set {
				_time_created = value;
				_time_createdChanged = true;
			}
		}

		private bool _last_modifiedChanged = false;
		private Int64 _last_modified;
		public Int64 last_modified {
			get { return _last_modified; }
			set {
				_last_modified = value;
				_last_modifiedChanged = true;
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

		private bool _priorityChanged = false;
		private Int64 _priority;
		public Int64 priority {
			get { return _priority; }
			set {
				_priority = value;
				_priorityChanged = true;
			}
		}

		private bool _privacyChanged = false;
		private String _privacy;
		public String privacy {
			get { return _privacy; }
			set {
				_privacy = value;
				_privacyChanged = true;
			}
		}

		private bool _ical_statusChanged = false;
		private String _ical_status;
		public String ical_status {
			get { return _ical_status; }
			set {
				_ical_status = value;
				_ical_statusChanged = true;
			}
		}

		private bool _flagsChanged = false;
		private Int64 _flags;
		public Int64 flags {
			get { return _flags; }
			set {
				_flags = value;
				_flagsChanged = true;
			}
		}

		private bool _todo_entryChanged = false;
		private Int64 _todo_entry;
		public Int64 todo_entry {
			get { return _todo_entry; }
			set {
				_todo_entry = value;
				_todo_entryChanged = true;
			}
		}

		private bool _todo_dueChanged = false;
		private Int64 _todo_due;
		public Int64 todo_due {
			get { return _todo_due; }
			set {
				_todo_due = value;
				_todo_dueChanged = true;
			}
		}

		private bool _todo_completedChanged = false;
		private Int64 _todo_completed;
		public Int64 todo_completed {
			get { return _todo_completed; }
			set {
				_todo_completed = value;
				_todo_completedChanged = true;
			}
		}

		private bool _todo_completeChanged = false;
		private Int64 _todo_complete;
		public Int64 todo_complete {
			get { return _todo_complete; }
			set {
				_todo_complete = value;
				_todo_completeChanged = true;
			}
		}

		private bool _todo_entry_tzChanged = false;
		private String _todo_entry_tz;
		public String todo_entry_tz {
			get { return _todo_entry_tz; }
			set {
				_todo_entry_tz = value;
				_todo_entry_tzChanged = true;
			}
		}

		private bool _todo_due_tzChanged = false;
		private String _todo_due_tz;
		public String todo_due_tz {
			get { return _todo_due_tz; }
			set {
				_todo_due_tz = value;
				_todo_due_tzChanged = true;
			}
		}

		private bool _todo_completed_tzChanged = false;
		private String _todo_completed_tz;
		public String todo_completed_tz {
			get { return _todo_completed_tz; }
			set {
				_todo_completed_tz = value;
				_todo_completed_tzChanged = true;
			}
		}

		private bool _recurrence_idChanged = false;
		private Int64 _recurrence_id;
		public Int64 recurrence_id {
			get { return _recurrence_id; }
			set {
				_recurrence_id = value;
				_recurrence_idChanged = true;
			}
		}

		private bool _recurrence_id_tzChanged = false;
		private String _recurrence_id_tz;
		public String recurrence_id_tz {
			get { return _recurrence_id_tz; }
			set {
				_recurrence_id_tz = value;
				_recurrence_id_tzChanged = true;
			}
		}

		private bool _alarm_last_ackChanged = false;
		private Int64 _alarm_last_ack;
		public Int64 alarm_last_ack {
			get { return _alarm_last_ack; }
			set {
				_alarm_last_ack = value;
				_alarm_last_ackChanged = true;
			}
		}

		private bool _todo_stampChanged = false;
		private Int64 _todo_stamp;
		public Int64 todo_stamp {
			get { return _todo_stamp; }
			set {
				_todo_stamp = value;
				_todo_stampChanged = true;
			}
		}

		private bool _offline_journalChanged = false;
		private Int64 _offline_journal;
		public Int64 offline_journal {
			get { return _offline_journal; }
			set {
				_offline_journal = value;
				_offline_journalChanged = true;
			}
		}

		public cal_todos() : this(null, null) { }

		public cal_todos(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "rowid": _rowid = (reader.IsDBNull(i)) ? default(Int64) : reader.GetFieldValue<Int64>(i); break;
					case "cal_id": _cal_id = reader.GetValue(i) as String; break;
					case "id": _id = reader.GetValue(i) as String; break;
					case "time_created": _time_created = (reader.IsDBNull(i)) ? default(Int64) : reader.GetFieldValue<Int64>(i); break;
					case "last_modified": _last_modified = (reader.IsDBNull(i)) ? default(Int64) : reader.GetFieldValue<Int64>(i); break;
					case "title": _title = reader.GetValue(i) as String; break;
					case "priority": _priority = (reader.IsDBNull(i)) ? default(Int64) : reader.GetFieldValue<Int64>(i); break;
					case "privacy": _privacy = reader.GetValue(i) as String; break;
					case "ical_status": _ical_status = reader.GetValue(i) as String; break;
					case "flags": _flags = (reader.IsDBNull(i)) ? default(Int64) : reader.GetFieldValue<Int64>(i); break;
					case "todo_entry": _todo_entry = (reader.IsDBNull(i)) ? default(Int64) : reader.GetFieldValue<Int64>(i); break;
					case "todo_due": _todo_due = (reader.IsDBNull(i)) ? default(Int64) : reader.GetFieldValue<Int64>(i); break;
					case "todo_completed": _todo_completed = (reader.IsDBNull(i)) ? default(Int64) : reader.GetFieldValue<Int64>(i); break;
					case "todo_complete": _todo_complete = (reader.IsDBNull(i)) ? default(Int64) : reader.GetFieldValue<Int64>(i); break;
					case "todo_entry_tz": _todo_entry_tz = reader.GetValue(i) as String; break;
					case "todo_due_tz": _todo_due_tz = reader.GetValue(i) as String; break;
					case "todo_completed_tz": _todo_completed_tz = reader.GetValue(i) as String; break;
					case "recurrence_id": _recurrence_id = (reader.IsDBNull(i)) ? default(Int64) : reader.GetFieldValue<Int64>(i); break;
					case "recurrence_id_tz": _recurrence_id_tz = reader.GetValue(i) as String; break;
					case "alarm_last_ack": _alarm_last_ack = (reader.IsDBNull(i)) ? default(Int64) : reader.GetFieldValue<Int64>(i); break;
					case "todo_stamp": _todo_stamp = (reader.IsDBNull(i)) ? default(Int64) : reader.GetFieldValue<Int64>(i); break;
					case "offline_journal": _offline_journal = (reader.IsDBNull(i)) ? default(Int64) : reader.GetFieldValue<Int64>(i); break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_cal_idChanged)
				changed.Add("cal_id", _cal_id);
			if (_idChanged)
				changed.Add("id", _id);
			if (_time_createdChanged)
				changed.Add("time_created", _time_created);
			if (_last_modifiedChanged)
				changed.Add("last_modified", _last_modified);
			if (_titleChanged)
				changed.Add("title", _title);
			if (_priorityChanged)
				changed.Add("priority", _priority);
			if (_privacyChanged)
				changed.Add("privacy", _privacy);
			if (_ical_statusChanged)
				changed.Add("ical_status", _ical_status);
			if (_flagsChanged)
				changed.Add("flags", _flags);
			if (_todo_entryChanged)
				changed.Add("todo_entry", _todo_entry);
			if (_todo_dueChanged)
				changed.Add("todo_due", _todo_due);
			if (_todo_completedChanged)
				changed.Add("todo_completed", _todo_completed);
			if (_todo_completeChanged)
				changed.Add("todo_complete", _todo_complete);
			if (_todo_entry_tzChanged)
				changed.Add("todo_entry_tz", _todo_entry_tz);
			if (_todo_due_tzChanged)
				changed.Add("todo_due_tz", _todo_due_tz);
			if (_todo_completed_tzChanged)
				changed.Add("todo_completed_tz", _todo_completed_tz);
			if (_recurrence_idChanged)
				changed.Add("recurrence_id", _recurrence_id);
			if (_recurrence_id_tzChanged)
				changed.Add("recurrence_id_tz", _recurrence_id_tz);
			if (_alarm_last_ackChanged)
				changed.Add("alarm_last_ack", _alarm_last_ack);
			if (_todo_stampChanged)
				changed.Add("todo_stamp", _todo_stamp);
			if (_offline_journalChanged)
				changed.Add("offline_journal", _offline_journal);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_cal_id,
				_id,
				_time_created,
				_last_modified,
				_title,
				_priority,
				_privacy,
				_ical_status,
				_flags,
				_todo_entry,
				_todo_due,
				_todo_completed,
				_todo_complete,
				_todo_entry_tz,
				_todo_due_tz,
				_todo_completed_tz,
				_recurrence_id,
				_recurrence_id_tz,
				_alarm_last_ack,
				_todo_stamp,
				_offline_journal,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"cal_id",
				"id",
				"time_created",
				"last_modified",
				"title",
				"priority",
				"privacy",
				"ical_status",
				"flags",
				"todo_entry",
				"todo_due",
				"todo_completed",
				"todo_complete",
				"todo_entry_tz",
				"todo_due_tz",
				"todo_completed_tz",
				"recurrence_id",
				"recurrence_id_tz",
				"alarm_last_ack",
				"todo_stamp",
				"offline_journal",
			};
		}

		public override string GetPKName() {
			return "rowid";
		}

		public override object GetPKValue() {
			return _rowid;
		}

	}

	[TableAttribute(Name = "cal_tz_version")]
	public partial class cal_tz_version : Model {
		private Int64 _rowid;
		/// <summary>
		/// Auto generated SQLite rowid column.
		/// </summary>
		public Int64 rowid {
			get { return _rowid; }
		}

		private bool _versionChanged = false;
		private String _version;
		public String version {
			get { return _version; }
			set {
				_version = value;
				_versionChanged = true;
			}
		}

		public cal_tz_version() : this(null, null) { }

		public cal_tz_version(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "rowid": _rowid = (reader.IsDBNull(i)) ? default(Int64) : reader.GetFieldValue<Int64>(i); break;
					case "version": _version = reader.GetValue(i) as String; break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_versionChanged)
				changed.Add("version", _version);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_version,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"version",
			};
		}

		public override string GetPKName() {
			return "rowid";
		}

		public override object GetPKValue() {
			return _rowid;
		}

	}

	[TableAttribute(Name = "cal_metadata")]
	public partial class cal_metadata : Model {
		private Int64 _rowid;
		/// <summary>
		/// Auto generated SQLite rowid column.
		/// </summary>
		public Int64 rowid {
			get { return _rowid; }
		}

		private bool _cal_idChanged = false;
		private String _cal_id;
		public String cal_id {
			get { return _cal_id; }
			set {
				_cal_id = value;
				_cal_idChanged = true;
			}
		}

		private bool _item_idChanged = false;
		private String _item_id;
		public String item_id {
			get { return _item_id; }
			set {
				_item_id = value;
				_item_idChanged = true;
			}
		}

		private bool _valueChanged = false;
		private byte[] _value;
		public byte[] value {
			get { return _value; }
			set {
				_value = value;
				_valueChanged = true;
			}
		}

		public cal_metadata() : this(null, null) { }

		public cal_metadata(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "rowid": _rowid = (reader.IsDBNull(i)) ? default(Int64) : reader.GetFieldValue<Int64>(i); break;
					case "cal_id": _cal_id = reader.GetValue(i) as String; break;
					case "item_id": _item_id = reader.GetValue(i) as String; break;
					case "value": _value = reader.GetValue(i) as Byte[]; break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_cal_idChanged)
				changed.Add("cal_id", _cal_id);
			if (_item_idChanged)
				changed.Add("item_id", _item_id);
			if (_valueChanged)
				changed.Add("value", _value);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_cal_id,
				_item_id,
				_value,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"cal_id",
				"item_id",
				"value",
			};
		}

		public override string GetPKName() {
			return "rowid";
		}

		public override object GetPKValue() {
			return _rowid;
		}

	}

	[TableAttribute(Name = "cal_alarms")]
	public partial class cal_alarms : Model {
		private Int64 _rowid;
		/// <summary>
		/// Auto generated SQLite rowid column.
		/// </summary>
		public Int64 rowid {
			get { return _rowid; }
		}

		private bool _cal_idChanged = false;
		private String _cal_id;
		public String cal_id {
			get { return _cal_id; }
			set {
				_cal_id = value;
				_cal_idChanged = true;
			}
		}

		private bool _item_idChanged = false;
		private String _item_id;
		public String item_id {
			get { return _item_id; }
			set {
				_item_id = value;
				_item_idChanged = true;
			}
		}

		private bool _recurrence_idChanged = false;
		private Int64 _recurrence_id;
		public Int64 recurrence_id {
			get { return _recurrence_id; }
			set {
				_recurrence_id = value;
				_recurrence_idChanged = true;
			}
		}

		private bool _recurrence_id_tzChanged = false;
		private String _recurrence_id_tz;
		public String recurrence_id_tz {
			get { return _recurrence_id_tz; }
			set {
				_recurrence_id_tz = value;
				_recurrence_id_tzChanged = true;
			}
		}

		private bool _icalStringChanged = false;
		private String _icalString;
		public String icalString {
			get { return _icalString; }
			set {
				_icalString = value;
				_icalStringChanged = true;
			}
		}

		public cal_alarms() : this(null, null) { }

		public cal_alarms(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "rowid": _rowid = (reader.IsDBNull(i)) ? default(Int64) : reader.GetFieldValue<Int64>(i); break;
					case "cal_id": _cal_id = reader.GetValue(i) as String; break;
					case "item_id": _item_id = reader.GetValue(i) as String; break;
					case "recurrence_id": _recurrence_id = (reader.IsDBNull(i)) ? default(Int64) : reader.GetFieldValue<Int64>(i); break;
					case "recurrence_id_tz": _recurrence_id_tz = reader.GetValue(i) as String; break;
					case "icalString": _icalString = reader.GetValue(i) as String; break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_cal_idChanged)
				changed.Add("cal_id", _cal_id);
			if (_item_idChanged)
				changed.Add("item_id", _item_id);
			if (_recurrence_idChanged)
				changed.Add("recurrence_id", _recurrence_id);
			if (_recurrence_id_tzChanged)
				changed.Add("recurrence_id_tz", _recurrence_id_tz);
			if (_icalStringChanged)
				changed.Add("icalString", _icalString);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_cal_id,
				_item_id,
				_recurrence_id,
				_recurrence_id_tz,
				_icalString,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"cal_id",
				"item_id",
				"recurrence_id",
				"recurrence_id_tz",
				"icalString",
			};
		}

		public override string GetPKName() {
			return "rowid";
		}

		public override object GetPKValue() {
			return _rowid;
		}

	}

	[TableAttribute(Name = "cal_attachments")]
	public partial class cal_attachments : Model {
		private Int64 _rowid;
		/// <summary>
		/// Auto generated SQLite rowid column.
		/// </summary>
		public Int64 rowid {
			get { return _rowid; }
		}

		private bool _item_idChanged = false;
		private String _item_id;
		public String item_id {
			get { return _item_id; }
			set {
				_item_id = value;
				_item_idChanged = true;
			}
		}

		private bool _cal_idChanged = false;
		private String _cal_id;
		public String cal_id {
			get { return _cal_id; }
			set {
				_cal_id = value;
				_cal_idChanged = true;
			}
		}

		private bool _recurrence_idChanged = false;
		private Int64 _recurrence_id;
		public Int64 recurrence_id {
			get { return _recurrence_id; }
			set {
				_recurrence_id = value;
				_recurrence_idChanged = true;
			}
		}

		private bool _recurrence_id_tzChanged = false;
		private String _recurrence_id_tz;
		public String recurrence_id_tz {
			get { return _recurrence_id_tz; }
			set {
				_recurrence_id_tz = value;
				_recurrence_id_tzChanged = true;
			}
		}

		private bool _icalStringChanged = false;
		private String _icalString;
		public String icalString {
			get { return _icalString; }
			set {
				_icalString = value;
				_icalStringChanged = true;
			}
		}

		public cal_attachments() : this(null, null) { }

		public cal_attachments(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "rowid": _rowid = (reader.IsDBNull(i)) ? default(Int64) : reader.GetFieldValue<Int64>(i); break;
					case "item_id": _item_id = reader.GetValue(i) as String; break;
					case "cal_id": _cal_id = reader.GetValue(i) as String; break;
					case "recurrence_id": _recurrence_id = (reader.IsDBNull(i)) ? default(Int64) : reader.GetFieldValue<Int64>(i); break;
					case "recurrence_id_tz": _recurrence_id_tz = reader.GetValue(i) as String; break;
					case "icalString": _icalString = reader.GetValue(i) as String; break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_item_idChanged)
				changed.Add("item_id", _item_id);
			if (_cal_idChanged)
				changed.Add("cal_id", _cal_id);
			if (_recurrence_idChanged)
				changed.Add("recurrence_id", _recurrence_id);
			if (_recurrence_id_tzChanged)
				changed.Add("recurrence_id_tz", _recurrence_id_tz);
			if (_icalStringChanged)
				changed.Add("icalString", _icalString);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_item_id,
				_cal_id,
				_recurrence_id,
				_recurrence_id_tz,
				_icalString,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"item_id",
				"cal_id",
				"recurrence_id",
				"recurrence_id_tz",
				"icalString",
			};
		}

		public override string GetPKName() {
			return "rowid";
		}

		public override object GetPKValue() {
			return _rowid;
		}

	}

	[TableAttribute(Name = "cal_relations")]
	public partial class cal_relations : Model {
		private Int64 _rowid;
		/// <summary>
		/// Auto generated SQLite rowid column.
		/// </summary>
		public Int64 rowid {
			get { return _rowid; }
		}

		private bool _cal_idChanged = false;
		private String _cal_id;
		public String cal_id {
			get { return _cal_id; }
			set {
				_cal_id = value;
				_cal_idChanged = true;
			}
		}

		private bool _item_idChanged = false;
		private String _item_id;
		public String item_id {
			get { return _item_id; }
			set {
				_item_id = value;
				_item_idChanged = true;
			}
		}

		private bool _recurrence_idChanged = false;
		private Int64 _recurrence_id;
		public Int64 recurrence_id {
			get { return _recurrence_id; }
			set {
				_recurrence_id = value;
				_recurrence_idChanged = true;
			}
		}

		private bool _recurrence_id_tzChanged = false;
		private String _recurrence_id_tz;
		public String recurrence_id_tz {
			get { return _recurrence_id_tz; }
			set {
				_recurrence_id_tz = value;
				_recurrence_id_tzChanged = true;
			}
		}

		private bool _icalStringChanged = false;
		private String _icalString;
		public String icalString {
			get { return _icalString; }
			set {
				_icalString = value;
				_icalStringChanged = true;
			}
		}

		public cal_relations() : this(null, null) { }

		public cal_relations(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "rowid": _rowid = (reader.IsDBNull(i)) ? default(Int64) : reader.GetFieldValue<Int64>(i); break;
					case "cal_id": _cal_id = reader.GetValue(i) as String; break;
					case "item_id": _item_id = reader.GetValue(i) as String; break;
					case "recurrence_id": _recurrence_id = (reader.IsDBNull(i)) ? default(Int64) : reader.GetFieldValue<Int64>(i); break;
					case "recurrence_id_tz": _recurrence_id_tz = reader.GetValue(i) as String; break;
					case "icalString": _icalString = reader.GetValue(i) as String; break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_cal_idChanged)
				changed.Add("cal_id", _cal_id);
			if (_item_idChanged)
				changed.Add("item_id", _item_id);
			if (_recurrence_idChanged)
				changed.Add("recurrence_id", _recurrence_id);
			if (_recurrence_id_tzChanged)
				changed.Add("recurrence_id_tz", _recurrence_id_tz);
			if (_icalStringChanged)
				changed.Add("icalString", _icalString);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_cal_id,
				_item_id,
				_recurrence_id,
				_recurrence_id_tz,
				_icalString,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"cal_id",
				"item_id",
				"recurrence_id",
				"recurrence_id_tz",
				"icalString",
			};
		}

		public override string GetPKName() {
			return "rowid";
		}

		public override object GetPKValue() {
			return _rowid;
		}

	}

	[TableAttribute(Name = "cal_attendees")]
	public partial class cal_attendees : Model {
		private Int64 _rowid;
		/// <summary>
		/// Auto generated SQLite rowid column.
		/// </summary>
		public Int64 rowid {
			get { return _rowid; }
		}

		private bool _item_idChanged = false;
		private String _item_id;
		public String item_id {
			get { return _item_id; }
			set {
				_item_id = value;
				_item_idChanged = true;
			}
		}

		private bool _recurrence_idChanged = false;
		private Int64 _recurrence_id;
		public Int64 recurrence_id {
			get { return _recurrence_id; }
			set {
				_recurrence_id = value;
				_recurrence_idChanged = true;
			}
		}

		private bool _recurrence_id_tzChanged = false;
		private String _recurrence_id_tz;
		public String recurrence_id_tz {
			get { return _recurrence_id_tz; }
			set {
				_recurrence_id_tz = value;
				_recurrence_id_tzChanged = true;
			}
		}

		private bool _cal_idChanged = false;
		private String _cal_id;
		public String cal_id {
			get { return _cal_id; }
			set {
				_cal_id = value;
				_cal_idChanged = true;
			}
		}

		private bool _icalStringChanged = false;
		private String _icalString;
		public String icalString {
			get { return _icalString; }
			set {
				_icalString = value;
				_icalStringChanged = true;
			}
		}

		public cal_attendees() : this(null, null) { }

		public cal_attendees(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "rowid": _rowid = (reader.IsDBNull(i)) ? default(Int64) : reader.GetFieldValue<Int64>(i); break;
					case "item_id": _item_id = reader.GetValue(i) as String; break;
					case "recurrence_id": _recurrence_id = (reader.IsDBNull(i)) ? default(Int64) : reader.GetFieldValue<Int64>(i); break;
					case "recurrence_id_tz": _recurrence_id_tz = reader.GetValue(i) as String; break;
					case "cal_id": _cal_id = reader.GetValue(i) as String; break;
					case "icalString": _icalString = reader.GetValue(i) as String; break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_item_idChanged)
				changed.Add("item_id", _item_id);
			if (_recurrence_idChanged)
				changed.Add("recurrence_id", _recurrence_id);
			if (_recurrence_id_tzChanged)
				changed.Add("recurrence_id_tz", _recurrence_id_tz);
			if (_cal_idChanged)
				changed.Add("cal_id", _cal_id);
			if (_icalStringChanged)
				changed.Add("icalString", _icalString);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_item_id,
				_recurrence_id,
				_recurrence_id_tz,
				_cal_id,
				_icalString,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"item_id",
				"recurrence_id",
				"recurrence_id_tz",
				"cal_id",
				"icalString",
			};
		}

		public override string GetPKName() {
			return "rowid";
		}

		public override object GetPKValue() {
			return _rowid;
		}

	}

	[TableAttribute(Name = "cal_recurrence")]
	public partial class cal_recurrence : Model {
		private Int64 _rowid;
		/// <summary>
		/// Auto generated SQLite rowid column.
		/// </summary>
		public Int64 rowid {
			get { return _rowid; }
		}

		private bool _item_idChanged = false;
		private String _item_id;
		public String item_id {
			get { return _item_id; }
			set {
				_item_id = value;
				_item_idChanged = true;
			}
		}

		private bool _cal_idChanged = false;
		private String _cal_id;
		public String cal_id {
			get { return _cal_id; }
			set {
				_cal_id = value;
				_cal_idChanged = true;
			}
		}

		private bool _icalStringChanged = false;
		private String _icalString;
		public String icalString {
			get { return _icalString; }
			set {
				_icalString = value;
				_icalStringChanged = true;
			}
		}

		public cal_recurrence() : this(null, null) { }

		public cal_recurrence(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "rowid": _rowid = (reader.IsDBNull(i)) ? default(Int64) : reader.GetFieldValue<Int64>(i); break;
					case "item_id": _item_id = reader.GetValue(i) as String; break;
					case "cal_id": _cal_id = reader.GetValue(i) as String; break;
					case "icalString": _icalString = reader.GetValue(i) as String; break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_item_idChanged)
				changed.Add("item_id", _item_id);
			if (_cal_idChanged)
				changed.Add("cal_id", _cal_id);
			if (_icalStringChanged)
				changed.Add("icalString", _icalString);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_item_id,
				_cal_id,
				_icalString,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"item_id",
				"cal_id",
				"icalString",
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