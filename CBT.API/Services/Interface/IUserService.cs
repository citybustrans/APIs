using CBT.API.Models;

namespace CBT.API.Services.Interface
{
    public interface IUserService
    {
        Task<bool> RegisterUser(User user);
        Task<string> LoginUser(string username, string password);
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUser(int id);
    }

}
