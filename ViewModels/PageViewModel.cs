using System;

namespace TestBlog2.Models
{
    public class PageViewModel
    {
        public int CategoryId { get; private set; }
        public int PageNumber { get; private set; }
        public int TotalPages { get; set; }

        public PageViewModel(int _CategoryId, int count, int pageNumber, int pageSize)
        {
            CategoryId = _CategoryId;
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
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
        public bool HasNextNextPage
        {
            get
            {
                return ((PageNumber + 1) < TotalPages);
            }
        }
    }
}