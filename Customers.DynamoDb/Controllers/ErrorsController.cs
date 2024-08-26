using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Customers.DynamoDb.Controllers;


public class ErrorsController : ControllerBase
{
    private readonly ILogger<ErrorsController> _logger;

    public ErrorsController(ILogger<ErrorsController> logger)
    {
        _logger = logger;
    }

    [Route("/error")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult Error()
    {
        Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;


        _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);


        var (statusCode, message) = exception switch
        {
            Customers.Application.Domain.Exceptions.ApplicationException ex => ((int)ex.StatusCode, ex.Message),
            _ => (StatusCodes.Status500InternalServerError, exception.Message)
        };
        return Problem(statusCode: statusCode, title: message);

    }
}
