using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ZeemProductManagementTest.Data;
using ZeemProductManagementTest.Repository;
using ZeemProductManagementTest.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Configuration.AddEnvironmentVariables();


// Add services to the container.
// Adding DbContext with InMemoryDatabase
// Addong DbContext with optional PostgreSQL
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

connectionString  = string.IsNullOrEmpty(connectionString)? Environment.GetEnvironmentVariable("POSTGRESQL_CONNECT") ?? string.Empty : connectionString;


if (!string.IsNullOrEmpty(connectionString))
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(connectionString));
}
else
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseInMemoryDatabase("ProductDb"));
}
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IProductRepo, ProductRepo>();

builder.Services.AddCors(e => e.AddDefaultPolicy (builder => builder
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader()
               )
);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo 
    {
        Title = "ZeemProductManagementTest",
        Version = "v1",
        Description = "Created By Seun Daniel Omatsola for Zeem Assessment"

    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{

//}

// Register the global exception handling middleware
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
