using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using TodoListAPI.Security;

namespace TodoListAPI.Results
{
    public class AddBearerChallengeOnUnauthorizedResult : IHttpActionResult
    {
        public AddBearerChallengeOnUnauthorizedResult(IHttpActionResult innerResult)
        {
            _innerResult = innerResult ?? throw new ArgumentNullException(nameof(innerResult));
        }

        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await _innerResult.ExecuteAsync(cancellationToken);

            if (response.StatusCode != HttpStatusCode.Unauthorized)
            {
                return response;
            }

            var bearerChallenge = new AuthenticationHeaderValue(Constants.AUTHORIZATION_SCHEME_BEARER);

            // only one challenge per authentication scheme
            if (response.Headers.WwwAuthenticate.All(authHeader => authHeader.Scheme != bearerChallenge.Scheme))
            {
                response.Headers.WwwAuthenticate.Add(bearerChallenge);
            }

            return response;
        }

        private readonly IHttpActionResult _innerResult;
    }
}
