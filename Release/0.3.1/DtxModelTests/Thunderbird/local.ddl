<?xml version="1.0"?>
<Database xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" Name="local.sqlite">
  <Table Name="cal_calendar_schema_version">
    <Column Name="rowid" Description="Auto generated SQLite rowid column." NetType="Int64" DbType="INTEGER" IsReadOnly="true" IsPrimaryKey="true" IsAutoIncrement="true" />
    <Column Name="version" NetType="Int64" DbType="INTEGER" Nullable="true" />
  </Table>
  <Table Name="cal_properties">
    <Column Name="rowid" Description="Auto generated SQLite rowid column." NetType="Int64" DbType="INTEGER" IsReadOnly="true" IsPrimaryKey="true" IsAutoIncrement="true" />
    <Column Name="item_id" NetType="String" DbType="TEXT" Nullable="true" />
    <Column Name="key" NetType="String" DbType="TEXT" Nullable="true" />
    <Column Name="value" NetType="ByteArray" DbType="BLOB" Nullable="true" />
    <Column Name="recurrence_id" NetType="Int64" DbType="INTEGER" Nullable="true" />
    <Column Name="recurrence_id_tz" NetType="String" DbType="TEXT" Nullable="true" />
    <Column Name="cal_id" NetType="String" DbType="TEXT" Nullable="true" />
    <Index Name="idx_cal_properties_cal_id_item_id_recurrence_id_recurrence_id_tz">
      <IndexColumn Name="cal_id" />
      <IndexColumn Name="item_id" />
      <IndexColumn Name="recurrence_id" />
      <IndexColumn Name="recurrence_id_tz" />
    </Index>
  </Table>
  <Table Name="cal_events">
    <Column Name="rowid" Description="Auto generated SQLite rowid column." NetType="Int64" DbType="INTEGER" IsReadOnly="true" IsPrimaryKey="true" IsAutoIncrement="true" />
    <Column Name="cal_id" NetType="String" DbType="TEXT" Nullable="true" />
    <Column Name="id" NetType="String" DbType="TEXT" Nullable="true" />
    <Column Name="time_created" NetType="Int64" DbType="INTEGER" Nullable="true" />
    <Column Name="last_modified" NetType="Int64" DbType="INTEGER" Nullable="true" />
    <Column Name="title" NetType="String" DbType="TEXT" Nullable="true" />
    <Column Name="priority" NetType="Int64" DbType="INTEGER" Nullable="true" />
    <Column Name="privacy" NetType="String" DbType="TEXT" Nullable="true" />
    <Column Name="ical_status" NetType="String" DbType="TEXT" Nullable="true" />
    <Column Name="flags" NetType="Int64" DbType="INTEGER" Nullable="true" />
    <Column Name="event_start" NetType="Int64" DbType="INTEGER" Nullable="true" />
    <Column Name="event_end" NetType="Int64" DbType="INTEGER" Nullable="true" />
    <Column Name="event_stamp" NetType="Int64" DbType="INTEGER" Nullable="true" />
    <Column Name="event_start_tz" NetType="String" DbType="TEXT" Nullable="true" />
    <Column Name="event_end_tz" NetType="String" DbType="TEXT" Nullable="true" />
    <Column Name="recurrence_id" NetType="Int64" DbType="INTEGER" Nullable="true" />
    <Column Name="recurrence_id_tz" NetType="String" DbType="TEXT" Nullable="true" />
    <Column Name="alarm_last_ack" NetType="Int64" DbType="INTEGER" Nullable="true" />
    <Column Name="offline_journal" NetType="Int64" DbType="INTEGER" Nullable="true" />
    <Index Name="idx_cal_events_flags_cal_id_recurrence_id">
      <IndexColumn Name="flags" />
      <IndexColumn Name="cal_id" />
      <IndexColumn Name="recurrence_id" />
    </Index>
    <Index Name="idx_cal_events_id_cal_id_recurrence_id">
      <IndexColumn Name="id" />
      <IndexColumn Name="cal_id" />
      <IndexColumn Name="recurrence_id" />
    </Index>
  </Table>
  <Table Name="cal_todos">
    <Column Name="rowid" Description="Auto generated SQLite rowid column." NetType="Int64" DbType="INTEGER" IsReadOnly="true" IsPrimaryKey="true" IsAutoIncrement="true" />
    <Column Name="cal_id" NetType="String" DbType="TEXT" Nullable="true" />
    <Column Name="id" NetType="String" DbType="TEXT" Nullable="true" />
    <Column Name="time_created" NetType="Int64" DbType="INTEGER" Nullable="true" />
    <Column Name="last_modified" NetType="Int64" DbType="INTEGER" Nullable="true" />
    <Column Name="title" NetType="String" DbType="TEXT" Nullable="true" />
    <Column Name="priority" NetType="Int64" DbType="INTEGER" Nullable="true" />
    <Column Name="privacy" NetType="String" DbType="TEXT" Nullable="true" />
    <Column Name="ical_status" NetType="String" DbType="TEXT" Nullable="true" />
    <Column Name="flags" NetType="Int64" DbType="INTEGER" Nullable="true" />
    <Column Name="todo_entry" NetType="Int64" DbType="INTEGER" Nullable="true" />
    <Column Name="todo_due" NetType="Int64" DbType="INTEGER" Nullable="true" />
    <Column Name="todo_completed" NetType="Int64" DbType="INTEGER" Nullable="true" />
    <Column Name="todo_complete" NetType="Int64" DbType="INTEGER" Nullable="true" />
    <Column Name="todo_entry_tz" NetType="String" DbType="TEXT" Nullable="true" />
    <Column Name="todo_due_tz" NetType="String" DbType="TEXT" Nullable="true" />
    <Column Name="todo_completed_tz" NetType="String" DbType="TEXT" Nullable="true" />
    <Column Name="recurrence_id" NetType="Int64" DbType="INTEGER" Nullable="true" />
    <Column Name="recurrence_id_tz" NetType="String" DbType="TEXT" Nullable="true" />
    <Column Name="alarm_last_ack" NetType="Int64" DbType="INTEGER" Nullable="true" />
    <Column Name="todo_stamp" NetType="Int64" DbType="INTEGER" Nullable="true" />
    <Column Name="offline_journal" NetType="Int64" DbType="INTEGER" Nullable="true" />
    <Index Name="idx_cal_todos_flags_cal_id_recurrence_id">
      <IndexColumn Name="flags" />
      <IndexColumn Name="cal_id" />
      <IndexColumn Name="recurrence_id" />
    </Index>
    <Index Name="idx_cal_todos_id_cal_id_recurrence_id">
      <IndexColumn Name="id" />
      <IndexColumn Name="cal_id" />
      <IndexColumn Name="recurrence_id" />
    </Index>
  </Table>
  <Table Name="cal_tz_version">
    <Column Name="rowid" Description="Auto generated SQLite rowid column." NetType="Int64" DbType="INTEGER" IsReadOnly="true" IsPrimaryKey="true" IsAutoIncrement="true" />
    <Column Name="version" NetType="String" DbType="TEXT" Nullable="true" />
  </Table>
  <Table Name="cal_metadata">
    <Column Name="rowid" Description="Auto generated SQLite rowid column." NetType="Int64" DbType="INTEGER" IsReadOnly="true" IsPrimaryKey="true" IsAutoIncrement="true" />
    <Column Name="cal_id" NetType="String" DbType="TEXT" Nullable="true" />
    <Column Name="item_id" NetType="String" DbType="TEXT" Nullable="true" />
    <Column Name="value" NetType="ByteArray" DbType="BLOB" Nullable="true" />
    <Index Name="idx_cal_metadata_cal_id_item_id">
      <IndexColumn Name="cal_id" />
      <IndexColumn Name="item_id" />
    </Index>
  </Table>
  <Table Name="cal_alarms">
    <Column Name="rowid" Description="Auto generated SQLite rowid column." NetType="Int64" DbType="INTEGER" IsReadOnly="true" IsPrimaryKey="true" IsAutoIncrement="true" />
    <Column Name="cal_id" NetType="String" DbType="TEXT" Nullable="true" />
    <Column Name="item_id" NetType="String" DbType="TEXT" Nullable="true" />
    <Column Name="recurrence_id" NetType="Int64" DbType="INTEGER" Nullable="true" />
    <Column Name="recurrence_id_tz" NetType="String" DbType="TEXT" Nullable="true" />
    <Column Name="icalString" NetType="String" DbType="TEXT" Nullable="true" />
    <Index Name="idx_cal_alarms_cal_id_item_id_recurrence_id_recurrence_id_tz">
      <IndexColumn Name="cal_id" />
      <IndexColumn Name="item_id" />
      <IndexColumn Name="recurrence_id" />
      <IndexColumn Name="recurrence_id_tz" />
    </Index>
  </Table>
  <Table Name="cal_attachments">
    <Column Name="rowid" Description="Auto generated SQLite rowid column." NetType="Int64" DbType="INTEGER" IsReadOnly="true" IsPrimaryKey="true" IsAutoIncrement="true" />
    <Column Name="item_id" NetType="String" DbType="TEXT" Nullable="true" />
    <Column Name="cal_id" NetType="String" DbType="TEXT" Nullable="true" />
    <Column Name="recurrence_id" NetType="Int64" DbType="INTEGER" Nullable="true" />
    <Column Name="recurrence_id_tz" NetType="String" DbType="TEXT" Nullable="true" />
    <Column Name="icalString" NetType="String" DbType="TEXT" Nullable="true" />
  </Table>
  <Table Name="cal_relations">
    <Column Name="rowid" Description="Auto generated SQLite rowid column." NetType="Int64" DbType="INTEGER" IsReadOnly="true" IsPrimaryKey="true" IsAutoIncrement="true" />
    <Column Name="cal_id" NetType="String" DbType="TEXT" Nullable="true" />
    <Column Name="item_id" NetType="String" DbType="TEXT" Nullable="true" />
    <Column Name="recurrence_id" NetType="Int64" DbType="INTEGER" Nullable="true" />
    <Column Name="recurrence_id_tz" NetType="String" DbType="TEXT" Nullable="true" />
    <Column Name="icalString" NetType="String" DbType="TEXT" Nullable="true" />
  </Table>
  <Table Name="cal_attendees">
    <Column Name="rowid" Description="Auto generated SQLite rowid column." NetType="Int64" DbType="INTEGER" IsReadOnly="true" IsPrimaryKey="true" IsAutoIncrement="true" />
    <Column Name="item_id" NetType="String" DbType="TEXT" Nullable="true" />
    <Column Name="recurrence_id" NetType="Int64" DbType="INTEGER" Nullable="true" />
    <Column Name="recurrence_id_tz" NetType="String" DbType="TEXT" Nullable="true" />
    <Column Name="cal_id" NetType="String" DbType="TEXT" Nullable="true" />
    <Column Name="icalString" NetType="String" DbType="TEXT" Nullable="true" />
  </Table>
  <Table Name="cal_recurrence">
    <Column Name="rowid" Description="Auto generated SQLite rowid column." NetType="Int64" DbType="INTEGER" IsReadOnly="true" IsPrimaryKey="true" IsAutoIncrement="true" />
    <Column Name="item_id" NetType="String" DbType="TEXT" Nullable="true" />
    <Column Name="cal_id" NetType="String" DbType="TEXT" Nullable="true" />
    <Column Name="icalString" NetType="String" DbType="TEXT" Nullable="true" />
  </Table>
  <Configuration Name="sqlite.pragma.auto_vacuum" Value="0" Description="http://sqlite.org/pragma.html#pragma_auto_vacuum" />
  <Configuration Name="sqlite.pragma.automatic_index" Value="1" Description="http://sqlite.org/pragma.html#pragma_automatic_index" />
  <Configuration Name="sqlite.pragma.cache_size" Value="2000" Description="http://sqlite.org/pragma.html#pragma_cache_size" />
  <Configuration Name="sqlite.pragma.cache_spill" Value="1" Description="http://sqlite.org/pragma.html#pragma_cache_spill" />
  <Configuration Name="sqlite.pragma.checkpoint_fullfsync" Value="0" Description="http://sqlite.org/pragma.html#pragma_checkpoint_fullfsync" />
  <Configuration Name="sqlite.pragma.defer_foreign_keys" Value="0" Description="http://sqlite.org/pragma.html#pragma_defer_foreign_keys" />
  <Configuration Name="sqlite.pragma.encoding" Value="UTF-8" Description="http://sqlite.org/pragma.html#pragma_encoding" />
  <Configuration Name="sqlite.pragma.foreign_keys" Value="0" Description="http://sqlite.org/pragma.html#pragma_foreign_keys" />
  <Configuration Name="sqlite.pragma.full_column_names" Value="0" Description="http://sqlite.org/pragma.html#pragma_full_column_names" />
  <Configuration Name="sqlite.pragma.fullfsync" Value="0" Description="http://sqlite.org/pragma.html#pragma_fullfsync" />
  <Configuration Name="sqlite.pragma.integrity_check" Value="ok" Description="http://sqlite.org/pragma.html#pragma_integrity_check" />
  <Configuration Name="sqlite.pragma.journal_mode" Value="delete" Description="http://sqlite.org/pragma.html#pragma_journal_mode" />
  <Configuration Name="sqlite.pragma.journal_size_limit" Value="-1" Description="http://sqlite.org/pragma.html#pragma_journal_size_limit" />
  <Configuration Name="sqlite.pragma.legacy_file_format" Value="0" Description="http://sqlite.org/pragma.html#pragma_legacy_file_format" />
  <Configuration Name="sqlite.pragma.locking_mode" Value="normal" Description="http://sqlite.org/pragma.html#pragma_locking_mode" />
  <Configuration Name="sqlite.pragma.max_page_count" Value="1073741823" Description="http://sqlite.org/pragma.html#pragma_max_page_count" />
  <Configuration Name="sqlite.pragma.page_size" Value="1024" Description="http://sqlite.org/pragma.html#pragma_page_size" />
  <Configuration Name="sqlite.pragma.query_only" Value="0" Description="http://sqlite.org/pragma.html#pragma_query_only" />
  <Configuration Name="sqlite.pragma.read_uncommitted" Value="0" Description="http://sqlite.org/pragma.html#pragma_read_uncommitted" />
  <Configuration Name="sqlite.pragma.recursive_triggers" Value="0" Description="http://sqlite.org/pragma.html#pragma_recursive_triggers" />
  <Configuration Name="sqlite.pragma.reverse_unordered_selects" Value="0" Description="http://sqlite.org/pragma.html#pragma_reverse_unordered_selects" />
  <Configuration Name="sqlite.pragma.schema_version" Value="41" Description="http://sqlite.org/pragma.html#pragma_schema_version" />
  <Configuration Name="sqlite.pragma.user_version" Value="0" Description="http://sqlite.org/pragma.html#pragma_user_version" />
  <Configuration Name="sqlite.pragma.secure_delete" Value="0" Description="http://sqlite.org/pragma.html#pragma_secure_delete" />
  <Configuration Name="sqlite.pragma.soft_heap_limit" Value="0" Description="http://sqlite.org/pragma.html#pragma_soft_heap_limit" />
  <Configuration Name="sqlite.pragma.synchronous" Value="2" Description="http://sqlite.org/pragma.html#pragma_synchronous" />
  <Configuration Name="sqlite.pragma.temp_store" Value="0" Description="http://sqlite.org/pragma.html#pragma_temp_store" />
  <Configuration Name="sqlite.pragma.wal_autocheckpoint" Value="1000" Description="http://sqlite.org/pragma.html#pragma_wal_autocheckpoint" />
  <Configuration Name="database.namespace" Value="DtxModelTests.Models" Description="Namespace for all the generated classes." />
  <Configuration Name="database.context_class" Value="ThunderbirdCalendarContext" Description="Name of the context class." />
  <Configuration Name="output.sql_tables" Value="False" Description="True to output the SQL database schematic tables." />
  <Configuration Name="output.cs_classes" Value="True" Description="True to output the C# classes." />
</Database>
