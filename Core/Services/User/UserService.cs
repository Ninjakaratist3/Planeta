using Core.Repositories.User;
using Core.Services.Subnet;
using Core.ViewModels.User;
using System;

namespace Core.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISubnetService _subnetService;

        public UserService(IUserRepository userRepository, ISubnetService subnetService)
        {
            _userRepository = userRepository;
            _subnetService = subnetService;
        }

        public UserViewModel Get(int id)
        {
            var user = _userRepository.Get(id);

            if (user == null)
            {
                throw new NullReferenceException();
            }

            return ConvertUserToUserViewModel(user);
        }

        public void Create(UserForm model)
        {
            if (!UserFormIsValid(model))
            {
                throw new ArgumentException();
            }

            var user = ConvertUserFormToUser(model);

            _userRepository.Add(user);
        }

        public void Update(UserForm model)
        {
            if (!UserFormIsValid(model))
            {
                throw new ArgumentException();
            }

            var user = ConvertUserFormToUser(model);

            _userRepository.Update(user);
        }

        public void Delete(int id)
        {
            if (id < 1)
            {
                throw new ArgumentException();
            }

            _userRepository.Delete(id);
        }

        public Models.User ConvertUserFormToUser(UserForm model)
        {
            var user = new Models.User();
            user.Id = model.Id;
            user.FirstName = model.FirstName;
            user.MiddleName = model.MiddleName;
            user.Surname = model.Surname;
            user.Age = model.Age;
            user.Gender = model.Gender;

            return user;
        }

        public UserViewModel ConvertUserToUserViewModel(Models.User model)
        {
            var userViewModel = new UserViewModel();
            userViewModel.Id = model.Id;
            userViewModel.FirstName = model.FirstName;
            userViewModel.MiddleName = model.MiddleName;
            userViewModel.Surname = model.Surname;
            userViewModel.Age = model.Age;
            userViewModel.Gender = model.Gender;
            try
            {
                userViewModel.Subnet = _subnetService.Get(model.Id);
            }
            catch
            {
                userViewModel.Subnet = null;
            }
            

            return userViewModel;
        }

        private bool UserFormIsValid(UserForm model)
        {
            if (string.IsNullOrEmpty(model.FirstName) || string.IsNullOrEmpty(model.Surname))
            {
                return false;
            }

            if (model.Age < 0)
            {
                return false;
            }

            return true;
        }
    }
}
