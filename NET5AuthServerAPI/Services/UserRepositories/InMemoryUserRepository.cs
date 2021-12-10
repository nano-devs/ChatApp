using NET5AuthServerAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NET5AuthServerAPI.Services.UserRepositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> users = new List<User>();

        public Task<User> Create(User user)
        {
            user.Id = Guid.NewGuid();

            users.Add(user);

            return Task.FromResult(user);
        }

        public Task<User> GetByEmail(string email)
        {
            return Task.FromResult(users.Find(user => user.Email == email));
        }

        public Task<User> GetById(Guid id)
        {
            return Task.FromResult(users.Find(user => user.Id == id));
        }

        public Task<User> GetByUserName(string username)
        {
            return Task.FromResult(users.Find(user => user.Username == username));
        }
    }
}
