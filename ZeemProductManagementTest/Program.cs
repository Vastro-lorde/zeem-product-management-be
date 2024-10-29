using Microsoft.EntityFrameworkCore;
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


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
