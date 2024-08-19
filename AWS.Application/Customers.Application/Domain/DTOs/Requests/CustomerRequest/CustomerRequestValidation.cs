using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Customers.Application.Domain.DTOs.Requests.CustomerRequest;

public partial class CustomerRequestValidation : AbstractValidator<CustomerRequestDto>
{
    public CustomerRequestValidation()
    {
        RuleFor(x => x.FullName)
            .NotNull().NotEmpty();


        RuleFor(x => x.Email)
            .EmailAddress();

        RuleFor(x => x.UserName)
                       .NotNull().NotEmpty()
                       .Matches(UserNameRegex());
    }


    [GeneratedRegex("^[a-zA-Z0-9]+$", RegexOptions.IgnoreCase)]
    private static partial Regex UserNameRegex();
}
