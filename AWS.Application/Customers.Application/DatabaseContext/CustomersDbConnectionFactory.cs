using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customers.Application.DatabaseContext
{
    public interface ICustomersDbConnectionFactory
    {
        public Task<IDbConnection> CreateConnectionAsync();
    }
    public class CustomersDbConnectionFactory : ICustomersDbConnectionFactory
    {
        private readonly string _connectionString;
        public CustomersDbConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<IDbConnection> CreateConnectionAsync()
        {
           var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}
