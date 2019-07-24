using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Internal;
using TodoListAPI.Domain;

namespace TodoListAPI.Dtos
{
    public class PaginatedResult<T>
    {
        public IEnumerable<T> Data { get; }

        public bool IsEmpty => !Data.Any();

        public PaginatedResult(IEnumerable<T> data, PaginationInfo paginationInfo)
        {
            Data = data;
            _paginationInfo = paginationInfo ?? throw new ArgumentNullException(nameof(paginationInfo));
        }

        public IEnumerable<PaginationLinkInfo> GetPaginationLinks(Uri requestUri)
        {
            if (_paginationInfo.TotalPages < 2)
            {
                return new PaginationLinkInfo[] { };
            }

            var links = new List<PaginationLinkInfo>(capacity: 4);

            if (_paginationInfo.PageNumber > 1)
            {
                var firstLink = new PaginationLinkInfo(pageNumber: 1,
                                                       _paginationInfo.PageSize,
                                                       LinkRelationType.First,
                                                       requestUri);

                var prevLink = new PaginationLinkInfo(pageNumber: _paginationInfo.PageNumber - 1,
                                                      _paginationInfo.PageSize,
                                                      LinkRelationType.Prev,
                                                      requestUri);

                links.Add(firstLink);
                links.Add(prevLink);
            }

            if (_paginationInfo.PageNumber >= _paginationInfo.TotalPages)
            {
                return links;
            }

            var nextLink = new PaginationLinkInfo(pageNumber: _paginationInfo.PageNumber + 1,
                                                  _paginationInfo.PageSize,
                                                  LinkRelationType.Next,
                                                  requestUri);

            var lastLink = new PaginationLinkInfo(pageNumber: _paginationInfo.TotalPages,
                                                  _paginationInfo.PageSize,
                                                  LinkRelationType.Last,
                                                  requestUri);

            links.Add(nextLink);
            links.Add(lastLink);

            return links;
        }

        private readonly PaginationInfo _paginationInfo;
    }
}
