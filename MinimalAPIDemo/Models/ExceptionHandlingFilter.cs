
namespace MinimalAPIDemo.Models
{
    public class ExceptionHandlingFilter : IEndpointFilter
    {
        private readonly ILogger<ExceptionHandlingFilter> _logger;

        public ExceptionHandlingFilter(ILogger<ExceptionHandlingFilter> logger)
        {
            _logger = logger;
        }
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context,
            EndpointFilterDelegate next)
        {
            try
            {
                return await next(context);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An unhandled exception has occured");
                return Results.Problem("An unexpected error has occured." +
                    " Please try again later");
            }
            
        }
    }
}
