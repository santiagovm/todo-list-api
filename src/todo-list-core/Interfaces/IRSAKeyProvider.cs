using System.Threading.Tasks;

namespace TodoListAPI.Interfaces
{
    public interface IRSAKeyProvider
    {
        Task<string> GetKeysAsync();
    }
}
