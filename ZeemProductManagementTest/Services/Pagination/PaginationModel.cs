namespace ZeemProductManagementTest.Services.Pagination
{
    public class PaginationModel<T>
    {
        public List<T> PageItems { get; set; } = new List<T>();
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalNumberOfPages { get; set; }
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalNumberOfPages;
    }

}
