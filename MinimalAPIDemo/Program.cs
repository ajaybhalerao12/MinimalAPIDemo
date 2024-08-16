using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MinimalAPIDemo.Models;

var builder = WebApplication.CreateBuilder(args);

// Configure Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add services to the container.
// Add API explorer for endpoint documentation
builder.Services.AddEndpointsApiExplorer();
// Add Swagger for API documentation
builder.Services.AddSwaggerGen();

// Register EmployeeService in DI container
builder.Services.AddSingleton<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IEmployeeServiceAsync, EmployeeServiceAsync>();

// Register the Products service in DI container
builder.Services.AddScoped<IProductsService, ProductService>();

builder.Services.AddDbContextPool<ApplicationDBContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DBConn"),
        new MySqlServerVersion(new Version(8, 4, 0))));

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
app.MapGet("/employees", (IEmployeeService employeeService, ILogger<Program> logger) =>
{
    try
    {
        logger.LogInformation("Retrieve all employees");
        return Results.Ok(employeeService.GetAllEmployees());
    }
    catch(Exception ex)
    {
        logger.LogError(ex, "An error occured while retrieving the employees");
        return Results.Problem(ex.Message);
    }
    
});


// Endpoint to retrieve single employee using the employee ID
app.MapGet("/employees/{id}", (int id, IEmployeeService employeeService, ILogger<Program> logger) =>
{

    try
    {
        logger.LogInformation($"Retrieve the employe by employee id:{id}");
        // create a scenario to throw the exception
        //int x = 10, y = 0;
        //int result = x / y;

        var employee = employeeService.GetEmployeeById(id);
        if(employee == null)
        {
            logger.LogWarning("Employee with ID {id} not found",id);
          return  Results.NotFound(new { Message = $"Employee with ID  {id} not found" });
        }
        return Results.Ok(employee);
        //return employee is not null ? Results.Ok(employee) : Results.NotFound();
    }
    catch (Exception ex) {
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
    
});

// Endpoint to create a new employee
app.MapPost("/employees", (Employee newEmployee, IEmployeeService employeeService,ILogger<Program> logger) =>
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
    catch(Exception ex)
    {
        logger.LogError("An error occured while creating employee");
        return Results.Problem(ex.Message);
    }

  
    // retrieve the employee ID
    //newEmployee.Id = employeesList.Count > 0 ? employeesList.Max(emp => emp.Id + 1) : 1;
    //employeesList.Add(newEmployee);

    //return Results.Created($"/employees/{newEmployee.Id}", newEmployee);
});

app.MapPut("/employees/{id}", (int id, Employee newEmployee, IEmployeeService employeeService,ILogger<Program> logger) =>
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
    catch(Exception ex)
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

});

// Endpoint to delete the employe using ID

app.MapDelete("/employeed/{id}", (int id, IEmployeeService employeeService,ILogger<Program> logger) =>
{

    try
    {
        logger.LogInformation($"Delete the employe with ID {id}");
        var result = employeeService.DeleteEmployee(id);
        if(!result)
        {
            logger.LogWarning($"Delete the employee with ID {id} not found");
        }
        return result ? Results.NoContent() : Results.NotFound();
    }
    catch (Exception ex) {
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
});

// Map a GET endpoint to retrieve all endpoints asynchronously
app.MapGet("v2/employees", async (IEmployeeServiceAsync employeeServiceAsync, ILogger<Program> logger) =>
{
    try
    {
        logger.LogInformation("Retrieve all employee asynchronously");
        IEnumerable<Employee> employees = await employeeServiceAsync.GetAllEmployeesAsync(); 
        return Results.Ok(employees);
    }
    catch(Exception ex)
    {
        logger.LogError("An error occured while retrieving all employees");
        return Results.Problem(ex.Message);
    }    
});

// Map a Get endpoint to retrive employee with specific ID asynchornously
app.MapGet("v2/employees/{id}", async (int id, IEmployeeServiceAsync employeeServiceAsync, ILogger<Program> logger) =>
{
    try
    {
        logger.LogInformation($"Retrieve an employee with ID {id}");
        Employee employee = await employeeServiceAsync.GetEmployeeByIdAsync(id);
        if(employee == null)
        {
            logger.LogWarning($"Employee with ID {id} not found");
            return Results.NotFound(new { Message = $"Employee with ID {id} not found" });
        }
        return Results.Ok(employee);
    }
    catch (Exception ex) {
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
});

// Create a POST request to create an employee asynchronously
app.MapPost("v2/employees", async (Employee employee, IEmployeeServiceAsync employeeServiceAsync, ILogger<Program> logger) =>
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
});


// Create a PUT request to update the employee asynchronously
app.MapPut("/v2/employees/{id}", async (int id,Employee updatedEmployee, IEmployeeServiceAsync employeeServiceAsync, ILogger<Program> logger) =>
{
    try
    {
        logger.LogInformation($"Update employee with ID {id}");
        var employee = await employeeServiceAsync.UpdateEmployee(id, updatedEmployee);
        if(employee == null)
        {
            logger.LogWarning($"Unable to update the employee with ID {id}");
            return Results.NotFound(new { Message = $"Unable to update the employee with ID {id}" });
        }
        return Results.Ok(employee);
    }catch(Exception ex)
    {
        logger.LogError($"An error occured while updateing employee with ID {id}");
        return Results.Problem(ex.Message);
    }
});

// Create a DELETE request to delete employee asynchronously
app.MapDelete("/v2/employees/{id}", async (int id, IEmployeeServiceAsync employeeServiceAsync, ILogger<Program> logger) =>
{
    try
    {
        logger.LogInformation($"Delete operation with ID {id} is initiated");
        bool result = await employeeServiceAsync.DeleteEmployeeAsync(id);
        if (!result) {
            logger.LogWarning($"Employee with ID {id} not found");
            return Results.NotFound(new { Message = $"Employee with ID {id} not found" });
        }
        return Results.Ok(result);
    }
    catch (Exception ex) {
        logger.LogError($"An unexpected error occured during delete of employee with ID {id}");
        return Results.Problem(ex.Message);
    }
});

//CRUD operation for PRODUCTS service

// Create a MAP Get to retrieve all products
app.MapGet("/products", async (IProductsService productsService, ILogger<Program> logger) =>
{
    try
    {
        logger.LogInformation("Retreived all the products");
        var products = await productsService.GetProductsAsync();
        return Results.Ok(products);
    }
    catch (Exception ex)
    {
        logger.LogError(ex.Message, "An error occured while retrieving the products");
        return Results.Problem(ex.Message);
    }
});

// Create a GET request to retrieve a product using ID
app.MapGet("/products/{id}", async (int id, IProductsService productService, ILogger<Program> logger) =>
{
    try
    {
        var product = await productService.GetProductByIdAsync(id);
        if (product == null)
        {
            logger.LogWarning($"A product with ID {id} is not present");
            return Results.NotFound(new { Message = $"A product with ID {id} does not exist" });
        }

        return Results.Ok(product);
    }
    catch (Exception ex)
    {        
        logger.LogError(ex, "An Error occured whiel retrieving the products with ID");
        var probelemDetails = new ProblemDetails
        {
            Status = 500,
            Title = "An unexpected error occured",
            Detail = ex.Message,
            Instance = "/products/{id"
        };
        return Results.Problem(ex.Message);
    }
});

// Create a POST requet to create a new PRODUCT
app.MapPost("/products", async (Product product, IProductsService productService, ILogger<Program> logger) =>
{
    try
    {
        if(!ValidationHelper.TryValidate(product, out var validationResults))
        {
            // Return 400 Bad Request
            return Results.BadRequest(validationResults);
        }
        var createdProduct = await productService.AddProductAsync(product);
        return Results.Created($"/products/{createdProduct.Id}", createdProduct);
    }
    catch(Exception ex)
    {
        logger.LogError(ex, "An error occured while creating product");
        return Results.Problem(ex.Message);
    }
    
});

// Crete a PUT request to update the product
app.MapPut("/products/{id}", async (int id, Product product, IProductsService productService, ILogger<Program> logger) =>
{
    try
    {
        if(!ValidationHelper.TryValidate(product,out var validationResults))
        {
            return Results.BadRequest(validationResults);
        }
        logger.LogInformation($"Update product with ID {id}");
        var updatedProduct = await productService.UpdateProductAsync(id, product);
        if(updatedProduct == null)
            return Results.NotFound($"Product with ID {id} not found");

        return Results.Ok(updatedProduct);
    }
    catch (Exception ex) {
        logger.LogError(ex, "An error occure while updating the product");
        return Results.Problem(ex.Message);
    }
});


// Create a Delete Request to remove the prouduct
app.MapDelete("/products/{id}", async (int id, IProductsService productService, ILogger<Program> logger) =>
{
    try
    {
        logger.LogInformation($"Delete product with ID {id}");
        var result =await productService.DeleteProductAsync(id);
        if (!result)
        {
            logger.LogWarning($"Product with ID {id} not found");
            return Results.NotFound($"Product with ID {id} not found");
        }
        return Results.NoContent();
    }
    catch(Exception ex)
    {
        logger.LogError(ex,"An unexpected error occured");
        return Results.Problem(ex.Message);
    }
});
app.Run();
