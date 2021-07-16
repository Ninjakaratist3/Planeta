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
            var userForm = new Models.User()
            {
                Id = model.Id,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                Surname = model.Surname,
                Age = model.Age,
                Gender = model.Gender
            };

            return userForm;
        }

        public UserViewModel ConvertUserToUserViewModel(Models.User model)
        {
            var user = new UserViewModel()
            {
                Id = model.Id,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                Surname = model.Surname,
                Age = model.Age,
                Gender = model.Gender,
                Subnet = _subnetService.Get(model.Id)
            };

            return user;
        }

        private bool UserFormIsValid(UserForm model)
        {
            // TODO: UserForm validation

            return true;
        }
    }
}
