using COMMSFinalProject.Models;

namespace COMMSFinalProject.Interfaces
{
    public interface IUser
    {
        public Task<User> Register(NewUser newUser);
        public Task<User> Login(User user);
        User Authenticate(string username, string password);
        User GetOwnData();
        string GenerateToken(User user);
    }
}
