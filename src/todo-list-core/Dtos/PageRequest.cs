using System;

namespace TodoListAPI.Dtos
{
    public class PageRequest
    {
        public int page_number { get; set; }
        public int page_size { get; set; }

        public PageRequest(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageNumber), pageNumber, "must be 1 or greater");
            }

            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, "must be 1 or greater");
            }
            
            page_number = pageNumber;
            page_size = pageSize;
        }

        public PageRequest()
        {
            // empty
            // for webapi infrastructure
        }
    }
}
