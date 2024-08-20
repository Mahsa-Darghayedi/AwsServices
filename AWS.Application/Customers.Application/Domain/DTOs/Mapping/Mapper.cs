using Customers.Application.Domain.DTOs.Messages;
using Customers.Application.Domain.DTOs.Requests.CustomerUpdate;
using Customers.Application.Domain.DTOs.Responses;
using Customers.Application.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Customers.Application.Domain.DTOs.Mapping;

public static class Mapper
{
    public static CustomerCreated ToCustomerCreatedMessage(this CustomerModel model)
        => new()
        {
            Id = model.Id,
            UserName = model.UserName,
            Email = model.Email,
            FullName = model.FullName,
            DateOfBirth = model.DateOfBirth
        };

    public static CustomerUpdated ToCustomerUpdatedMessage(this CustomerModel model)
    => new()
    {
        Id = model.Id,
        UserName = model.UserName,
        Email = model.Email,
        FullName = model.FullName,
        DateOfBirth = model.DateOfBirth
    };


    public static CustomerResponseDto ToCustomerResponseDto(this CustomerModel model)
      => new()
      {
          Id = model.Id,
          UserName = model.UserName,
          Email = model.Email,
          FullName = model.FullName,
          DateOfBirth = model.DateOfBirth
      };

    public static CustomerModel ToCustomerModel(this CustomerUpdateDto dto)
    => new()
    {
        Id = dto.Id,
        UserName = dto.Customer.UserName,
        Email = dto.Customer.Email,
        FullName = dto.Customer.FullName,
        DateOfBirth = dto.Customer.DateOfBirth
    };


    public static IReadOnlyCollection<CustomerResponseDto> ToCustomerResponseDto(this IReadOnlyCollection<CustomerModel> models)
        => models.Select(m => m.ToCustomerResponseDto()).ToList().AsReadOnly();
}
