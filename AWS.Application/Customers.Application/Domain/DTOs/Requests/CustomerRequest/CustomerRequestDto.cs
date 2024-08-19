using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customers.Application.Domain.DTOs.Requests.CustomerRequest;

public class CustomerRequestDto
{
    public string UserName { get; init; } = default!;

    public string FullName { get; init; } = default!;

    public string Email { get; init; } = default!;

    public string DateOfBirth { get; init; } = default!;
}
