using ExportApp.Models;

namespace ExportApp.Services.User
{
    public class UserService : IUserService
    {
        public UserService() 
        {
            Console.WriteLine("UserService constructor called.");
        }

        public int SaveDetails (UserDetails userDetails)
        {
            return userDetails.Id;
        }
    }
}
