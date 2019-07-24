using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using TodoListAPI.Dtos;
using TodoListAPI.Extensions;

namespace TodoListAPI.Results
{
    internal class PaginatedResult : IHttpActionResult
    {
        public PaginatedResult(IHttpActionResult httpActionResult, IEnumerable<PaginationLinkInfo> links)
        {
            _httpActionResult = httpActionResult ?? throw new ArgumentNullException(nameof(httpActionResult));
            _links = links ?? throw new ArgumentNullException(nameof(links));
        }
        
        #region Implementation of IHttpActionResult

        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await _httpActionResult.ExecuteAsync(cancellationToken);
            
            response.AddLinks(_links.ToArray());
            
            return response;
        }

        #endregion

        private readonly IHttpActionResult _httpActionResult;
        private readonly IEnumerable<PaginationLinkInfo> _links;
    }
}
