using Core.ViewModels.User;

namespace Core.Services.User
{
    public interface IUserService
    {
        public UserViewModel Get(int id);
        public void Create(UserForm model);
        public void Update(UserForm model);
        public void Delete(int id);
        public Models.User ConvertUserFormToUser(UserForm model);
        public UserViewModel ConvertUserToUserViewModel(Models.User model);
    }
}
