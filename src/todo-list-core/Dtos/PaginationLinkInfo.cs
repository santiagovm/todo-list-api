using System;
using TodoListAPI.Domain;

namespace TodoListAPI.Dtos
{
    public class PaginationLinkInfo
    {
        public int PageNumber { get; }
        public int PageSize { get; }
        public LinkRelationType Relation { get; }
        public Uri Url { get; }

        public PaginationLinkInfo(int pageNumber, int pageSize, LinkRelationType relation, Uri url)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Relation = relation;
            Url = url;
        }
    }
}
