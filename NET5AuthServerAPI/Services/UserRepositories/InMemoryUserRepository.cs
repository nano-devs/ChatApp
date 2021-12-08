using NET5AuthServerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NET5AuthServerAPI.Services.UserRepositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> users = new List<User>();
        private int counter = 0;

        public Task<User> Create(User user)
        {
            user.Id = counter + 1;
            counter += 1;

            users.Add(user);

            return Task.FromResult(user);
        }

        public Task<User> GetByEmail(string email)
        {
            return Task.FromResult(users.Find(user => user.Email == email));
        }

        public Task<User> GetById(int id)
        {
            return Task.FromResult(users.Find(user => user.Id == id));
        }

        public Task<User> GetByUserName(string username)
        {
            return Task.FromResult(users.Find(user => user.Username == username));
        }
    }
}
