using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using ZeemProductManagementTest.Data;
using ZeemProductManagementTest.DTOs;
using ZeemProductManagementTest.Models;
using ZeemProductManagementTest.Repository;
using ZeemProductManagementTest.Services.Pagination;
namespace Tests
{
    public class ProductRepoTests
    {
        private AppDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsPaginatedProducts()
        {
            // Arrange
            using var context = CreateDbContext();

            var products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Product1" },
                new Product { Id = Guid.NewGuid(), Name = "Product2" }
            };
            context.Products.AddRange(products);

            await context.SaveChangesAsync();

            var repo = new ProductRepo(context);

            // Act
            var result = await repo.GetAllAsync(2, 1);

            // Assert
            Assert.Equal(2, result.PageItems.Count);
            Assert.Contains(result.PageItems, p => p.Name == "Product1");
            Assert.Contains(result.PageItems, p => p.Name == "Product2");
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsProduct_WhenProductExists()
        {
            // Arrange
            using var context = CreateDbContext();
            var product = new Product { Id = Guid.NewGuid(), Name = "Product1" };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var repo = new ProductRepo(context);

            // Act
            var result = await repo.GetByIdAsync(product.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Product1", result.Name);
        }

        [Fact]
        public async Task AddAsync_AddsProductSuccessfully()
        {
            // Arrange
            using var context = CreateDbContext();
            var product = new Product { Id = Guid.NewGuid(), Name = "New Product" };
            var repo = new ProductRepo(context);

            // Act
            var result = await repo.AddAsync(product);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Product", result.Name);
            Assert.Single(context.Products);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesExistingProduct()
        {
            // Arrange
            using var context = CreateDbContext();
            var product = new Product { Id = Guid.NewGuid(), Name = "Old Name", Description = "Old Description", Price = 10, Stock = 5 };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var repo = new ProductRepo(context);
            var updateDto = new UpdateProductDTO { Name = "New Name", Price = 20 };

            // Act
            var result = await repo.UpdateAsync(product.Id, updateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Name", result.Name);
            Assert.Equal("Old Description", result.Description); // unchanged
            Assert.Equal(20, result.Price); // updated
            Assert.Equal(5, result.Stock);  // unchanged
        }

        [Fact]
        public async Task DeleteAsync_RemovesProduct()
        {
            // Arrange
            using var context = CreateDbContext();
            var product = new Product { Id = Guid.NewGuid(), Name = "Product to Delete" };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var repo = new ProductRepo(context);

            // Act
            var result = await repo.DeleteAsync(product.Id);

            // Assert
            Assert.True(result);
            Assert.Null(await context.Products.FindAsync(product.Id));
        }

        [Fact]
        public async Task SearchAsync_FiltersAndPaginatesProducts()
        {
            // Arrange
            using var context = CreateDbContext();
            var products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Apple", Description = "Fresh" },
                new Product { Id = Guid.NewGuid(), Name = "Banana", Description = "Yellow Fruit" },
                new Product { Id = Guid.NewGuid(), Name = "Grapes", Description = "Fresh and Sweet" }
            };
            context.Products.AddRange(products);
            await context.SaveChangesAsync();

            var repo = new ProductRepo(context);

            // Act
            var result = await repo.SearchAsync("Fresh", 2, 1);

            // Assert
            Assert.Equal(2, result.PageItems.Count);
            Assert.Contains(result.PageItems, p => p.Name == "Apple");
            Assert.Contains(result.PageItems, p => p.Name == "Grapes");
        }
    }
}
    
