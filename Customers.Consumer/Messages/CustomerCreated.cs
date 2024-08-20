using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Customers.Consumer.Messages;

public class CustomerCreated : ISqsMessage
{
    public int Id { get; set; } = default;

    public required string UserName { get; init; }

    public required string FullName { get; init; }

    public required string Email { get; init; }

    public required string DateOfBirth { get; init; }

}

public class CustomerCreatedHandler : IRequestHandler<CustomerCreated>
{
    private readonly ILogger<CustomerCreatedHandler> _logger;

    public CustomerCreatedHandler(ILogger<CustomerCreatedHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(CustomerCreated request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Customer Created Message received. Customer Name is : {FullName}", request.FullName);
        return Task.CompletedTask;
    }
}