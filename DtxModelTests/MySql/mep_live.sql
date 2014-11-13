CREATE TABLE Users (
	id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT ,
	username TEXT NOT NULL ,
	Permissions_id INTEGER NOT NULL ,
	Contacts_id INTEGER NOT NULL ,
	password TEXT NOT NULL ,
	date_registered INTEGER NOT NULL ,
	activation_code TEXT ,
	banned SMALLINT NOT NULL ,
	ban_reason TEXT ,
	last_online INTEGER NOT NULL 
);



CREATE TABLE Settings (
	id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT ,
	property TEXT NOT NULL ,
	value TEXT ,
	description TEXT 
);



CREATE TABLE Logs (
	id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT ,
	Users_id INTEGER NOT NULL ,
	time INTEGER ,
	type SMALLINT ,
	ip INTEGER ,
	message TEXT 
);



CREATE TABLE Permissions (
	id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT ,
	name TEXT ,
	base_permissions_id INTEGER ,
	can_register BOOLEAN ,
	can_login BOOLEAN ,
	can_edit_users BOOLEAN ,
	can_edit_settings BOOLEAN ,
	can_manage_images BOOLEAN ,
	access_xflow BOOLEAN 
);



CREATE TABLE Projects (
	id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT ,
	Companies_id_architect INTEGER ,
	ProjectCategories_id INTEGER ,
	Addresses_id INTEGER ,
	name TEXT ,
	status INTEGER ,
	directory TEXT ,
	date_started INTEGER ,
	date_completed INTEGER ,
	details TEXT 
);



CREATE TABLE Companies (
	id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT ,
	name TEXT ,
	url TEXT ,
	notes TEXT ,
	Addresses_id INTEGER 
);



CREATE TABLE ProjectCategories (
	id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT ,
	name TEXT 
);



CREATE TABLE ProjectPictures (
	id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT ,
	Projects_id INTEGER NOT NULL ,
	description TEXT ,
	display BOOLEAN NOT NULL ,
	featured BOOLEAN NOT NULL 
);



CREATE TABLE Contacts (
	id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT ,
	Addresses_id INTEGER ,
	Companies_id INTEGER ,
	first_name TEXT NOT NULL ,
	last_name TEXT ,
	display TEXT ,
	nickname TEXT ,
	title TEXT ,
	email1 TEXT ,
	email2 TEXT ,
	website TEXT ,
	phone_work TEXT ,
	phone_home TEXT ,
	phone_cell TEXT ,
	fax TEXT ,
	notes TEXT 
);



CREATE TABLE Messages (
	id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT ,
	Users_id INTEGER NOT NULL ,
	Projects_id INTEGER NOT NULL ,
	Contacts_id_to INTEGER NOT NULL ,
	Contacts_id_from INTEGER NOT NULL ,
	Message TEXT ,
	type INTEGER 
);



CREATE TABLE ProjectsContactsList (
	id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT ,
	type TEXT ,
	Projects_id INTEGER NOT NULL ,
	Contacts_id INTEGER NOT NULL 
);



CREATE TABLE Addresses (
	id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT ,
	address TEXT ,
	address2 TEXT ,
	city TEXT ,
	state TEXT ,
	zip TEXT 
);



CREATE TABLE ConstructionDetails (
	id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT ,
	Projects_id INTEGER NOT NULL ,
	Users_id INTEGER NOT NULL ,
	date INTEGER NOT NULL ,
	type TEXT NOT NULL ,
	value_string TEXT ,
	value_boolean BOOLEAN ,
	note TEXT 
);



CREATE TABLE ProjectTasks (
	id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT ,
	Projects_id INTEGER NOT NULL ,
	ProjectTaskTypes_id INTEGER NOT NULL ,
	ProjectTaskStatuses_id INTEGER NOT NULL ,
	priority SMALLINT ,
	status BOOLEAN ,
	task TEXT ,
	date_added INTEGER NOT NULL ,
	date_completed INTEGER 
);



CREATE TABLE ProjectTaskTypes (
	id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT ,
	type TEXT 
);



CREATE TABLE ProjectTaskStatuses (
	id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT ,
	status TEXT 
);



CREATE TABLE ProjectTasksUsersInvolved (
	id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT ,
	Users_id INTEGER NOT NULL ,
	ProjectTasks_id INTEGER NOT NULL 
);



