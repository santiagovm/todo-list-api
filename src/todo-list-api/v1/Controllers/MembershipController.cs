using System;
using System.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Serilog;
using TodoListAPI.Interfaces;
using TodoListAPI.Results;

namespace TodoListAPI.v1.Controllers
{
    public class MembershipController : ApiController
    {
        public MembershipController(IAuthService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        [HttpGet]
        public async Task<IHttpActionResult> Authenticate(string username, string password)
        {
            try
            {
                string token = await _authService.GenerateJwtAsync(username, password);
                return Ok(token);
            }
            catch (SecurityException e)
            {
                Log.Error(e, "could not generate security token");
                return new AddBearerChallengeOnUnauthorizedResult(Unauthorized());
            }
        }

        private readonly IAuthService _authService;
    }
}
