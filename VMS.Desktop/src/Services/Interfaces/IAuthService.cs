using System.Threading.Tasks;

namespace VMS.Services.Interfaces
{
    public interface IAuthService
    {
        string? CurrentUser { get; }
        Task<bool> LoginAsync(string username, string password);
        void Logout();
    }
}
