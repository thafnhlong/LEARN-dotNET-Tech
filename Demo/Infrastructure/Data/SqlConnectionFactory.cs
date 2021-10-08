using Domain.Common;
using Microsoft.Data.Sqlite;
using System;
using System.Data;

namespace Infrastructure.Data
{
    public class SqlConnectionFactory : ISqlConnectionFactory, IDisposable
    {
        private readonly string _connectionString;
        private IDbConnection _connection;

        public SqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Dispose()
        {
            if (_connection != null && _connection.State == ConnectionState.Open) {
                _connection.Dispose();
            }
        }

        public IDbConnection GetOpenConnection()
        {
            if (_connection == null || _connection.State != ConnectionState.Open) {
                _connection = new SqliteConnection(_connectionString);
                _connection.Open();
            }

            return _connection;
        }
    }
}