using ZeemProductManagementTest.DTOs;
using ZeemProductManagementTest.Models;
using ZeemProductManagementTest.Services.Pagination;

namespace ZeemProductManagementTest.Repository
{
    public interface IProductRepo
    {
        Task<Product> AddAsync(Product product);
        Task<bool> DeleteAsync(Guid id);
        Task<PaginationModel<Product>> GetAllAsync(int pageSize, int pageNumber);
        Task<Product?> GetByIdAsync(Guid id);
        Task<Product?> UpdateAsync(Guid id, UpdateProductDTO product);
    }
}