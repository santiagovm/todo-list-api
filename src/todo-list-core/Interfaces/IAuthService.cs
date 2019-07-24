using System.Security.Claims;
using System.Threading.Tasks;

namespace TodoListAPI.Interfaces
{
    public interface IAuthService
    {
        Task<string> GenerateJwtAsync(string username, string password);

        Task<ClaimsPrincipal> ValidateTokenAsync(string jwtEncodedString);
    }
}
