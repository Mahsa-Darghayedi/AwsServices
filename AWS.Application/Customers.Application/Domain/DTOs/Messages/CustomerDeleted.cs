using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customers.Application.Domain.DTOs.Messages;

public class CustomerDeleted
{
    public required int Id { get; init; }
}
