using System.Net;
using System.Text.Json;

namespace MinimalAPIDemo.Models
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }

        }
        private static Task HandleException(HttpContext context, Exception ex)
        {

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var result = JsonSerializer.Serialize(new { Messsage = "An exception occured.", Detail = ex.Message });
            return context.Response.WriteAsync(result);
        }
    }
}
