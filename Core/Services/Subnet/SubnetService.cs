using Core.Repositories.Subnet;
using Core.Repositories.User;
using Core.ViewModels.Subnet;
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

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

            if (subnet == null)
            {
                throw new NullReferenceException();
            }

            return ConvertSubnetToSubnetViewModel(subnet);
        }

        public void Create(SubnetForm model)
        {
            if (!SubnetFormIsValid(model))
            {
                throw new ArgumentException();
            }

            var subnet = ConvertSubnetFormToSubnet(model);

            _subnetRepository.Add(subnet);
        }

        public void Update(SubnetForm model)
        {
            if (!SubnetFormIsValid(model))
            {
                throw new ArgumentException();
            }

            var subnet = ConvertSubnetFormToSubnet(model);

            _subnetRepository.Update(subnet);
        }

        public void Delete(int userId)
        {
            if (userId < 1)
            {
                throw new ArgumentException();
            }

            _subnetRepository.Delete(userId);
        }

        public Models.Subnet ConvertSubnetFormToSubnet(SubnetForm model)
        {
            var IPWithMask = model.IP.Split('/');

            var subnet = new Models.Subnet();
            subnet.Id = model.Id;
            subnet.IP = IPAddress.Parse(IPWithMask[0]);
            subnet.Mask = GetMask(IPWithMask[1]);
            subnet.StartOfService = model.StartOfService;
            subnet.EndOfService = model.EndOfService;
            subnet.UserId = model.UserId;

            return subnet;
        }

        public SubnetViewModel ConvertSubnetToSubnetViewModel(Models.Subnet model)
        {
            var subnetViewModel = new SubnetViewModel();
            subnetViewModel.Id = model.Id;
            subnetViewModel.IP = model.IP.ToString();
            subnetViewModel.Mask = model.Mask;
            subnetViewModel.StartOfService = model.StartOfService;
            subnetViewModel.EndOfService = model.EndOfService;
            subnetViewModel.UserId = model.UserId;
            subnetViewModel.User = _userRepository.Get(model.UserId);

            return subnetViewModel;
        }

        private bool SubnetFormIsValid(SubnetForm model)
        {
            Regex ipRegex = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}/\d{1,2}");
            if (!ipRegex.IsMatch(model.IP))
            {
                return false;
            }

            if (model?.StartOfService >= model?.EndOfService)
            {
                return false;
            }

            if (_userRepository.Get(model.UserId) == null)
            {
                return false;
            }

            return true;
        }

        private string GetMask(string mask)
        {
            var binaryMask = new StringBuilder();
            int decimalMask = Convert.ToInt32(mask);

            for (int i = 1; i <= 32; i++)
            {
                if (decimalMask >= i)
                {
                    binaryMask.Append("1");
                }
                else
                {
                    binaryMask.Append("0");
                }

                if (i % 8 == 0 && i != 32)
                {
                    binaryMask.Append(".");
                }
            }

            return string.Join('.', binaryMask.ToString().Split(".").Select(x => FromBinary(x)).ToList());
        }

        private int FromBinary(string binaryNumber)
        {
            int number = 0;
            foreach (var symbol in binaryNumber)
            {
                number <<= 1;
                number += symbol - '0';
            }

            return number;
        }
    }
}
