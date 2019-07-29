using System.Collections.Generic;
using System.Web.Http;
using TodoListAPI.Dtos;
using TodoListAPI.Results;

namespace TodoListAPI.Extensions
{
    internal static class HttpActionResultExtensions
    {
        public static IHttpActionResult WithLinks(this IHttpActionResult httpActionResult, IEnumerable<PaginationLinkInfo> links)
        {
            return new PaginatedResult(httpActionResult, links);
        }
    }
}
