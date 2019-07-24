using System;
using System.Security.Claims;
using System.Threading;
using TodoListAPI.Interfaces;

namespace TodoListAPI.Security
{
    public class SessionInfo : ISessionInfo
    {
        #region Implementation of ISessionInfo

        public int UserId
        {
            get
            {
                string nameIdentifier = GetClaimValue(ClaimTypes.NameIdentifier);

                if (int.TryParse(nameIdentifier, out int userId))
                {
                    return userId;
                }

                throw new Exception($"current thread's principal does not contain a claim name identifier with a number, found: [{nameIdentifier}]");
            }
        }

        public string Username => GetClaimValue(ClaimTypes.Email);

        #endregion

        private static string GetClaimValue(string claimType)
        {
            if (!( Thread.CurrentPrincipal is ClaimsPrincipal claimsPrincipal ))
            {
                return null;
            }

            Claim claim = claimsPrincipal.FindFirst(claimType);

            return claim?.Value;
        }
    }
}
