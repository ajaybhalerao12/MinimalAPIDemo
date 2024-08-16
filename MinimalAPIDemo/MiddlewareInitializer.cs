using MinimalAPIDemo.Models;

namespace MinimalAPIDemo
{
    public static partial class MiddlewareInitializer
    {
        public static WebApplication ConfigureMiddleware(this WebApplication app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
            // Configure the HTTP request pipeline for the development pipeline.
            if (app.Environment.IsDevelopment())
            {
                // Use Swagger middleware for swagger documentation
                app.UseSwagger();
                // Use Swagger UI middleware to interact with Swagger documentation
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            return app;
        }
    }
}
