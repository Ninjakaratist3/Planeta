using System.Collections.Generic;

namespace Core.Repositories.User
{
    public interface IUserRepository
    {
        Models.User Get(int id);
        List<Models.User> GetUsers();
        void Add(Models.User model);
        void Update(Models.User model);
        void Delete(int id);
    }
}
