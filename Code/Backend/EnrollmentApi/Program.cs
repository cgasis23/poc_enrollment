using Microsoft.EntityFrameworkCore;
using EnrollmentApi.Data;
using EnrollmentApi.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Enrollment API",
        Version = "v1",
        Description = "A REST API for customer enrollment management with Multi-Factor Authentication (MFA) support. This API provides comprehensive customer enrollment functionality including CRUD operations, MFA setup and verification, document management, and advanced search capabilities.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Enrollment API Support",
            Email = "support@enrollmentapi.com"
        },
        License = new Microsoft.OpenApi.Models.OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });

    // Include XML comments if available
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }

    // Add security definitions for future authentication
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Add Entity Framework with In-Memory Database
// For production, you can switch to SQL Server by:
// 1. Adding connection string to appsettings.json
// 2. Changing this to: options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
builder.Services.AddDbContext<EnrollmentDbContext>(options =>
    options.UseInMemoryDatabase("EnrollmentDb"));

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Register services
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IMfaService, MfaService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
// Always enable Swagger for easier testing
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Enrollment API V1");
    c.RoutePrefix = "swagger"; // Serve Swagger UI at /swagger
});

// Only redirect to HTTPS if we're not in development or if HTTPS is available
if (!app.Environment.IsDevelopment() || app.Configuration["ASPNETCORE_URLS"]?.Contains("https") == true)
{
    app.UseHttpsRedirection();
}

// Use CORS
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<EnrollmentDbContext>();
    context.Database.EnsureCreated();
}

app.Run();
