# Minimal API in .NET Core Web API

This project demonstrates a Minimal API implementation using .NET Core. Minimal APIs are designed to create HTTP APIs with minimal dependencies, making them ideal for microservices and lightweight applications.

## Features

- Create, Read, Update, and Delete (CRUD) operations for products and employees
- JWT authentication for secure access
- Dependency Injection for service management
- Logging and Exception handling filters

## Prerequisites

- .NET 8 SDK
- Visual Studio 2022 or Visual Studio Code

## Getting Started

### Clone the Repository

```bash
git clone https://github.com/ajaybhalerao12/MinimalAPIDemo.git
cd MinimalAPIDemo
```

### Build and Run

1. Open the project in Visual Studio or Visual Studio Code.
2. Restore the dependencies and build the project:

    ```bash
    dotnet restore
    dotnet build
    ```
3. Update the appsettings.json file with your JWT settings:
   {
    "JwtSettings": {
        "SecretKey": "your-secret-key",
        "Issuer": "your-issuer",
        "Audience": "your-audience"
    }
   }
4. Apply migrations to create the database:
   dotnet ef database update

5. Run the application:

    ```bash
    dotnet run
    ```

4. The API will be available at `https://localhost:7025`.

    ## Authentication
    To access the API endpoints, you need to log in using the default credentials:

    Username: user
    Password: pass@123
    You can obtain a JWT token by sending a POST request to the /authenticate endpoint with the default credentials.

## API Endpoints

### GET /products
Retrieve all products

### GET /products/{id}
Retrieve a product by ID

### POST /products
Create a new product
### PUT /products/{id}
Update an existing product

### DELETE /products/{id}
Delete a product

### GET /employees

Retrieves a list of employee items.

### GET /employees/{id}

Retrieves a specific employee item by ID.

### POST /employees

Creates a new employee item.

### PUT /employees/{id}

Updates an existing employee item by ID.

### DELETE /employees/{id}

Deletes a employee item by ID.

## Example Request
Get All Products
curl -X GET "https://localhost:5001/products" -H "Authorization: Bearer {your_jwt_token}"


### POST /employees

```json
{  
  "name": "Sample Employee Name",
  "position": "Software Engineer",
  "salary": 12000  
}
```

## Technologies Used

- .NET 8
- ASP.NET Core Minimal API
- Swagger for API documentation

## Contributing

Contributions are welcome! Please open an issue or submit a pull request.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
