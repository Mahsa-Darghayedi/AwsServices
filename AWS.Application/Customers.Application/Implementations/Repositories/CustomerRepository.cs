using Customers.Application.DatabaseContext;
using Customers.Application.Domain.Contracts.Repositories;
using Customers.Application.Domain.Entities;
using Dapper;
using System.Data;

namespace Customers.Application.Implementations.Repositories;

internal class CustomerRepository : ICustomerRepository
{
    public readonly ICustomersDbConnectionFactory _dbConnectionFactory;


    public CustomerRepository(ICustomersDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<bool> CreateAsync(CustomerModel customer)
    {
        IDbConnection connection = await GetConnection();
        var result = await connection.ExecuteAsync(@"Insert into Customers ( UserName, FullName, Email, DateOfBirth)
                                                    Values ( @UserName, @FullName, @Email, @DateOfBirth)", customer);

        return result > 0;
    }

    public async Task<CustomerModel?> GetAsync(int id)
    {
        var connection =await GetConnection();
        CustomerModel? model = await connection.QueryFirstOrDefaultAsync<CustomerModel>(@"select * from Customers where Id = @ID", new { ID = id });
        return model;
    }
    

    public async Task<bool> IsEmailValid(string email)
    {
        var connection = await  GetConnection();
        CustomerModel? model = await connection.QueryFirstOrDefaultAsync<CustomerModel>(@"Select * from Customers where Email=@Email", new { Email = email });
        return model is null;
    }

    public async Task<bool> IsUserNameValid(string userName)
    {
        var connection = await  GetConnection();
        CustomerModel? customer = await connection.QueryFirstOrDefaultAsync<CustomerModel>(@"select * from Customers where UserName =@UserName", new { UserName = userName }) ;
        return customer is null;
    }

    #region Privates
    private async Task<IDbConnection> GetConnection()
    {
        return await _dbConnectionFactory.CreateConnectionAsync();
    }
    #endregion

}
