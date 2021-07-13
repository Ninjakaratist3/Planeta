using Core.Models;
using System.Collections.Generic;

namespace Core.Repositories
{
    public interface IUserRepository
    {
        User Get(int id);
        List<User> GetUsers();
        void Add(User user);
        void Update(User user);
        void Delete(int id);
    }
}
