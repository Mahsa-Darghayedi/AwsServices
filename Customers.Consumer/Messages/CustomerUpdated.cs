using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customers.Consumer.Messages;

public class CustomerUpdated : ISqsMessage
{
    public int Id { get; set; } = default;

    public required string UserName { get; init; }

    public required string FullName { get; init; }

    public required string Email { get; init; }

    public required string DateOfBirth { get; init; }
}


public class CustomerUpdatedHandler : IRequestHandler<CustomerUpdated>
{
    private readonly ILogger<CustomerUpdatedHandler> _logger;
    public CustomerUpdatedHandler(ILogger<CustomerUpdatedHandler> logger)
    {
        _logger = logger;
    }
    public Task Handle(CustomerUpdated request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Customer {FullName} is up to date now.", request.FullName);
        return Task.CompletedTask;
    }
}
