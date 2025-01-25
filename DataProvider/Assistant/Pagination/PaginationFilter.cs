namespace DataProvider.Assistant.Pagination
{
    public class PaginationFilter
    {
        private const int MinPageNumber = 1;
        private const int MaxPageSize = 200;

        protected PaginationFilter(int pageNumber, int pageSize)
        {
            Page = pageNumber > 0 ? pageNumber : MinPageNumber;
            PageSize = pageSize > 0 && pageSize <= MaxPageSize ? pageSize : MaxPageSize;
        }

        protected PaginationFilter()
        {
        }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPageCount { get; set; }
        public SortByEnum SortByEnum { get; set; }
    }
}
