using MinimalAPIDemo.Models;

namespace MinimalAPIDemo.EndpointMappers
{
    internal static class EndPointsAuthenticationMapper
    {
        public static WebApplication MapAuthenticationEndpoints(this WebApplication app)
        {
            app.MapPost("/authenticate", AuthenticateUser()).WithTags("Authentication");
            return app;
        }

        private static Func<AuthService, string, string, IResult> AuthenticateUser()
        {
            return (AuthService authService, string username, string password) =>
            {
                // Here, you should validate the username and password against your storage
                // The provided credentials are hardcoded for demonstration.
                // In a production scenario, you would typically query a database
                // or another secure storage mechanism to validate the credentials.
                // This is a simple example with hardcoded values
                if (username == "user" && password == "pass@123")
                {
                    var token = authService.GenerateJwtToken(username);
                    return Results.Ok(new { Token = token });
                }
                return Results.Unauthorized();
            };
        }
    }
}