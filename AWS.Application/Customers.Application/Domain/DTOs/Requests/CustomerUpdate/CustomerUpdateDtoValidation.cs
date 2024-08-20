using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customers.Application.Domain.DTOs.Requests.CustomerUpdate
{
    public class CustomerUpdateDtoValidation : AbstractValidator<CustomerUpdateDto>
    {
        public CustomerUpdateDtoValidation()
        {
            RuleFor(c => c.Id)
                .NotEmpty()
                .GreaterThan(0).WithMessage("Invalid Request");

            RuleFor(c => c.Customer)
                .NotNull().WithMessage("Invalid Request"); ;
        }
    }
}
