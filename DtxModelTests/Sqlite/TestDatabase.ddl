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
  <Association Table1Name="Logs" Table1="Logs" Table1Column="Users_rowid" Table1Cardinality="Many" Table2Name="Users" Table2="Users" Table2Column="rowid" Table2Cardinality="One" />
</Database>
