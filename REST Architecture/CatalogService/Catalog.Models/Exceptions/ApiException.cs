using System.Net;

namespace Catalog.Models.Exceptions
{
    public class ApiException : Exception
    {
        public ApiException(HttpStatusCode statusCode, string message = null) 
            : base(message) => StatusCode = statusCode;

        public HttpStatusCode StatusCode { get; set; }
    }
}
