using Microsoft.EntityFrameworkCore;
using CarePulseERP.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "CarePulse MediCore ERP API",
        Version = "v1",
        Description = "Enterprise Hospital Management API architected by Khadija Yaseen."
    });
});

// Configure SQL Server Database Connection via EF Core
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Server=(localdb)\\MSSQLLocalDB;Database=CarePulseHospitalDB;Trusted_Connection=True;TrustServerCertificate=True;";

builder.Services.AddDbContext<HospitalDbContext>(options =>
    options.UseSqlServer(connectionString));

// Configure CORS for Frontend Integration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// 1. Enable Static Files & Default Page (Serves the FULL WEBSITE UI on root http://localhost:port/)
app.UseDefaultFiles();
app.UseStaticFiles();

// 2. Enable Swagger at /swagger for API testing
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CarePulse MediCore ERP API v1");
    c.RoutePrefix = "swagger"; // Available at /swagger
});

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();
