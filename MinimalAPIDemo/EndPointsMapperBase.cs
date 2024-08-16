using Microsoft.AspNetCore.Mvc;
using MinimalAPIDemo.EndpointMappers;
using MinimalAPIDemo.Models;

namespace MinimalAPIDemo
{
    public static partial class EndPointsMapperBase
    {
        public static WebApplication RegisterEndpoints(this WebApplication app)
        {
            // Endpoint to retrieve all the employees
            app.MapEmployeeEndpoints();
            app.MapEmployeeEndpoints2();
            app.MapProductEndpoints();

            return app;
        }
    }
}
