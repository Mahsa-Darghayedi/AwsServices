
using Customers.Application.Domain.DTOs.Requests.CustomerRequest;
using Customers.Application.Domain.DTOs.Responses;

namespace Customers.Application.Domain.Contracts.Services;

public interface ICustomerService
{
    Task<bool> CreateAsync(CustomerRequestDto customer);
    Task<bool> DeleteAsync(Guid id);
    Task<CustomerResponseDto?> GetAsync(int id);
}
