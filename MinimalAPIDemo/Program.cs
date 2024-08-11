using MinimalAPIDemo.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add API explorer for endpoint documentation
builder.Services.AddEndpointsApiExplorer();
// Add Swagger for API documentation
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline for the development pipeline.
if (app.Environment.IsDevelopment())
{
    // Use Swagger middleware for swagger documentation
    app.UseSwagger();
    // Use Swagger UI middleware to interact with Swagger documentation
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Create an in memory list for the employe list
var employeesList = new List<Employee>()
{
    new Employee(){ Id =  1 , Name = "Ajay", Position= "Software Engineer", Salary = 12000},
    new Employee{ Id = 2 , Name = "Prashant", Position = "Project Manager", Salary = 98000 }
};

// CRUD operation for the Employee model

// Endpoint to retrieve all the employees
// Map a GET request to /employees to return employees list
app.MapGet("/employees", () => employeesList);

// Endpoint to retrieve single employee using the employee ID
app.MapGet("/employees/{id}", (int id) =>
{
    Employee? employee = employeesList.FirstOrDefault(emp => emp.Id == id);
    return employee is not null? Results.Ok(employee): Results.NotFound();
});

// Endpoint to create a new employee
app.MapPost("/employees",(Employee newEmployee) =>{

    // retrieve the employee ID
    newEmployee.Id = employeesList.Count > 0 ? employeesList.Max(emp => emp.Id + 1) : 1;
    employeesList.Add(newEmployee);

    return Results.Created($"/employees/{newEmployee.Id}", newEmployee);

});

app.MapPut("/employees/{id}", (int id, Employee newEmployee) =>
{
    // Find the employee using ID to be updated
    Employee? employeeToBeUpdated = employeesList.FirstOrDefault(emp => emp.Id == id);

    if(employeeToBeUpdated is null) return Results.NotFound();

    employeeToBeUpdated.Name = newEmployee.Name;
    employeeToBeUpdated.Position = newEmployee.Position;
    employeeToBeUpdated.Salary = newEmployee.Salary;

    return Results.Ok(employeeToBeUpdated);

});

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
});

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
