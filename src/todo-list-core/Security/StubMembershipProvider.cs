using System;
using System.Collections.Generic;
using System.Security.Claims;
using TodoListAPI.Interfaces;

namespace TodoListAPI.Security
{
    [Obsolete("this is not production grade, stub for testing")]
    public class StubMembershipProvider : IMembershipProvider
    {
        #region Implementation of IMembershipProvider

        public IEnumerable<Claim> GetUserClaims(string username)
        {
            return new[]
                   {
                       new Claim(ClaimTypes.NameIdentifier, "333"), 
                       new Claim(ClaimTypes.Email, "foo@bar.com"),
                       new Claim(TodoApiClaimTypes.AllowedActions, "get,post,put"), // for example, not allowed to delete
                   };
        }
        
        public bool VerifyUserPassword(string username, string password)
        {
            return username == "foo" && password == "bar";
        }

        #endregion
    }
}
