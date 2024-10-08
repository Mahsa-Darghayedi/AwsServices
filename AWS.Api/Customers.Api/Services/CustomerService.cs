using Customers.Application.Domain.Contracts.Messaging;
using Customers.Application.Domain.Contracts.Repositories;
using Customers.Application.Domain.Contracts.Services;
using Customers.Application.Domain.DTOs.Mapping;
using Customers.Application.Domain.DTOs.Messages;
using Customers.Application.Domain.DTOs.Requests.CustomerRequest;
using Customers.Application.Domain.DTOs.Requests.CustomerUpdate;
using Customers.Application.Domain.DTOs.Responses;
using Customers.Application.Domain.Entities;
using Customers.Application.Domain.Exceptions;
namespace Customers.Api.Services;

internal class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    private readonly ISqsMessagePublisher _messagePublisher;

    public CustomerService(ICustomerRepository customerRepository, ISqsMessagePublisher messagePublisher)
    {
        _customerRepository = customerRepository;
        _messagePublisher = messagePublisher;
    }



    public async Task<bool> CreateAsync(CustomerRequestDto customer)
    {

        new DuplicateItemException(nameof(customer.UserName)).ThrowIf(!await _customerRepository.IsUserNameValid(customer.UserName));
        new DuplicateItemException(nameof(customer.Email)).ThrowIf(!await _customerRepository.IsEmailValid(customer.Email));

        CustomerModel model = new()
        {

            UserName = customer.UserName,
            FullName = customer.FullName,
            Email = customer.Email,
            DateOfBirth = customer.DateOfBirth,
        };
        var response = await _customerRepository.CreateAsync(model);
        new OperationFaildException().ThrowIf(!response);

        if (response)
        {
            CustomerCreated createdDto = model.ToCustomerCreatedMessage();
            await _messagePublisher.SendMessageAsync(createdDto);
        }
        return response;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var result = await _customerRepository.DeleteAsync(id);
        new OperationFaildException().ThrowIf(!result);
        await _messagePublisher.SendMessageAsync(new CustomerDeleted() { Id = id });
        return result;

    }


    public async Task<IReadOnlyCollection<CustomerResponseDto>> GetAllAsync()
    {
        var models = await _customerRepository.GetAllAsync();
        if (models == null)
            return new List<CustomerResponseDto>().AsReadOnly();

        return models.ToCustomerResponseDto();
    }

    public async Task<CustomerResponseDto?> GetAsync(int id)
    {
        CustomerModel? model = await _customerRepository.GetAsync(id);
        if (model == null)
            return null;

        return model.ToCustomerResponseDto();
    }

    public async Task<CustomerResponseDto> UpdateCustomer(CustomerUpdateDto dto)
    {
        var model = dto.ToCustomerModel();
        bool result = await _customerRepository.UpdateAsync(model);
        new OperationFaildException().ThrowIf(!result);

        CustomerUpdated createdDto = model.ToCustomerUpdatedMessage();
        await _messagePublisher.SendMessageAsync(createdDto);

        return model.ToCustomerResponseDto();
    }
}
