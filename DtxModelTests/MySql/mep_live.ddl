<?xml version="1.0"?>
<Database xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" Name="mep_live" Namespace="DtxModelTests.MySql" ContextClass="mep_liveContext" TargetDb="MySQL">
  <Table Name="Users">
    <Column Name="id" DefaultValue="" Description="Row identifier." NetType="Int32" DbType="INT" IsPrimaryKey="true" IsAutoIncrement="true" />
    <Column Name="username" DefaultValue="" Description="Username for the user." NetType="String" DbType="VARCHAR" />
    <Column Name="Permissions_id" DefaultValue="3" Description="Permission ID associated with this account." NetType="Int32" DbType="INT" />
    <Column Name="Contacts_id" DefaultValue="" Description="" NetType="Int32" DbType="INT" />
    <Column Name="password" DefaultValue="" Description="MD5 hashed password." NetType="String" DbType="VARCHAR" />
    <Column Name="date_registered" DefaultValue="" Description="Date when the user registered on the application." NetType="Int64" DbType="BIGINT" />
    <Column Name="activation_code" DefaultValue="" Description="Code that is used when the account needs activation. Empty if no activation is required." NetType="String" DbType="VARCHAR" Nullable="true" />
    <Column Name="banned" DefaultValue="" Description="True if the user is banned, false otherwise." NetType="Int16" DbType="TINYINT" />
    <Column Name="ban_reason" DefaultValue="" Description="Reason that the user was banned." NetType="String" DbType="TEXT" Nullable="true" />
    <Column Name="last_online" DefaultValue="" Description="Time the user was last active on this application." NetType="Int32" DbType="INT" />
  </Table>
  <Table Name="Settings">
    <Column Name="id" DefaultValue="" Description="Row identifier." NetType="Int32" DbType="INT" IsPrimaryKey="true" IsAutoIncrement="true" />
    <Column Name="property" DefaultValue="" Description="Key that references the same row's value." NetType="String" DbType="VARCHAR" />
    <Column Name="value" DefaultValue="" Description="Stored setting value." NetType="String" DbType="VARCHAR" Nullable="true" />
    <Column Name="description" DefaultValue="" Description="Short description about the setting." NetType="String" DbType="VARCHAR" Nullable="true" />
  </Table>
  <Table Name="Logs">
    <Column Name="id" DefaultValue="" Description="Row identifier." NetType="Int32" DbType="INT" IsPrimaryKey="true" IsAutoIncrement="true" />
    <Column Name="Users_id" DefaultValue="" Description="User ID that this log item occured on." NetType="Int32" DbType="INT" />
    <Column Name="time" DefaultValue="" Description="Time of log entry." NetType="Int64" DbType="BIGINT" Nullable="true" />
    <Column Name="type" DefaultValue="" Description="Type ID of the error that occured." NetType="Int16" DbType="TINYINT" Nullable="true" />
    <Column Name="ip" DefaultValue="" Description="IP address of the client that this log item occured on." NetType="Int64" DbType="BIGINT" Nullable="true" />
    <Column Name="message" DefaultValue="" Description="Full message text of the error and or backtrace." NetType="String" DbType="TEXT" Nullable="true" />
  </Table>
  <Table Name="Permissions">
    <Column Name="id" DefaultValue="" Description="Row identifier." NetType="Int32" DbType="INT" IsPrimaryKey="true" IsAutoIncrement="true" />
    <Column Name="name" DefaultValue="" Description="Internal name associated with this permission set." NetType="String" DbType="VARCHAR" Nullable="true" />
    <Column Name="base_permissions_id" DefaultValue="" Description="Permission set that serves as a base for this one." NetType="Int32" DbType="INT" Nullable="true" />
    <Column Name="can_register" DefaultValue="1" Description="True for enabling registration." NetType="Boolean" DbType="BOOL" Nullable="true" />
    <Column Name="can_login" DefaultValue="1" Description="True to allow user login." NetType="Boolean" DbType="BOOL" Nullable="true" />
    <Column Name="can_edit_users" DefaultValue="0" Description="True to allow user to edit other user's settings." NetType="Boolean" DbType="BOOL" Nullable="true" />
    <Column Name="can_edit_settings" DefaultValue="0" Description="True to allow user to edit application settings." NetType="Boolean" DbType="BOOL" Nullable="true" />
    <Column Name="can_manage_images" DefaultValue="0" Description="True to allow image upload and management." NetType="Boolean" DbType="BOOL" Nullable="true" />
    <Column Name="access_xflow" DefaultValue="0" Description="True to allow access to x-flow for projects." NetType="Boolean" DbType="BOOL" Nullable="true" />
  </Table>
  <Table Name="Projects">
    <Column Name="id" DefaultValue="" Description="Project id." NetType="Int32" DbType="INT" IsPrimaryKey="true" IsAutoIncrement="true" />
    <Column Name="Companies_id_architect" DefaultValue="" Description="ID of the architect that designed the building." NetType="Int32" DbType="INT" Nullable="true" />
    <Column Name="ProjectCategories_id" DefaultValue="" Description="Category of the project." NetType="Int32" DbType="INT" Nullable="true" />
    <Column Name="Addresses_id" DefaultValue="" Description="" NetType="Int32" DbType="INT" Nullable="true" />
    <Column Name="name" DefaultValue="" Description="Name of the project." NetType="String" DbType="VARCHAR" Nullable="true" />
    <Column Name="status" DefaultValue="" Description="Status of the project. (Ongoing, Postponed, Complete, etc...)" NetType="Int32" DbType="INT" Nullable="true" />
    <Column Name="directory" DefaultValue="" Description="Location the project is located on the server." NetType="String" DbType="VARCHAR" Nullable="true" />
    <Column Name="date_started" DefaultValue="" Description="Date when the project started." NetType="Int64" DbType="BIGINT" Nullable="true" />
    <Column Name="date_completed" DefaultValue="" Description="Date when the project was completed." NetType="Int64" DbType="BIGINT" Nullable="true" />
    <Column Name="details" DefaultValue="" Description="Short description of the project." NetType="String" DbType="TEXT" Nullable="true" />
  </Table>
  <Table Name="Companies">
    <Column Name="id" DefaultValue="" Description="Row identifier." NetType="Int32" DbType="INT" IsPrimaryKey="true" IsAutoIncrement="true" />
    <Column Name="name" DefaultValue="" Description="Name of the architectural firm." NetType="String" DbType="VARCHAR" Nullable="true" />
    <Column Name="url" DefaultValue="" Description="Internet acddress of the architect." NetType="String" DbType="VARCHAR" Nullable="true" />
    <Column Name="notes" DefaultValue="" Description="" NetType="String" DbType="TINYTEXT" Nullable="true" />
    <Column Name="Addresses_id" DefaultValue="" Description="" NetType="Int32" DbType="INT" Nullable="true" />
  </Table>
  <Table Name="ProjectCategories">
    <Column Name="id" DefaultValue="" Description="Row identifier." NetType="Int32" DbType="INT" IsPrimaryKey="true" IsAutoIncrement="true" />
    <Column Name="name" DefaultValue="" Description="Name of project category" NetType="String" DbType="VARCHAR" Nullable="true" />
  </Table>
  <Table Name="ProjectPictures">
    <Column Name="id" DefaultValue="" Description="Image identifier." NetType="Int32" DbType="INT" IsPrimaryKey="true" IsAutoIncrement="true" />
    <Column Name="Projects_id" DefaultValue="" Description="Id of the project this picture belongs to." NetType="Int32" DbType="INT" />
    <Column Name="description" DefaultValue="" Description="Short description of the picture." NetType="String" DbType="TINYTEXT" Nullable="true" />
    <Column Name="display" DefaultValue="1" Description="True if the picture is to be displayed; false otherwise." NetType="Boolean" DbType="BOOL" />
    <Column Name="featured" DefaultValue="0" Description="True if the picture is to be featured on the main rotation." NetType="Boolean" DbType="BOOL" />
  </Table>
  <Table Name="Contacts">
    <Column Name="id" DefaultValue="" Description="" NetType="Int32" DbType="INT" IsPrimaryKey="true" IsAutoIncrement="true" />
    <Column Name="Addresses_id" DefaultValue="" Description="" NetType="Int32" DbType="INT" Nullable="true" />
    <Column Name="Companies_id" DefaultValue="" Description="" NetType="Int32" DbType="INT" Nullable="true" />
    <Column Name="first_name" DefaultValue="" Description="" NetType="String" DbType="VARCHAR" />
    <Column Name="last_name" DefaultValue="" Description="" NetType="String" DbType="VARCHAR" Nullable="true" />
    <Column Name="display" DefaultValue="" Description="Combination of the first and last names." NetType="String" DbType="VARCHAR" Nullable="true" />
    <Column Name="nickname" DefaultValue="" Description="" NetType="String" DbType="VARCHAR" Nullable="true" />
    <Column Name="title" DefaultValue="" Description="" NetType="String" DbType="VARCHAR" Nullable="true" />
    <Column Name="email1" DefaultValue="" Description="" NetType="String" DbType="VARCHAR" Nullable="true" />
    <Column Name="email2" DefaultValue="" Description="" NetType="String" DbType="VARCHAR" Nullable="true" />
    <Column Name="website" DefaultValue="" Description="" NetType="String" DbType="VARCHAR" Nullable="true" />
    <Column Name="phone_work" DefaultValue="" Description="" NetType="String" DbType="VARCHAR" Nullable="true" />
    <Column Name="phone_home" DefaultValue="" Description="" NetType="String" DbType="VARCHAR" Nullable="true" />
    <Column Name="phone_cell" DefaultValue="" Description="" NetType="String" DbType="VARCHAR" Nullable="true" />
    <Column Name="fax" DefaultValue="" Description="" NetType="String" DbType="VARCHAR" Nullable="true" />
    <Column Name="notes" DefaultValue="" Description="Additional notes about this contact." NetType="String" DbType="TINYTEXT" Nullable="true" />
  </Table>
  <Table Name="Messages">
    <Column Name="id" DefaultValue="" Description="" NetType="Int32" DbType="INT" IsPrimaryKey="true" IsAutoIncrement="true" />
    <Column Name="Users_id" DefaultValue="" Description="" NetType="Int32" DbType="INT" />
    <Column Name="Projects_id" DefaultValue="" Description="" NetType="Int32" DbType="INT" />
    <Column Name="Contacts_id_to" DefaultValue="" Description="" NetType="Int32" DbType="INT" />
    <Column Name="Contacts_id_from" DefaultValue="" Description="" NetType="Int32" DbType="INT" />
    <Column Name="Message" DefaultValue="" Description="" NetType="String" DbType="TEXT" Nullable="true" />
    <Column Name="type" DefaultValue="" Description="In, Out, Internal" NetType="Int32" DbType="INT" Nullable="true" />
  </Table>
  <Table Name="ProjectsContactsList">
    <Column Name="id" DefaultValue="" Description="" NetType="Int32" DbType="INT" IsPrimaryKey="true" IsAutoIncrement="true" />
    <Column Name="type" DefaultValue="" Description="Name of this relationship. (GC, PC, Architect, Civil, etc...)" NetType="String" DbType="VARCHAR" Nullable="true" />
    <Column Name="Projects_id" DefaultValue="" Description="" NetType="Int32" DbType="INT" />
    <Column Name="Contacts_id" DefaultValue="" Description="" NetType="Int32" DbType="INT" />
  </Table>
  <Table Name="Addresses">
    <Column Name="id" DefaultValue="" Description="" NetType="Int32" DbType="INT" IsPrimaryKey="true" IsAutoIncrement="true" />
    <Column Name="address" DefaultValue="" Description="" NetType="String" DbType="VARCHAR" Nullable="true" />
    <Column Name="address2" DefaultValue="" Description="" NetType="String" DbType="VARCHAR" Nullable="true" />
    <Column Name="city" DefaultValue="" Description="" NetType="String" DbType="VARCHAR" Nullable="true" />
    <Column Name="state" DefaultValue="" Description="" NetType="String" DbType="CHAR" Nullable="true" />
    <Column Name="zip" DefaultValue="" Description="" NetType="String" DbType="VARCHAR" Nullable="true" />
  </Table>
  <Table Name="ConstructionDetails">
    <Column Name="id" DefaultValue="" Description="" NetType="Int32" DbType="INT" IsPrimaryKey="true" IsAutoIncrement="true" />
    <Column Name="Projects_id" DefaultValue="" Description="" NetType="Int32" DbType="INT" />
    <Column Name="Users_id" DefaultValue="" Description="" NetType="Int32" DbType="INT" />
    <Column Name="date" DefaultValue="" Description="" NetType="Int64" DbType="BIGINT" />
    <Column Name="type" DefaultValue="" Description="" NetType="String" DbType="VARCHAR" />
    <Column Name="value_string" DefaultValue="" Description="" NetType="String" DbType="VARCHAR" Nullable="true" />
    <Column Name="value_boolean" DefaultValue="" Description="If this is a true or false detail, this replaces the value." NetType="Boolean" DbType="BOOL" Nullable="true" />
    <Column Name="note" DefaultValue="" Description="" NetType="String" DbType="TEXT" Nullable="true" />
  </Table>
  <Table Name="ProjectTasks">
    <Column Name="id" DefaultValue="" Description="" NetType="Int32" DbType="INT" IsPrimaryKey="true" IsAutoIncrement="true" />
    <Column Name="Projects_id" DefaultValue="" Description="" NetType="Int32" DbType="INT" />
    <Column Name="ProjectTaskTypes_id" DefaultValue="" Description="" NetType="Int32" DbType="INT" />
    <Column Name="ProjectTaskStatuses_id" DefaultValue="" Description="" NetType="Int32" DbType="INT" />
    <Column Name="priority" DefaultValue="" Description="" NetType="Int16" DbType="SMALLINT" Nullable="true" />
    <Column Name="status" DefaultValue="" Description="" NetType="Boolean" DbType="BOOL" Nullable="true" />
    <Column Name="task" DefaultValue="" Description="" NetType="String" DbType="TEXT" Nullable="true" />
    <Column Name="date_added" DefaultValue="" Description="" NetType="Int64" DbType="BIGINT" />
    <Column Name="date_completed" DefaultValue="" Description="" NetType="Int64" DbType="BIGINT" Nullable="true" />
  </Table>
  <Table Name="ProjectTaskTypes">
    <Column Name="id" DefaultValue="" Description="" NetType="Int32" DbType="INT" IsPrimaryKey="true" IsAutoIncrement="true" />
    <Column Name="type" DefaultValue="" Description="" NetType="String" DbType="VARCHAR" Nullable="true" />
  </Table>
  <Table Name="ProjectTaskStatuses">
    <Column Name="id" DefaultValue="" Description="" NetType="Int32" DbType="INT" IsPrimaryKey="true" IsAutoIncrement="true" />
    <Column Name="status" DefaultValue="" Description="" NetType="String" DbType="VARCHAR" Nullable="true" />
  </Table>
  <Table Name="ProjectTasksUsersInvolved">
    <Column Name="id" DefaultValue="" Description="" NetType="Int32" DbType="INT" IsPrimaryKey="true" IsAutoIncrement="true" />
    <Column Name="Users_id" DefaultValue="" Description="" NetType="Int32" DbType="INT" />
    <Column Name="ProjectTasks_id" DefaultValue="" Description="" NetType="Int32" DbType="INT" />
  </Table>
</Database>
