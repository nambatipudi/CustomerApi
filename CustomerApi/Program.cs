using CustomerApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using CustomerApi.Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CustomerContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register the ICustomerContext as CustomerContext
builder.Services.AddScoped<ICustomerContext, CustomerContext>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidateModelAttribute>();
    options.Filters.Add<GlobalExceptionFilter>();
});

builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CustomerService", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<CustomerContext>();

    if (!File.Exists(builder.Configuration.GetConnectionString("DefaultConnection")))
    {
        context.Database.EnsureCreated();
    }

    if (!context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
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