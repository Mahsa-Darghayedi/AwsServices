using Customers.Application.Domain.DTOs.Requests;
using Customers.Application.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customers.Application.Domain.Contracts.Repositories;

public interface ICustomerRepository
{
    Task<bool> CreateAsync(CustomerModel customer);
    Task<bool> IsUserNameValid(string userName);
    Task<bool> IsEmailValid(string email);
    Task<CustomerModel?> GetAsync(int id);
}
