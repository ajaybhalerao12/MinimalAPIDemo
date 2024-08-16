using Microsoft.AspNetCore.Mvc;
using MinimalAPIDemo.Models;

namespace MinimalAPIDemo.EndpointMappers
{
    public static class EndPointEmployeeMapper
    {

        // Endpoint to retrieve all the employees
        public static WebApplication MapEmployeeEndpoints(this WebApplication app)
        {
            // Map a GET request to /employees to return employees list
            app.MapGet("/employees", GetAllEmployees());
            // Endpoint to retrieve single employee using the employee ID
            app.MapGet("/employees/{id}", GetEmployeeById());


            // Endpoint to create a new employee
            app.MapPost("/employees", CreateEmployee());

            app.MapPut("/employees/{id}", UpdateEmployee());

            // Endpoint to delete the employe using ID
            app.MapDelete("/employeed/{id}", DeleteEmployee());

            return app;
        }

        private static Func<IEmployeeService, ILogger<Program>, IResult> GetAllEmployees()
        {
            return (employeeService, logger) =>
            {
                try
                {
                    logger.LogInformation("Retrieve all employees");
                    return Results.Ok(employeeService.GetAllEmployees());
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occured while retrieving the employees");
                    return Results.Problem(ex.Message);
                }
            };
        }
        private static Func<int, IEmployeeService, ILogger<Program>, IResult> GetEmployeeById()
        {
            return (id, employeeService, logger) =>
            {

                try
                {
                    logger.LogInformation($"Retrieve the employe by employee id:{id}");
                    // create a scenario to throw the exception
                    //int x = 10, y = 0;
                    //int result = x / y;

                    var employee = employeeService.GetEmployeeById(id);
                    if (employee == null)
                    {
                        logger.LogWarning("Employee with ID {id} not found", id);
                        return Results.NotFound(new { Message = $"Employee with ID  {id} not found" });
                    }
                    return Results.Ok(employee);
                    //return employee is not null ? Results.Ok(employee) : Results.NotFound();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"An error occured while retrieving the employee with ID: {id}", id);
                    var problemDetails = new ProblemDetails
                    {
                        Status = 500,
                        Title = "An unexpected error occured",
                        Detail = ex.Message,
                        Instance = $"employee/{id}"
                    };

                    return Results.Problem(problemDetails);
                    //return Results.Problem($"{ex.Message}");
                }
            };
        }
        private static Func<Employee, IEmployeeService, ILogger<Program>, IResult> CreateEmployee()
        {
            return (newEmployee, employeeService, logger) =>
            {
                try
                {
                    logger.LogInformation("Create a new employee");
                    if (!ValidationHelper.TryValidate(newEmployee, out var validationResults))
                    {
                        // Returns 400 Bad Request if validation fails.
                        return Results.BadRequest(validationResults);
                    }
                    var createdEmployee = employeeService.AddEmployee(newEmployee);
                    return Results.Created($"/employees/{createdEmployee.Id}", createdEmployee);
                }
                catch (Exception ex)
                {
                    logger.LogError("An error occured while creating employee");
                    return Results.Problem(ex.Message);
                }


                // retrieve the employee ID
                //newEmployee.Id = employeesList.Count > 0 ? employeesList.Max(emp => emp.Id + 1) : 1;
                //employeesList.Add(newEmployee);

                //return Results.Created($"/employees/{newEmployee.Id}", newEmployee);
            };
        }

        private static Func<int, Employee, IEmployeeService, ILogger<Program>, IResult> UpdateEmployee()
        {
            return (id, newEmployee, employeeService, logger) =>
            {
                try
                {
                    logger.LogInformation($"Retrieve the employe with ID {id}");
                    if (!ValidationHelper.TryValidate(newEmployee, out var validationResults))
                    {
                        logger.LogWarning($"Employee with ID {id} not found");
                        // Returns 400 Bad Request if the validation fails
                        return Results.BadRequest(validationResults);
                    }

                    Employee? employee = employeeService.UpdateEmployee(id, newEmployee);
                    return employee is not null ? Results.Ok(employee) : Results.NotFound();
                }
                catch (Exception ex)
                {
                    logger.LogError($"{ex.Message}", $"An error occured while retrieving the employee with ID {id}");
                    return Results.Problem($"{ex.Message}");
                }


                //// Find the employee using ID to be updated
                //Employee? employeeToBeUpdated = employeesList.FirstOrDefault(emp => emp.Id == id);

                //if (employeeToBeUpdated is null) return Results.NotFound();

                //employeeToBeUpdated.Name = newEmployee.Name;
                //employeeToBeUpdated.Position = newEmployee.Position;
                //employeeToBeUpdated.Salary = newEmployee.Salary;

                //return Results.Ok(employeeToBeUpdated);

            };
        }

        private static Func<int, IEmployeeService, ILogger<Program>, IResult> DeleteEmployee()
        {
            return (id, employeeService, logger) =>
            {

                try
                {
                    logger.LogInformation($"Delete the employe with ID {id}");
                    var result = employeeService.DeleteEmployee(id);
                    if (!result)
                    {
                        logger.LogWarning($"Delete the employee with ID {id} not found");
                    }
                    return result ? Results.NoContent() : Results.NotFound();
                }
                catch (Exception ex)
                {
                    logger.LogInformation(ex.Message, $"An error occured to delete employee with ID {id}");
                    return Results.Problem($"{ex.Message}");
                }

                //// retrieve the employee to be deleted using ID
                //var employeeToBeDeleted = employeesList.FirstOrDefault(emp => emp.Id == id);
                //if (employeeToBeDeleted is null) return Results.NotFound(); // If not found then return 404

                //// Remove the employee from the list
                //employeesList.Remove(employeeToBeDeleted);

                //// Return a 204 No Content Response
                //return Results.NoContent();
            };
        }
    }
}