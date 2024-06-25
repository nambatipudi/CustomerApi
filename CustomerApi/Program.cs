using CustomerApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using CustomerApi.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure Entity Framework to use SQLite with the connection string from appsettings.json
builder.Services.AddDbContext<CustomerContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add controllers with a custom validation filter
builder.Services.AddControllers(options =>
{
    // Add the ValidateModelAttribute to handle model validation globally
    options.Filters.Add<ValidateModelAttribute>();
});

// Configure Swagger for API documentation
builder.Services.AddSwaggerGen(c =>
{
    // Define a Swagger document with version information
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CustomerService", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Use the developer exception page in development environment
    app.UseDeveloperExceptionPage();
}

// Enable middleware to serve generated Swagger as a JSON endpoint
app.UseSwagger();

// Enable middleware to serve Swagger UI
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CustomerService v1");
});

// Configure routing
app.UseRouting();

// Configure authorization
app.UseAuthorization();

// Map controller endpoints
app.MapControllers();

// Run the application
await app.RunAsync();