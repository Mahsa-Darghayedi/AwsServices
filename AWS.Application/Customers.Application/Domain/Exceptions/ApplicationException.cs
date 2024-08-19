using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Customers.Application.Domain.Exceptions;

public class ApplicationException : Exception
{
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.InternalServerError;
    public new string Message { get; set; } = "One or more error occured.";
}

public static class ApplicationExceptionHandler
{
    public static void ThrowIf(this ApplicationException exception, bool condition)
    {
        if (condition)
            throw exception;
    }
}


public class DuplicateItemException : ApplicationException
{
    public DuplicateItemException(string propName)
    {
        StatusCode = HttpStatusCode.BadRequest;
        Message = $"The {propName ?? "Property "} is duplicate.";
    }


}
