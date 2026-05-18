using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Models;



namespace UserService.Repository
{
    public interface IUserRepository
    {
        Task<User> Register(User user);

        Task<User?> GetUserByEmail(string email);

        Task<User?> GetUserById(int id);

        Task<IEnumerable<User>> GetAllUsers();
        Task<User?> UpdateUser(User user);
        Task<bool> DeleteUser(int id);

    }
}
