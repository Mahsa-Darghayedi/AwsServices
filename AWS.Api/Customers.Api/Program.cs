using Customers.Api.Middlewars;
using Customers.Application.DatabaseContext;
using Customers.Application.Extensions;
using Customers.Application.Implementations.Messaging;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();

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


