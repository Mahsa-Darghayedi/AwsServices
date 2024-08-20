
using Customers.Application.Domain.DTOs.Requests.CustomerRequest;
using Customers.Application.Domain.DTOs.Requests.CustomerUpdate;
using Customers.Application.Domain.DTOs.Responses;
using Customers.Application.Domain.Entities;

namespace Customers.Application.Domain.Contracts.Services;

public interface ICustomerService
{
    Task<bool> CreateAsync(CustomerRequestDto customer);
    Task<bool> DeleteAsync(int id);
    Task<IReadOnlyCollection<CustomerResponseDto>> GetAllAsync();
    Task<CustomerResponseDto?> GetAsync(int id);
    Task<CustomerResponseDto> UpdateCustomer(CustomerUpdateDto dto);
}
