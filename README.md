# Minimal API in .NET Core Web API

This project demonstrates a Minimal API implementation using .NET Core. Minimal APIs are designed to create HTTP APIs with minimal dependencies, making them ideal for microservices and lightweight applications.

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

3. Run the application:

    ```bash
    dotnet run
    ```

4. The API will be available at `https://localhost:7025`.

## API Endpoints

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
