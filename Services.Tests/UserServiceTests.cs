using Core.Models;
using Core.Repositories.Subnet;
using Core.Repositories.User;
using Core.Services.Subnet;
using Core.Services.User;
using Core.ViewModels.User;
using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Linq;
using Xunit;

namespace Services.Tests
{
    public class UserServiceTests
    {
        private string _connectionString = "Server=(localdb)\\mssqllocaldb;Database=Planeta;Trusted_Connection=True;Integrated Security=True";

        [Fact]
        public void CreateValidUser()
        {
            var userRepository = new UserRepository(_connectionString);
            var subnetRepository = new SubnetRepository(_connectionString);
            ISubnetService subnetService = new SubnetService(userRepository, subnetRepository);
            IUserService userService = new UserService(userRepository, subnetService);
            var user = new UserForm()
            {
                FirstName = "Василий",
                Surname = "Васильев",
                Age = 19,
                Gender = "Мужчина"
            };

            userService.Create(user);

            var userId = GetLastUserId();
            var model = userService.Get(userId);
            Assert.Equal(model.FirstName, user.FirstName);
            Assert.Equal(model.Surname, user.Surname);
            Assert.Equal(model.Age, user.Age);
            Assert.Equal(model.Gender, user.Gender);

            userRepository.Delete(userId);
        }

        [Fact]
        public void CreateInvalidUser()
        {
            var userRepository = new UserRepository(_connectionString);
            var subnetRepository = new SubnetRepository(_connectionString);
            ISubnetService subnetService = new SubnetService(userRepository, subnetRepository);
            IUserService userService = new UserService(userRepository, subnetService);
            var user = new UserForm()
            {
                FirstName = "",
                Surname = "Васильев",
                Age = -2,
                Gender = "Мужчина"
            };
            var userId = GetLastUserId();

            Action action = () => userService.Create(user);

            Assert.Throws<ArgumentException>(action);

            userRepository.Delete(userId);
        }

        [Fact]
        public void GetNotExistingUser()
        {
            var userRepository = new UserRepository(_connectionString);
            var subnetRepository = new SubnetRepository(_connectionString);
            ISubnetService subnetService = new SubnetService(userRepository, subnetRepository);
            IUserService userService = new UserService(userRepository, subnetService);

            Action action = () => userService.Get(-1);

            Assert.Throws<NullReferenceException>(action);
        }

        [Fact]
        public void UpdateUserWithValidModel()
        {
            var userRepository = new UserRepository(_connectionString);
            var subnetRepository = new SubnetRepository(_connectionString);
            ISubnetService subnetService = new SubnetService(userRepository, subnetRepository);
            IUserService userService = new UserService(userRepository, subnetService);
            var user = new UserForm()
            {
                FirstName = "Василий",
                Surname = "Васильев",
                Age = 19,
                Gender = "Мужчина"
            };
            userService.Create(user);
            var userId = GetLastUserId();
            user.Id = userId;
            user.FirstName = "Иван";
            user.Age = 22;

            userService.Update(user);

            var model = userService.Get(userId);
            Assert.Equal(model.FirstName, user.FirstName);
            Assert.Equal(model.Surname, user.Surname);
            Assert.Equal(model.Age, user.Age);
            Assert.Equal(model.Gender, user.Gender);

            userRepository.Delete(userId);
        }

        [Fact]
        public void UpdateUserWithInvalidModel()
        {
            var userRepository = new UserRepository(_connectionString);
            var subnetRepository = new SubnetRepository(_connectionString);
            ISubnetService subnetService = new SubnetService(userRepository, subnetRepository);
            IUserService userService = new UserService(userRepository, subnetService);
            var user = new UserForm()
            {
                FirstName = "Василий",
                Surname = "Васильев",
                Age = 19,
                Gender = "Мужчина"
            };
            userService.Create(user);
            var userId = GetLastUserId();
            user.FirstName = "";
            user.Age = -1;

            Action action = () => userService.Update(user);

            Assert.Throws<ArgumentException>(action);

            userRepository.Delete(userId);
        }

        [Fact]
        public void DeleteWithValidId()
        {
            var userRepository = new UserRepository(_connectionString);
            var subnetRepository = new SubnetRepository(_connectionString);
            ISubnetService subnetService = new SubnetService(userRepository, subnetRepository);
            IUserService userService = new UserService(userRepository, subnetService);
            var user = new UserForm()
            {
                FirstName = "Василий",
                Surname = "Васильев",
                Age = 19,
                Gender = "Мужчина"
            };
            userService.Create(user);
            var userId = GetLastUserId();

            subnetService.Delete(userId);

            Action action = () => subnetService.Get(userId);
            Assert.Throws<NullReferenceException>(action);

            userRepository.Delete(userId);
        }

        [Fact]
        public void DeleteWithIdLessThanOne()
        {
            var userRepository = new UserRepository(_connectionString);
            var subnetRepository = new SubnetRepository(_connectionString);
            ISubnetService subnetService = new SubnetService(userRepository, subnetRepository);
            IUserService userService = new UserService(userRepository, subnetService);

            Action action = () => userService.Delete(-1);

            Assert.Throws<ArgumentException>(action);
        }

        private int GetLastUserId()
        {
            string sqlGetIdCommand = "SELECT Id FROM Users";

            using (IDbConnection dataBaseConnection = new SqlConnection(_connectionString))
            {
                return dataBaseConnection.Query<int>(sqlGetIdCommand).LastOrDefault();
            }
        }
    }
}
