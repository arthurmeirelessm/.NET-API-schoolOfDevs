using SchoolOfDevs.Exceptions;
using System.Net;
using System.Text.Json;

namespace SchoolOfDevs.Middleware
{
    public class ErrorHandlerMiddleware
    {
       
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public object JsonSerielizer { get; private set; }

        public async Task Invoke (HttpContext context)
        {
              try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = error switch
                {

                    BadRequestException => (int)HttpStatusCode.BadRequest, //custom application error
                    KeyNotFoundException => (int)HttpStatusCode.NotFound, //Not found error
                    ForbiddenException => (int)HttpStatusCode.Forbidden, //unauthorized
                    _ => (int)HttpStatusCode.InternalServerError
                };


                var result = JsonSerializer.Serialize(new { message = error?.Message });
                await response.WriteAsync(result);
            }
        }

    }
}
