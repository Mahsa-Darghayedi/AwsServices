using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customers.Application.Domain.DTOs.Messages;

public class CustomerUpdated
{
    public int Id { get; set; } = default;

    public required string UserName { get; init; }

    public required string FullName { get; init; }

    public required string Email { get; init; }

    public required string DateOfBirth { get; init; }
}
