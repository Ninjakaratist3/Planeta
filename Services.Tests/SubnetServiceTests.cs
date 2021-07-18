using Core.Models;
using Core.Repositories.Subnet;
using Core.Repositories.User;
using Core.Services.Subnet;
using Core.ViewModels.Subnet;
using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Linq;
using Xunit;

namespace Services.Tests
{
    public class SubnetServiceTests
    {
        private string _connectionString = "Server=(localdb)\\mssqllocaldb;Database=Planeta;Trusted_Connection=True;Integrated Security=True";

        [Fact]
        public void CreateValidSubnet()
        {
            var userRepository = new UserRepository(_connectionString);
            var subnetRepository = new SubnetRepository(_connectionString);
            ISubnetService subnetService = new SubnetService(userRepository, subnetRepository);
            var userId = CreateUser();
            var subnet = new SubnetForm()
            {
                IP = "255.255.255.128/24",
                StartOfService = DateTime.Now,
                EndOfService = DateTime.MaxValue,
                UserId = userId
            };

            subnetService.Create(subnet);

            var subnetViewModel = subnetService.Get(userId);
            var model = subnetService.ConvertSubnetToSubnetViewModel(subnetService.ConvertSubnetFormToSubnet(subnet));
            Assert.Equal(model.IP, subnetViewModel.IP);
            Assert.Equal(model.Mask, subnetViewModel.Mask);
            Assert.Equal(model.StartOfService.ToShortDateString(), subnetViewModel.StartOfService.ToShortDateString());
            Assert.Equal(model.EndOfService.ToShortDateString(), subnetViewModel.EndOfService.ToShortDateString());
            Assert.Equal(model.UserId, subnetViewModel.UserId);

            userRepository.Delete(userId);
        }

        [Fact]
        public void CreateSubnetWithInvalidIP()
        {
            var userRepository = new UserRepository(_connectionString);
            var subnetRepository = new SubnetRepository(_connectionString);
            ISubnetService subnetService = new SubnetService(userRepository, subnetRepository);
            var subnet = new SubnetForm()
            {
                IP = "255.255.255/24",
                StartOfService = DateTime.Now,
                EndOfService = DateTime.MaxValue,
                UserId = 1
            };

            Action action = () => subnetService.Create(subnet);

            Assert.Throws<ArgumentException>(action);
        }


        [Fact]
        public void CreateSubnetWithInvalidTime()
        {
            var userRepository = new UserRepository(_connectionString);
            var subnetRepository = new SubnetRepository(_connectionString);
            ISubnetService subnetService = new SubnetService(userRepository, subnetRepository);
            var subnet = new SubnetForm()
            {
                IP = "255.255.255/24",
                StartOfService = DateTime.MaxValue,
                EndOfService = DateTime.Now,
                UserId = 1
            };

            Action action = () => subnetService.Create(subnet);

            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void GetNotExistingSubnet()
        {
            var userRepository = new UserRepository(_connectionString);
            var subnetRepository = new SubnetRepository(_connectionString);
            ISubnetService subnetService = new SubnetService(userRepository, subnetRepository);

            Action action = () => subnetService.Get(-1);

            Assert.Throws<NullReferenceException>(action);
        }

        [Fact]
        public void UpdateValidSubnet()
        {
            var userRepository = new UserRepository(_connectionString);
            var subnetRepository = new SubnetRepository(_connectionString);
            ISubnetService subnetService = new SubnetService(userRepository, subnetRepository);
            var userId = CreateUser();
            var subnet = new SubnetForm()
            {
                IP = "255.255.255.128/24",
                StartOfService = DateTime.Now,
                EndOfService = DateTime.MaxValue,
                UserId = userId
            };
            subnetService.Create(subnet);
            subnet.IP = "255.255.128.0/17";
            subnet.EndOfService = DateTime.Now.AddDays(1);

            subnetService.Update(subnet);

            var subnetViewModel = subnetService.Get(userId);
            var model = subnetService.ConvertSubnetToSubnetViewModel(subnetService.ConvertSubnetFormToSubnet(subnet));
            Assert.Equal(model.IP, subnetViewModel.IP);
            Assert.Equal(model.Mask, subnetViewModel.Mask);
            Assert.Equal(model.StartOfService.ToShortDateString(), subnetViewModel.StartOfService.ToShortDateString());
            Assert.Equal(model.EndOfService.ToShortDateString(), subnetViewModel.EndOfService.ToShortDateString());
            Assert.Equal(model.UserId, subnetViewModel.UserId);

            userRepository.Delete(userId);
        }

        [Fact]
        public void UpdateSubnetWithInvalidIP()
        {
            var userRepository = new UserRepository(_connectionString);
            var subnetRepository = new SubnetRepository(_connectionString);
            ISubnetService subnetService = new SubnetService(userRepository, subnetRepository);
            var userId = CreateUser();
            var subnet = new SubnetForm()
            {
                IP = "255.255.255.128/24",
                StartOfService = DateTime.Now,
                EndOfService = DateTime.MaxValue,
                UserId = userId
            };
            subnetService.Create(subnet);
            subnet.IP = "255.255.255/24";


            Action action = () => subnetService.Update(subnet);

            Assert.Throws<ArgumentException>(action);

            userRepository.Delete(userId);
        }


        [Fact]
        public void UpdateSubnetWithInvalidTime()
        {
            var userRepository = new UserRepository(_connectionString);
            var subnetRepository = new SubnetRepository(_connectionString);
            ISubnetService subnetService = new SubnetService(userRepository, subnetRepository);
            var userId = CreateUser();
            var subnet = new SubnetForm()
            {
                IP = "255.255.255.128/24",
                StartOfService = DateTime.Now,
                EndOfService = DateTime.MaxValue,
                UserId = userId
            };
            subnetService.Create(subnet);
            subnet.StartOfService = DateTime.MaxValue;
            subnet.EndOfService = DateTime.Now;

            Action action = () => subnetService.Update(subnet);

            Assert.Throws<ArgumentException>(action);

            userRepository.Delete(userId);
        }

        [Fact]
        public void DeleteWithValidUserId()
        {
            var userRepository = new UserRepository(_connectionString);
            var subnetRepository = new SubnetRepository(_connectionString);
            ISubnetService subnetService = new SubnetService(userRepository, subnetRepository);
            var userId = CreateUser();
            var subnet = new SubnetForm()
            {
                IP = "255.255.255.128/24",
                StartOfService = DateTime.Now,
                EndOfService = DateTime.MaxValue,
                UserId = userId
            };
            subnetService.Create(subnet);

            subnetService.Delete(userId);

            Action action = () => subnetService.Get(userId);
            Assert.Throws<NullReferenceException>(action);

            userRepository.Delete(userId);
        }

        [Fact]
        public void DeleteWithUserIdLessThanOne()
        {
            var userRepository = new UserRepository(_connectionString);
            var subnetRepository = new SubnetRepository(_connectionString);
            ISubnetService subnetService = new SubnetService(userRepository, subnetRepository);

            Action action = () => subnetService.Delete(-1);

            Assert.Throws<ArgumentException>(action);
        }

        private int CreateUser()
        {
            var userRepository = new UserRepository(_connectionString);
            var user = new User()
            {
                FirstName = "Василий",
                Surname = "Васильев",
                Age = 19,
                Gender = "Мужчина"
            };

            userRepository.Add(user);

            string sqlGetIdCommand = "SELECT Id FROM Users";

            using (IDbConnection dataBaseConnection = new SqlConnection(_connectionString))
            {
                return dataBaseConnection.Query<int>(sqlGetIdCommand).LastOrDefault();
            }
        }
    }
}
