using System.Collections.Generic;

namespace BackEndAPI.Models
{
    public abstract class PaginationResponse<TModel>
    {
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }

        public IEnumerable<TModel> Items { get; set; }
    }
}