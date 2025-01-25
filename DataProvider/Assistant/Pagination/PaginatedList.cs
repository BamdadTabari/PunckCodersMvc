namespace DataProvider.Assistant.Pagination
{
    public class PaginatedList<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public List<T> Data { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public PaginatedList(IEnumerable<T> items, int count, int page, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = page;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Data = items.ToList();
        }
    }

}
