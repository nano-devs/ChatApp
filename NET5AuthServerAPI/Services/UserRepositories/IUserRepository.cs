using NET5AuthServerAPI.Models;
using System;
using System.Threading.Tasks;

namespace NET5AuthServerAPI.Services.UserRepositories
{
    public interface IUserRepository
    {
        Task<User> GetById(Guid id);
        Task<User> GetByEmail(string email);
        Task<User> GetByUserName(string userName);
        Task<User> Create(User user);
    }
}
