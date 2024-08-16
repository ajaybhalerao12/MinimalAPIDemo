using Microsoft.AspNetCore.Mvc;
using MinimalAPIDemo.Models;

namespace MinimalAPIDemo.EndpointMappers
{
    public static class EndPointEmployeeV2Mapper
    {

        public static WebApplication MapEmployeeEndpoints2(this WebApplication app)
        {
            // Map a GET endpoint to retrieve all endpoints asynchronously
            app.MapGet("/v2/employees", GetAllEmployeesAsync());

            // Map a Get endpoint to retrive employee with specific ID asynchornously
            app.MapGet("/v2/employees/{id}", GetEmployeeByIDAsync());

            // Create a POST request to create an employee asynchronously
            app.MapPost("v2/employees", CreateEmployeeAsync());


            // Create a PUT request to update the employee asynchronously
            app.MapPut("/v2/employees/{id}", UpdateEmployeeAsync());

            // Create a DELETE request to delete employee asynchronously
            app.MapDelete("/v2/employees/{id}", DeleteEmployeeAsync());


            return app;
        }

        private static Func<int, IEmployeeServiceAsync, ILogger<Program>, Task<IResult>> DeleteEmployeeAsync()
        {
            return async (int id, IEmployeeServiceAsync employeeServiceAsync, ILogger<Program> logger) =>
            {
                try
                {
                    logger.LogInformation($"Delete operation with ID {id} is initiated");
                    bool result = await employeeServiceAsync.DeleteEmployeeAsync(id);
                    if (!result)
                    {
                        logger.LogWarning($"Employee with ID {id} not found");
                        return Results.NotFound(new { Message = $"Employee with ID {id} not found" });
                    }
                    return Results.Ok(result);
                }
                catch (Exception ex)
                {
                    logger.LogError($"An unexpected error occured during delete of employee with ID {id}");
                    return Results.Problem(ex.Message);
                }
            };
        }

        private static Func<int, Employee, IEmployeeServiceAsync, ILogger<Program>, Task<IResult>> UpdateEmployeeAsync()
        {
            return async (int id, Employee updatedEmployee, IEmployeeServiceAsync employeeServiceAsync, ILogger<Program> logger) =>
            {
                try
                {
                    logger.LogInformation($"Update employee with ID {id}");
                    var employee = await employeeServiceAsync.UpdateEmployee(id, updatedEmployee);
                    if (employee == null)
                    {
                        logger.LogWarning($"Unable to update the employee with ID {id}");
                        return Results.NotFound(new { Message = $"Unable to update the employee with ID {id}" });
                    }
                    return Results.Ok(employee);
                }
                catch (Exception ex)
                {
                    logger.LogError($"An error occured while updateing employee with ID {id}");
                    return Results.Problem(ex.Message);
                }
            };
        }

        private static Func<Employee, IEmployeeServiceAsync, ILogger<Program>, Task<IResult>> CreateEmployeeAsync()
        {
            return async (Employee employee, IEmployeeServiceAsync employeeServiceAsync, ILogger<Program> logger) =>
            {
                try
                {
                    logger.LogInformation("Create a new employee asynchronously");
                    var createdEmployee = await employeeServiceAsync.AddEmployeeAsync(employee);
                    return Results.Created($"v2/employees/{createdEmployee.Id}", createdEmployee);
                }
                catch (Exception ex)
                {
                    logger.LogError($"An error occured while creating a new employee");
                    return Results.Problem(ex.Message);
                }
            };
        }

        private static Func<int, IEmployeeServiceAsync, ILogger<Program>, Task<IResult>> GetEmployeeByIDAsync()
        {
            return async (int id, IEmployeeServiceAsync employeeServiceAsync, ILogger<Program> logger) =>
            {
                try
                {
                    logger.LogInformation($"Retrieve an employee with ID {id}");
                    Employee employee = await employeeServiceAsync.GetEmployeeByIdAsync(id);
                    if (employee == null)
                    {
                        logger.LogWarning($"Employee with ID {id} not found");
                        return Results.NotFound(new { Message = $"Employee with ID {id} not found" });
                    }
                    return Results.Ok(employee);
                }
                catch (Exception ex)
                {
                    logger.LogError($"An error occoured while retrieving the employee with ID {id}");
                    ProblemDetails problemDetails = new ProblemDetails
                    {
                        Status = 500,
                        Title = "An unexpected error occured",
                        Detail = ex.Message,
                        Instance = "v2/employees/{id}"
                    };
                    return Results.Problem(problemDetails);
                }
            };
        }

        private static Func<IEmployeeServiceAsync, ILogger<Program>, Task<IResult>> GetAllEmployeesAsync()
        {
            return async (IEmployeeServiceAsync employeeServiceAsync, ILogger<Program> logger) =>
            {
                try
                {
                    logger.LogInformation("Retrieve all employee asynchronously");
                    IEnumerable<Employee> employees = await employeeServiceAsync.GetAllEmployeesAsync();
                    return Results.Ok(employees);
                }
                catch (Exception ex)
                {
                    logger.LogError("An error occured while retrieving all employees");
                    return Results.Problem(ex.Message);
                }
            };
        }
    }
}