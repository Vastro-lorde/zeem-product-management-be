using Microsoft.EntityFrameworkCore;
using ZeemProductManagementTest.Data;
using ZeemProductManagementTest.DTOs;
using ZeemProductManagementTest.Models;
using ZeemProductManagementTest.Services.Pagination;

namespace ZeemProductManagementTest.Repository
{
    public class ProductRepo : IProductRepo
    {
        private readonly AppDbContext _context;
        public ProductRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PaginationModel<Product>> GetAllAsync(int pageSize, int pageNumber)
        {
            try
            {
                return await PaginationClass.PaginateAsync(_context.Products.AsQueryable(), pageSize, pageNumber);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<Product?> GetByIdAsync(Guid id)
        {
            try
            {
                return await _context.Products.FindAsync(id);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<Product> AddAsync(Product product)
        {
            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return product;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<Product?> UpdateAsync(Guid id, UpdateProductDTO product)
        {
            try
            {
                var existingProduct = await _context.Products.FindAsync(id);
                if (existingProduct == null) return null;

                existingProduct.Name = product.Name ?? existingProduct.Name;
                existingProduct.Description = product.Description ?? existingProduct.Description;
                existingProduct.Price = product.Price ?? existingProduct.Price;
                existingProduct.Stock = product.Stock ?? existingProduct.Stock;

                _context.Products.Update(existingProduct);
                await _context.SaveChangesAsync();
                return existingProduct;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null) return false;

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<PaginationModel<Product>> SearchAsync(string searchQuery, int pageSize, int pageNumber)
        {
            try
            {
                string query = searchQuery.ToLower();
                // Filter products that contain the search query in the Name or Description fields return IQueryable
                var filteredProducts = _context.Products
                    .Where(p => p.Name.ToLower().Contains(query) || p.Description.ToLower().Contains(query));

                // Paginate the filtered results
                return await PaginationClass.PaginateAsync(filteredProducts, pageSize, pageNumber);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
