using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace Customers.DynamoDb.Middlewars;

public class ValidationExceptionMiddleware
{
    private readonly RequestDelegate _request;

    public ValidationExceptionMiddleware(RequestDelegate request)
    {
        _request = request;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _request(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            var error = new ValidationProblemDetails
            {
                Status = (int)HttpStatusCode.BadRequest,
                Extensions =
                {
                    ["traceId"] = context.TraceIdentifier
                },
            };
            foreach (var validationFailure in ex.Errors)
            {
                error.Errors.Add(new KeyValuePair<string, string[]>(
                    validationFailure.PropertyName,
                    [validationFailure.ErrorMessage]));
            }
            await context.Response.WriteAsJsonAsync(error);
        }
    }
}
