using Core.Repositories.Subnet;
using Core.Repositories.User;
using Core.Services.Subnet;
using Core.ViewModels.Subnet;
using System;
using Xunit;

namespace Services.Tests
{
    public class SubnetServiceTests
    {
        private string _connectionString = "Server=(localdb)\\mssqllocaldb;Database=Planeta;Trusted_Connection=True;Integrated Security=True";

        [Fact]
        public void GetExistingSubnet()
        {
            var userRepository = new UserRepository(_connectionString);
            var subnetRepository = new SubnetRepository(_connectionString);
            ISubnetService subnetService = new SubnetService(userRepository, subnetRepository);

            SubnetViewModel result = subnetService.Get(1);

            Assert.NotNull(result);
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
    }
}
