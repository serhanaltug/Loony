namespace Loony.Web.Models
{
    public class ListViewModel<T> where T : class
    {
        public PaginatedList<T> Data { get; set; }
        public Page Page { get; set; }

        public ListViewModel(string filters, string search, string order, string sortdir)
        {
            Page = new Page()
            {
                Filter = filters,
                Search = search,
                Order = order,
                SortDirection = sortdir
            };
        }
    }
}
