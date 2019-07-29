using System.Collections.Generic;
using System.Security.Claims;

namespace TodoListAPI.Interfaces
{
    public interface IMembershipProvider
    {
        IEnumerable<Claim> GetUserClaims(string username);

        bool VerifyUserPassword(string username, string password);
    }
}
