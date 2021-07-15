using Core.Repositories.Subnet;
using Core.Repositories.User;
using Core.ViewModels.Subnet;
using System;
using System.Net;

namespace Core.Services.Subnet
{
    public class SubnetService : ISubnetService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISubnetRepository _subnetRepository;

        public SubnetService(IUserRepository userRepository, ISubnetRepository subnetRepository)
        {
            _userRepository = userRepository;
            _subnetRepository = subnetRepository;
        }

        public SubnetViewModel Get(int userId)
        {
            var subnet = _subnetRepository.Get(userId);

            return ConvertSubnetToSubnetViewModel(subnet);
        }

        public void Create(SubnetForm model)
        {
            var subnet = ConvertSubnetFormToSubnet(model);

            _subnetRepository.Add(subnet);
        }

        public void Update(SubnetForm model)
        {
            var subnet = ConvertSubnetFormToSubnet(model);

            _subnetRepository.Update(subnet);
        }

        public void Delete(int userId)
        {
            _subnetRepository.Delete(userId);
        }

        public Models.Subnet ConvertSubnetFormToSubnet(SubnetForm model)
        {
            var IPWithMask = model.IP.Split('/');
            model.IP = IPWithMask[0];
            var subnetForm = new Models.Subnet()
            {
                Id = model.Id,
                IP = IPAddress.Parse(model.IP),
                Mask = Convert.ToString(Convert.ToInt32(IPWithMask[1]), 2).PadLeft(8, '0'),
                StartOfService = model.StartOfService,
                EndOfService = model.EndOfService,
                UserId = model.UserId
            };

            return subnetForm;
        }

        public SubnetViewModel ConvertSubnetToSubnetViewModel(Models.Subnet model)
        {
            var subnetForm = new SubnetViewModel()
            {
                Id = model.Id,
                IP = model.IP.ToString(),
                Mask = model.Mask,
                StartOfService = model.StartOfService,
                EndOfService = model.EndOfService,
                UserId = model.UserId,
                User = _userRepository.Get(model.UserId)
            };

            return subnetForm;
        }
    }
}
