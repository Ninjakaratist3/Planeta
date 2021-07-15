using Core.Repositories.Subnet;
using Core.Repositories.User;
using Core.ViewModels.User;
using System.Collections.Generic;
using System.Net;

namespace Core.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISubnetRepository _subnetRepository;

        public UserService(IUserRepository userRepository, ISubnetRepository subnetRepository)
        {
            _userRepository = userRepository;
            _subnetRepository = subnetRepository;
        }

        public UserViewModel Get(int id)
        {
            var user = _userRepository.Get(id);

            return ConvertUserToUserViewModel(user);
        }

        public void Create(UserForm model)
        {
            var user = ConvertUserFormToUser(model);

            _userRepository.Add(user);
        }

        public void Update(UserForm model)
        {
            var user = ConvertUserFormToUser(model);

            _userRepository.Update(user);
        }

        public void Delete(int id)
        {
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
                Gender = model.Gender
            };

            var subnet = _subnetRepository.Get(model.Id);

            if (subnet != null)
            {
                user.SubnetIP = IPAddress.Parse(_subnetRepository.Get(model.Id).IP.ToString());
            }

            return user;
        }
    }
}
