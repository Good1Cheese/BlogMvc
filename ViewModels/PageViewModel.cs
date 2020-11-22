using System;

namespace BlogMvc.Models
{
    public class PageViewModel
    {
        public int CategoryId { get; private set; }
        public int PageNumber { get; private set; }
        public double TotalPages { get; set; }

        public PageViewModel(int _CategoryId, int count, int pageNumber, int pageSize)
        {
            CategoryId = _CategoryId;
            PageNumber = pageNumber;
            TotalPages = Math.Ceiling(Convert.ToSingle(count) / Convert.ToSingle(pageSize));
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageNumber > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageNumber < TotalPages);
            }
        }

    }
}   