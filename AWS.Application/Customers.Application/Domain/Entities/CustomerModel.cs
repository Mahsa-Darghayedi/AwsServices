using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customers.Application.Domain.Entities;

public class CustomerModel
{

    [Key]
    public int Id { get; set; } = default;


    public required string UserName { get; init; }

    public required string FullName { get; init; }

    public required string Email { get; init; }


    public required string DateOfBirth { get; init; }

}
