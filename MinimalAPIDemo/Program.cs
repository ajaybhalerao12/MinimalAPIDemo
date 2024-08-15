using MinimalAPIDemo.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add API explorer for endpoint documentation
builder.Services.AddEndpointsApiExplorer();
// Add Swagger for API documentation
builder.Services.AddSwaggerGen();

// Register EmployeeService in DI container
builder.Services.AddSingleton<IEmployeeService, EmployeeService>();

var app = builder.Build();

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

// Endpoint to retrieve all the employees
// Map a GET request to /employees to return employees list
app.MapGet("/employees", (IEmployeeService employeeService) =>
{
    try
    {
        return Results.Ok(employeeService.GetAllEmployees());
    }
    catch(Exception ex)
    {
        return Results.Problem(ex.Message);
    }
    
});


// Endpoint to retrieve single employee using the employee ID
app.MapGet("/employees/{id}", (int id, IEmployeeService employeeService) =>
{

    try
    {
        // create a scenario to throw the exception
        int x = 10, y = 0;
        int result = x / y;

        var employee = employeeService.GetEmployeeById(id);
        return employee is not null ? Results.Ok(employee) : Results.NotFound();
    }
    catch (Exception ex) { 
        return Results.Problem($"{ex.Message}");
    }
    
});

// Endpoint to create a new employee
app.MapPost("/employees", (Employee newEmployee, IEmployeeService employeeService) =>
{
    try
    {
        if (!ValidationHelper.TryValidate(newEmployee, out var validationResults))
        {
            // Returns 400 Bad Request if validation fails.
            return Results.BadRequest(validationResults);
        }
        var createdEmployee = employeeService.AddEmployee(newEmployee);
        return Results.Created($"/employees/{createdEmployee.Id}", createdEmployee);
    }
    catch(Exception ex)
    {
        return Results.Problem(ex.Message);
    }

  
    // retrieve the employee ID
    //newEmployee.Id = employeesList.Count > 0 ? employeesList.Max(emp => emp.Id + 1) : 1;
    //employeesList.Add(newEmployee);

    //return Results.Created($"/employees/{newEmployee.Id}", newEmployee);
});

app.MapPut("/employees/{id}", (int id, Employee newEmployee, IEmployeeService employeeService) =>
{
    try
    {
        if (!ValidationHelper.TryValidate(newEmployee, out var validationResults))
        {
            // Returns 400 Bad Request if the validation fails
            return Results.BadRequest(validationResults);
        }

        Employee? employee = employeeService.UpdateEmployee(id, newEmployee);
        return employee is not null ? Results.Ok(employee) : Results.NotFound();
    }
    catch(Exception ex)
    {
        return Results.Problem($"{ex.Message}");
    }
    

    //// Find the employee using ID to be updated
    //Employee? employeeToBeUpdated = employeesList.FirstOrDefault(emp => emp.Id == id);

    //if (employeeToBeUpdated is null) return Results.NotFound();

    //employeeToBeUpdated.Name = newEmployee.Name;
    //employeeToBeUpdated.Position = newEmployee.Position;
    //employeeToBeUpdated.Salary = newEmployee.Salary;

    //return Results.Ok(employeeToBeUpdated);

});

// Endpoint to delete the employe using ID

app.MapDelete("/employeed/{id}", (int id, IEmployeeService employeeService) =>
{

    try
    {
        var result = employeeService.DeleteEmployee(id);
        return result ? Results.NoContent() : Results.NotFound();
    }
    catch (Exception ex) {
        return Results.Problem($"{ex.Message}");
    }

    
    //// retrieve the employee to be deleted using ID
    //var employeeToBeDeleted = employeesList.FirstOrDefault(emp => emp.Id == id);
    //if (employeeToBeDeleted is null) return Results.NotFound(); // If not found then return 404

    //// Remove the employee from the list
    //employeesList.Remove(employeeToBeDeleted);

    //// Return a 204 No Content Response
    //return Results.NoContent();
});

app.Run();
