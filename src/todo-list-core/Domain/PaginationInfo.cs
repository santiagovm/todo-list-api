using System;
using TodoListAPI.Dtos;

namespace TodoListAPI.Domain
{
    public class PaginationInfo
    {
        public int PageNumber { get; }
        public int PageSize { get; }
        public int TotalPages { get; }

        public PaginationInfo(PageRequest pageRequest, int itemsTotalCount)
        {
            PageNumber = pageRequest.page_number;
            PageSize = pageRequest.page_size;
            TotalPages = (int) Math.Ceiling(itemsTotalCount / (double) pageRequest.page_size);
        }
    }
}
