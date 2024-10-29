using Microsoft.EntityFrameworkCore;
using ZeemProductManagementTest.Data;
using ZeemProductManagementTest.Repository;

var builder = WebApplication.CreateBuilder(args);


builder.Configuration.AddEnvironmentVariables();


// Add services to the container.
// Adding DbContext with InMemoryDatabase
// Addong DbContext with optional PostgreSQL
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
Console.WriteLine($"Connection String: {Environment.GetEnvironmentVariable("POSTGRESQL_CONNECT")}");

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
app.UseSwagger();
    app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
