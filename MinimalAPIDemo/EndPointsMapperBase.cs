using MinimalAPIDemo.EndpointMappers;
using MinimalAPIDemo.Models;
namespace MinimalAPIDemo
{
    public static partial class EndPointsMapperBase
    {
        public static WebApplication RegisterEndpoints(this WebApplication app)
        {
            app.MapAuthenticationEndpoints();
            app.MapEmployeeEndpoints();
            app.MapEmployeeEndpoints2();
            app.MapProductEndpoints();

            return app;
        }
    }
}
