using CBT.API.Models;
using System.Threading.Tasks;

namespace CBT.API.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<bool> CreateUser(User user);
        Task<User> GetUserById(int id);
        Task<User> GetUserByUsername(string username);
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUser(int id);
    }
}



