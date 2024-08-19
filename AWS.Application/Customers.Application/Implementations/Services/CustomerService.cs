
using Customers.Application.Domain.Contracts.Messaging;
using Customers.Application.Domain.Contracts.Repositories;
using Customers.Application.Domain.Contracts.Services;
using Customers.Application.Domain.DTOs.Mapping;
using Customers.Application.Domain.DTOs.Messages;
using Customers.Application.Domain.DTOs.Requests.CustomerRequest;
using Customers.Application.Domain.DTOs.Responses;
using Customers.Application.Domain.Entities;
using Customers.Application.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customers.Application.Implementations.Services;

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
        if (response)
        {
            CustomerCreated createdDto = model.ToCustomerCreatedMessage();
            await _messagePublisher.SendMessageAsync(createdDto);
        }
        return response;
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<CustomerResponseDto?> GetAsync(int id)
    {
        CustomerModel? model = await _customerRepository.GetAsync(id);
        if (model == null)
            return null;

        return model.ToCustomerResponseDto();
    }
}
