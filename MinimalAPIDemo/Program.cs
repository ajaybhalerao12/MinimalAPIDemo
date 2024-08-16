using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MinimalAPIDemo;
using MinimalAPIDemo.Models;

var builder = WebApplication.CreateBuilder(args);

// Configure Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.RegisterApplicationServices(builder.Configuration);

var app = builder.Build();

app.ConfigureMiddleware();

app.RegisterEndpoints();


app.Run();
