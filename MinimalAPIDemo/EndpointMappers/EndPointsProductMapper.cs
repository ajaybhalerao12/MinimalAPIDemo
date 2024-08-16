using Microsoft.AspNetCore.Mvc;
using MinimalAPIDemo.Models;

namespace MinimalAPIDemo.EndpointMappers
{
    public static class EndPointsProductMapper
    {
        //CRUD operation for PRODUCTS service
        public static WebApplication MapProductEndpoints(this WebApplication app)
        {

            // Create a MAP Get to retrieve all products
            app.MapGet("/products", GetAllProducts());

            // Create a GET request to retrieve a product using ID
            app.MapGet("/products/{id}", GetProductById());

            // Create a POST requet to create a new PRODUCT
            app.MapPost("/products", CreateProduct());

            // Crete a PUT request to update the product
            app.MapPut("/products/{id}", UpdateProduct());

            // Create a Delete Request to remove the prouduct
            app.MapDelete("/products/{id}", DeleteProductById());

            return app;
        }

        private static Func<int, IProductsService, ILogger<Program>, Task<IResult>> DeleteProductById()
        {
            return async (int id, IProductsService productService, ILogger<Program> logger) =>
            {
                try
                {
                    logger.LogInformation($"Delete product with ID {id}");
                    var result = await productService.DeleteProductAsync(id);
                    if (!result)
                    {
                        logger.LogWarning($"Product with ID {id} not found");
                        return Results.NotFound($"Product with ID {id} not found");
                    }
                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An unexpected error occured");
                    return Results.Problem(ex.Message);
                }
            };
        }

        private static Func<int, Product, IProductsService, ILogger<Program>, Task<IResult>> UpdateProduct()
        {
            return async (int id, Product product, IProductsService productService, ILogger<Program> logger) =>
            {
                try
                {
                    if (!ValidationHelper.TryValidate(product, out var validationResults))
                    {
                        return Results.BadRequest(validationResults);
                    }
                    logger.LogInformation($"Update product with ID {id}");
                    var updatedProduct = await productService.UpdateProductAsync(id, product);
                    if (updatedProduct == null)
                        return Results.NotFound($"Product with ID {id} not found");

                    return Results.Ok(updatedProduct);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occure while updating the product");
                    return Results.Problem(ex.Message);
                }
            };
        }

        private static Func<Product, IProductsService, ILogger<Program>, Task<IResult>> CreateProduct()
        {
            return async (Product product, IProductsService productService, ILogger<Program> logger) =>
            {
                try
                {
                    if (!ValidationHelper.TryValidate(product, out var validationResults))
                    {
                        // Return 400 Bad Request
                        return Results.BadRequest(validationResults);
                    }
                    var createdProduct = await productService.AddProductAsync(product);
                    return Results.Created($"/products/{createdProduct.Id}", createdProduct);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occured while creating product");
                    return Results.Problem(ex.Message);
                }

            };
        }

        private static Func<int, IProductsService, ILogger<Program>, Task<IResult>> GetProductById()
        {
            return async (int id, IProductsService productService, ILogger<Program> logger) =>
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
            };
        }

        private static Func<IProductsService, ILogger<Program>, Task<IResult>> GetAllProducts()
        {
            return async (IProductsService productsService, ILogger<Program> logger) =>
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
            };
        }
    }
}