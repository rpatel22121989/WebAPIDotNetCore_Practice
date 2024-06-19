using ExportApp.Models;

namespace ExportApp.Services.User
{
    public interface IUserService
    {
        int SaveDetails(UserDetails userDetails);
    }
}
