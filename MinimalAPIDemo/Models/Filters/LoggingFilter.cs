namespace MinimalAPIDemo.Models.Filters
{
    public class LoggingFilter : IEndpointFilter
    {
        private readonly ILogger<LoggingFilter> _logger;

        public LoggingFilter(ILogger<LoggingFilter> logger)
        {
            _logger = logger;
        }
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context,
            EndpointFilterDelegate next)
        {
            _logger.LogInformation("Handling request: {RequestPath}",
                context.HttpContext.Request.Path);

            var result = await next(context);

            _logger.LogInformation($"Finished handling request: " +
                $"{context.HttpContext.Request.Path}");
            return result;
        }
    }
}
