using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using TodoListAPI.Interfaces;
using TodoListAPI.Results;

namespace TodoListAPI.Security
{
    public class TokenAuthenticate : AuthorizationFilterAttribute, IAuthenticationFilter
    {
        #region Implementation of IAuthenticationFilter

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var authService = (IAuthService) context.ActionContext
                                                    .ControllerContext
                                                    .Configuration
                                                    .DependencyResolver
                                                    .GetService(typeof(IAuthService));

            HttpRequestMessage request = context.Request;

            AuthenticationHeaderValue authorization = request.Headers.Authorization;

            if (authorization == null)
            {
                return;
            }

            if (authorization.Scheme != Constants.AUTHORIZATION_SCHEME_BEARER)
            {
                return;
            }

            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationFailureResult("missing credentials", request);
                return;
            }

            ClaimsPrincipal claimsPrincipal = await authService.ValidateTokenAsync(authorization.Parameter);

            if (claimsPrincipal == null)
            {
                context.ErrorResult = new AuthenticationFailureResult("invalid credentials", request);
                return;
            }

            context.Principal = claimsPrincipal;
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            context.Result = new AddBearerChallengeOnUnauthorizedResult(context.Result);
            return Task.FromResult(0);
        }

        #endregion

        #region Overrides of AuthorizationFilterAttribute

        public override async Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            bool isAuthorized = CheckAuthorization(actionContext);

            if (!isAuthorized)
            {
                var failureResult = new AuthenticationFailureResult("not authorized", actionContext.Request);
                HttpResponseMessage response = await failureResult.ExecuteAsync(cancellationToken);
                actionContext.Response = response;
                return;
            }

            await base.OnAuthorizationAsync(actionContext, cancellationToken);
        }
        
        #endregion

        private static bool CheckAuthorization(HttpActionContext actionContext)
        {
            if (!( actionContext.RequestContext.Principal is ClaimsPrincipal claimsPrincipal ))
            {
                return false;
            }

            string actionName = actionContext.ActionDescriptor.ActionName;

            return CheckClaims(claimsPrincipal, actionName);
        }

        /// <summary>
        /// this authorization strategy is based on http verbs
        /// user will have a claim called actions with a comma-separated list of http verbs is allowed to use
        /// for example: get,post,put
        /// then the http verb of the action being authorized is checked against the list in the JWT claims
        /// maybe something more sophisticated would be needed, like role-based security
        /// </summary>
        /// <param name="claimsPrincipal"></param>
        /// <param name="actionNameToCheck"></param>
        /// <returns></returns>
        private static bool CheckClaims(ClaimsPrincipal claimsPrincipal, string actionNameToCheck)
        {
            bool isAuthenticated = claimsPrincipal.Identity.IsAuthenticated;

            if (!isAuthenticated)
            {
                return false;
            }

            return ( from actionsClaim in claimsPrincipal.FindAll(c => c.Type == TodoApiClaimTypes.AllowedActions)

                     select actionsClaim.Value.ToLower()
                     into actionsClaimValue

                     select actionsClaimValue.Split(',')
                     into authorizedActionsList

                     select authorizedActionsList.Any(authorizedAction => authorizedAction == actionNameToCheck.ToLower()) )
                .Any(authorizedAction => authorizedAction);
        }
    }
}
