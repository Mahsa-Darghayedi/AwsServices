using Amazon.SQS;
using Customers.Application.DatabaseContext;
using Customers.Application.Domain.Contracts.Messaging;
using Customers.Application.Domain.Contracts.Repositories;
using Customers.Application.Domain.Contracts.Services;
using Customers.Application.Implementations.Messaging;
using Customers.Application.Implementations.Repositories;
using Customers.Application.Implementations.Services;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Customers.Application.Extensions;

public static class ApplicationDependency
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = Assembly.GetAssembly(typeof(IServiceAssemblyMaker));
        services.AddSingleton<ICustomersDbConnectionFactory>(_ =>
        new CustomersDbConnectionFactory(configuration.GetConnectionString("CustomersDb")!));

        services.AddSingleton<DatabaseInitializer>();
        services.AddSingleton<ICustomerService, CustomerService>();
        services.AddSingleton<ICustomerRepository, CustomerRepository>();

        services.AddSingleton<IAmazonSQS, AmazonSQSClient>();
        services.AddSingleton<ISqsMessagePublisher, SqsMessagePublisher>();
        services.Configure<QueueSetting>(configuration.GetSection(QueueSetting.Key));


        services.AddValidatorsFromAssembly(assembly);


        return services;
    }
}
