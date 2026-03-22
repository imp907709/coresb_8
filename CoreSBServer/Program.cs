using CoreSBBL;
using CoreSBShared.Registrations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.FrameworkRegistrations();

builder.RegisterConnections();

builder.RegisterKeys();

builder.RegisterContexts();
builder.RegisterServices();

builder.RegisterContextsBL();
builder.RegisterServicesBL();

var app = builder.Build();

app.Registration();

// Access the logger from the application services
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Application started.");

app.Run();
