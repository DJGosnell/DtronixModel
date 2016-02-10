<?xml version="1.0"?>
<Database xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" Name="TestDatabase" Namespace="DtronixModelTests.Sqlite" ContextClass="TestDatabaseContext" ImplementINotifyPropertyChanged="true" TargetDb="Sqlite">
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
    <Column Name="db_int16" NetType="Int16" DbType="SMALLINT" Nullable="true" />
    <Column Name="db_int32" NetType="Int32" DbType="INTEGER" Nullable="true" />
    <Column Name="db_int64" NetType="Int64" DbType="INTEGER" Nullable="true" />
    <Column Name="db_byte_array" NetType="ByteArray" DbType="BLOB" Nullable="true" />
    <Column Name="db_byte" NetType="Byte" DbType="BLOB" Nullable="true" />
    <Column Name="db_date_time" NetType="DateTimeOffset" DbType="DATETIME" Nullable="true" />
    <Column Name="db_decimal" NetType="Decimal" DbType="REAL" Nullable="true" />
    <Column Name="db_float" NetType="Float" DbType="FLOAT" Nullable="true" />
    <Column Name="db_double" NetType="Double" DbType="DOUBLE" Nullable="true" />
    <Column Name="db_bool" NetType="Boolean" DbType="BOOLEAN" Nullable="true" />
    <Column Name="db_string" NetType="String" DbType="TEXT" Nullable="true" />
    <Column Name="db_enum" NetType="TestEnum" DbType="INTEGER" />
  </Table>
  <Association Table1Name="Logs" Table1="Logs" Table1Column="Users_rowid" Table1Cardinality="Many" Table2Name="User" Table2="Users" Table2Column="rowid" Table2Cardinality="One" />
  <Enumeration Name="TestEnum">
    <EnumValue Name="Unset" />
    <EnumValue Name="Enum1" />
    <EnumValue Name="SecondEnumValue" />
  </Enumeration>
</Database>
