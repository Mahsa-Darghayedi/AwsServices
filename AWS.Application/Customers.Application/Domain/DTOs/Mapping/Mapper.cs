using Customers.Application.Domain.DTOs.Messages;
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
}
