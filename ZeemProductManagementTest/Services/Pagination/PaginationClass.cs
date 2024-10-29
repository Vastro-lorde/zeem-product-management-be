using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ZeemProductManagementTest.Services.Pagination
{
    public class PaginationClass
    {
        public static async Task<PaginationModel<TSource>> PaginateAsync<TSource>(IQueryable<TSource> source, int pageSize, int pageNumber)
        {
            // Calculate the total number of items
            var count = await source.CountAsync();


            // Calculate total pages and set current page items
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            var currentPageItems = await source
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Build and return the pagination result
            return new PaginationModel<TSource>
            {
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalNumberOfPages = totalPages,
                PageItems = currentPageItems,
            };
        }
    }

}
