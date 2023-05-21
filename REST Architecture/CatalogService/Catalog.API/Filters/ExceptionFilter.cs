using Catalog.Models.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Catalog.API.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var statusCode = ResolveStatusCode(exception);
            context.Result = new StatusCodeResult(statusCode);
            context.ExceptionHandled = true;
        }

        private static int ResolveStatusCode(Exception exception) => exception switch
        {
            ApiException apiException => (int)apiException.StatusCode,
            _ => (int)HttpStatusCode.InternalServerError,
        };
    }
}
