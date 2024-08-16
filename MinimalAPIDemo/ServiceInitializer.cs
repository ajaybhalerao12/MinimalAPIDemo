

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MinimalAPIDemo.Models;
using MinimalAPIDemo.Models.Products;
using System.Text;

namespace MinimalAPIDemo
{
    public static partial class ServiceInitializer
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services,IConfiguration configuration)
        {
            RegisterSwaggerServices(services);
            RegisterCustomServices(services);            
            RegisterAuthenticationAuthorizationServices(services, configuration);

            RegisterDBContext(services, configuration);
            return services;
        }
        
        private static void RegisterAuthenticationAuthorizationServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                            .AddJwtBearer(options =>
                            {
                                options.TokenValidationParameters = new TokenValidationParameters()
                                {
                                    ValidateIssuer = true,
                                    ValidateAudience = true,
                                    ValidateLifetime = true,
                                    ValidateIssuerSigningKey = true,
                                    ValidIssuer = configuration["Jwt:Issuer"],
                                    ValidAudience = configuration["Jwt:Audience"],
                                    IssuerSigningKey = new
                                    SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                                };
                            });
            services.AddAuthorization();
        }

        private static void RegisterDBContext(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<ApplicationDBContext>(options =>
     options.UseMySql(configuration.GetConnectionString("DBConn"),
         new MySqlServerVersion(new Version(8, 4, 0))));
        }

        private static void RegisterCustomServices(IServiceCollection services)
        {
            // Add JWT token Authentication services
            services.AddSingleton<AuthService>();

            // Register EmployeeService in DI container
            services.AddSingleton<IEmployeeService, EmployeeService>();
            services.AddSingleton<IEmployeeServiceAsync, EmployeeServiceAsync>();

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


