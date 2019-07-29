using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace TodoListAPI.Results
{
    public class AuthenticationFailureResult : IHttpActionResult
    {
        public AuthenticationFailureResult(string reasonPhrase, HttpRequestMessage request)
        {
            _reasonPhrase = reasonPhrase ?? throw new ArgumentNullException(nameof(reasonPhrase));
            _request = request ?? throw new ArgumentNullException(nameof(request));
        }

        #region Implementation of IHttpActionResult

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                           {
                               RequestMessage = _request,
                               ReasonPhrase = _reasonPhrase,
                           };

            return Task.FromResult(response);
        }

        #endregion

        private readonly string _reasonPhrase;
        private readonly HttpRequestMessage _request;
    }
}
