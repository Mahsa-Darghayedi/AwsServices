using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customers.Application.DatabaseContext;

public class DatabaseInitializer
{
    private readonly ICustomersDbConnectionFactory _connectionFactory;
    public DatabaseInitializer(ICustomersDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
    public async Task InitilizeDbAsync()
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        await connection.ExecuteAsync(@" create table if not exists Customers (
                Id INTEGER Primary key AUTOINCREMENT,
                UserName Text Not Null,
                FullName Text Not Null,
                Email Text Not Null,
                DateOfBirth Text Not Null)");
    }
}
