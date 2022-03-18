using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace DtronixModel
{
    /// <summary>
    /// Contains reader and disposal logic for reading from a table.  Must dispose.
    /// </summary>
    /// <typeparam name="T">Table Row to work with.</typeparam>
    public sealed class SqlStatementReader<T> : IDisposable, IAsyncDisposable
        where T : TableRow, new()
    {
        private readonly SqlStatement<T> _statement;

        /// <summary>
        /// Database reader to use for reading from the executed SQL statement.
        /// </summary>
        public DbDataReader Reader { get; }

        internal SqlStatementReader(SqlStatement<T> statement, DbDataReader reader)
        {
            _statement = statement;
            Reader = reader;
        }

        /// <summary>
        /// Releases all resources held by this reader.
        /// </summary>
        public void Dispose()
        {
            Reader.Dispose();
            _statement.Command.Dispose();
        }

        /// <summary>
        /// Asynchronously releases all resources held by this reader.
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            await Reader.DisposeAsync();
            await _statement.Command.DisposeAsync();
        }
    }
}
