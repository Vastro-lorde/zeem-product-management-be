using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ZeemProductManagementTest.Models;

namespace ZeemProductManagementTest.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        

    }
}
