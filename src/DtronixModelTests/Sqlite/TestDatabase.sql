CREATE TABLE Users (
	rowid INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT ,
	username TEXT NOT NULL ,
	password TEXT NOT NULL ,
	last_logged INTEGER NOT NULL 
);



CREATE TABLE Logs (
	rowid INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT ,
	Users_rowid INTEGER NOT NULL ,
	text TEXT NOT NULL 
);



CREATE TABLE AllTypes (
	id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT ,
	db_int16 INTEGER ,
	db_int32 INTEGER ,
	db_int64 INTEGER ,
	db_uint16 INTEGER ,
	db_uint32 INTEGER ,
	db_uint64 INTEGER ,
	db_byte_array BLOB ,
	db_byte BLOB ,
	db_date_time DATETIME ,
	db_decimal REAL ,
	db_float FLOAT ,
	db_double DOUBLE ,
	db_bool BOOLEAN ,
	db_string TEXT ,
	db_enum INTEGER NOT NULL 
);



