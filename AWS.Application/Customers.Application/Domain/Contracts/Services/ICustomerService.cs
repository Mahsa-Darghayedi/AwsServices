
using Customers.Application.Domain.DTOs.Requests.CustomerRequest;

namespace Customers.Application.Domain.Contracts.Services;

public interface ICustomerService
{
    Task<bool> CreateAsync(CustomerRequestDto customer);
    Task<bool> DeleteAsync(Guid id);
}
