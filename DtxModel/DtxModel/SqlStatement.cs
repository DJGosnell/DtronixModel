using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace DtxModel {
    class SqlStatement {
        public enum Mode {
            Select,
            Insert,
            Update,
            Delete
        }

        public DbConnection connection;


        public static SqlStatement Select {
            get {
                return new SqlStatement(Mode.Select);
            }
        }

        public static SqlStatement Update {
            get {
                return new SqlStatement(Mode.Update);
            }
        }

        public static SqlStatement Insert {
            get {
                return new SqlStatement(Mode.Insert);
            }
        }
        
        public static SqlStatement Delete {
            get {
                return new SqlStatement(Mode.Delete);
            }
        }


        private Mode mode;

        public SqlStatement(Mode mode) {
            this.mode = mode;
        }

        public 
    }
}
