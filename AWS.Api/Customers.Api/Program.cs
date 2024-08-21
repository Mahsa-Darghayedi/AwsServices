using Amazon.SQS;
using Customers.Api.Middlewars;
using Customers.Api.Services;
using Customers.Api.SqsPublisher.Messaging;
using Customers.Application.DatabaseContext;
using Customers.Application.Domain.Contracts.Messaging;
using Customers.Application.Domain.Contracts.Services;
using Customers.Application.Extensions;
using Customers.Application.Implementations.Messaging;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();



builder.Services.AddSingleton<ICustomerService, CustomerService>();
builder.Services.AddSingleton<ISqsMessagePublisher, SqsMessagePublisher>();
builder.Services.Configure<QueueSetting>(builder.Configuration.GetSection(QueueSetting.Key));
builder.Services.AddSingleton<IAmazonSQS, AmazonSQSClient>();



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});
builder.Services.AddApplicationService(builder.Configuration);


var app = builder.Build();
app.UseExceptionHandler(new ExceptionHandlerOptions
{
    ExceptionHandlingPath = "/error",
    AllowStatusCode404Response = true
});


app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));


app.UseHttpsRedirection();
app.UseMiddleware<ValidationExceptionMiddleware>();
app.MapControllers();

var databasInitilizaer = app.Services.GetRequiredService<DatabaseInitializer>();
await databasInitilizaer.InitilizeDbAsync();


app.Run();


