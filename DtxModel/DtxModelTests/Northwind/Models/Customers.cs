using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace DtxModelTests.Northwind.Models {
    class Customers {

        private SQLiteConnection connection;

        private long _rowid;

        public long rowid {
            get { return _rowid; }
            set { _rowid = value; }
        }

        private string _CustomerID;

        public string CustomerID {
            get { return _CustomerID; }
            set { _CustomerID = value; }
        }

        private string _CompanyName;

        public string CompanyName {
            get { return _CompanyName; }
            set { _CompanyName = value; }
        }

        private string _ContactName;

        public string ContactName {
            get { return _ContactName; }
            set { _ContactName = value; }
        }

        private string _Address;

        public string Address {
            get { return _Address; }
            set { _Address = value; }
        }

        private string _City;

        public string City {
            get { return _City; }
            set { _City = value; }
        }

        private string _Region;

        public string Region {
            get { return _Region; }
            set { _Region = value; }
        }

        private string _PostalCode;

        public string PostalCode {
            get { return _PostalCode; }
            set { _PostalCode = value; }
        }

        private string _Country;

        public string Country {
            get { return _Country; }
            set { _Country = value; }
        }

        private string _Phone;

        public string Phone {
            get { return _Phone; }
            set { _Phone = value; }
        }

        private string _Fax;

        public string Fax {
            get { return _Fax; }
            set { _Fax = value; }
        }

        public Customers(SQLiteConnection connection) {
            this.connection = connection;
        }

        public getBy


    }
}
