using Microsoft.EntityFrameworkCore;

namespace Loony.Web.Models
{

    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.AddRange(items);
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }

    }

    public class Page
    {
        public int DataCount { get; set; }
        public int SelectedPage { get; set; }
        public string Order { get; set; }
        public string SortDirection { get; set; }
        public string Filter { get; set; }
        public string Search { get; set; }
        public int PageSize { get; set; } = 10;

        public bool HasPreviousPage { get { return (SelectedPage > 1); } }
        public bool HasNextPage { get { return (SelectedPage < PageCount()); } }


        public int PageCount()
        {
            return (int)Math.Ceiling(DataCount / (double)PageSize);
        }
    }
}
