using Amazon.SimpleNotificationService;
using Customers.Application.DatabaseContext;
using Customers.Application.Domain.Contracts.Services;
using Customers.SNS.Middlewars;
using Customers.SNS.Services;
using Customers.SNS.SnsPublisher;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<TopicSetting>(builder.Configuration.GetSection(TopicSetting.Key));
builder.Services.AddSingleton<ICustomerService, CustomerService>();
builder.Services.AddSingleton<IAmazonSimpleNotificationService, AmazonSimpleNotificationServiceClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<ValidationExceptionMiddleware>();

app.MapControllers();



var databasInitilizaer = app.Services.GetRequiredService<DatabaseInitializer>();
await databasInitilizaer.InitilizeDbAsync();
app.Run();
