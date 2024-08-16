﻿

using Microsoft.EntityFrameworkCore;
using MinimalAPIDemo.Models;

namespace MinimalAPIDemo
{
    public static partial class ServiceInitializer
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services,IConfiguration configuration)
        {            
            RegisterSwaggerServices(services);
            RegisterCustomServices(services);
            RegisterDBContext(services,configuration);
            return services;
        }

        private static void RegisterDBContext(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<ApplicationDBContext>(options =>
     options.UseMySql(configuration.GetConnectionString("DBConn"),
         new MySqlServerVersion(new Version(8, 4, 0))));
        }

        private static void RegisterCustomServices(IServiceCollection services)
        {
            // Register EmployeeService in DI container
            services.AddSingleton<IEmployeeService, EmployeeService>();
            services.AddScoped<IEmployeeServiceAsync, EmployeeServiceAsync>();

            // Register the Products service in DI container
            services.AddScoped<IProductsService, ProductService>();
        }

        private static void RegisterSwaggerServices(IServiceCollection services)
        {
            // Add services to the container.
            // Add API explorer for endpoint documentation            
            services.AddEndpointsApiExplorer();
            // Add Swagger for API documentation
            services.AddSwaggerGen();
        }
    }
}


