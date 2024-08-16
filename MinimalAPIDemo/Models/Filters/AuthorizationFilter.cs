
namespace MinimalAPIDemo.Models.Filters
{
    public class AuthorizationFilter : IEndpointFilter
    {
        private readonly ILogger<AuthorizationFilter> logger;

        public AuthorizationFilter(ILogger<AuthorizationFilter> logger)
        {
            this.logger = logger;
        }
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context,
            EndpointFilterDelegate next)
        {
            var user = context.HttpContext.User;

            if(!user.Identity?.IsAuthenticated ?? true)
            {
                return Results.Unauthorized();
            }
            // If the user is authenticated continue executing the next filter or 
            // the endpoint action.
            return await next(context);
        }
    }
}
