<?xml version="1.0"?>
<Database xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" Name="TestDatabase" Namespace="DtxModelTests.Sqlite" ContextClass="TestDatabaseContext" TargetDb="Sqlite">
  <Table Name="Users">
    <Column Name="rowid" NetType="Int64" DbType="INTEGER" IsPrimaryKey="true" IsAutoIncrement="true" />
    <Column Name="username" NetType="String" DbType="TEXT" />
    <Column Name="password" NetType="String" DbType="TEXT" />
    <Column Name="last_logged" NetType="Int64" DbType="INTEGER" />
  </Table>
  <Table Name="Logs">
    <Column Name="rowid" NetType="Int64" DbType="INTEGER" IsPrimaryKey="true" IsAutoIncrement="true" />
    <Column Name="Users_rowid" Description="User which created this log item." NetType="Int64" DbType="INTEGER" />
    <Column Name="text" NetType="String" DbType="TEXT" />
  </Table>
  <Table Name="AllTypes">
    <Column Name="id" NetType="Int64" DbType="INTEGER" IsPrimaryKey="true" IsAutoIncrement="true" />
    <Column Name="db_int16" NetType="Int16" DbType="SMALLINT" />
    <Column Name="db_int32" NetType="Int32" DbType="INTEGER" />
    <Column Name="db_byte_array" NetType="ByteArray" DbType="BLOB" />
    <Column Name="db_byte" NetType="Byte" DbType="BLOB" />
    <Column Name="db_date_time" NetType="DateTime" DbType="DATETIME" />
    <Column Name="db_date_time_offset" NetType="DateTimeOffset" DbType="DATETIME" />
    <Column Name="db_decimal" NetType="Decimal" DbType="REAL" />
    <Column Name="db_float" NetType="Float" DbType="FLOAT" />
    <Column Name="db_double" NetType="Double" DbType="DOUBLE" />
    <Column Name="db_bool" NetType="Boolean" DbType="BOOLEAN" />
    <Column Name="db_string" NetType="String" DbType="TEXT" />
    <Column Name="db_char" NetType="Char" DbType="CHAR" />
  </Table>
  <Association Table1Name="Logs" Table1="Logs" Table1Column="Users_rowid" Table1Cardinality="Many" Table2Name="User" Table2="Users" Table2Column="rowid" Table2Cardinality="One" />
</Database>
