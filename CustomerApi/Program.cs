using CustomerApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using System.ComponentModel.DataAnnotations;
using CustomerApi.Controllers;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<CustomerContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// ASP.NET Core automatically handles model validation errors by returning a 400 Bad Request 
// with ModelState as the response body.
// One way is that you could suppress this feature by:
builder.Services.AddControllers(options =>
    {
        options.Filters.Add<ValidateModelAttribute>();
        
    });
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CustomerService", Version = "v1" });
});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CustomerService v1");
});

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();