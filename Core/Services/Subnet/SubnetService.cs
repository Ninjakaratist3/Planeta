using Core.Repositories.Subnet;
using Core.Repositories.User;
using Core.ViewModels.Subnet;
using System;
using System.Linq;
using System.Net;
using System.Text;

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
            model.IP = IPWithMask[0];
            var subnetForm = new Models.Subnet()
            {
                Id = model.Id,
                IP = IPAddress.Parse(model.IP),
                Mask = GetMask(IPWithMask[1]),
                StartOfService = model.StartOfService,
                EndOfService = model.EndOfService,
                UserId = model.UserId
            };

            return subnetForm;
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

        private long FromBinary(string input)
        {
            long big = 0;
            foreach (var c in input)
            {
                big <<= 1;
                big += c - '0';
            }

            return big;
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

        private bool SubnetFormIsValid(SubnetForm model)
        {
            // TODO: IP Validation

            if (model.StartOfService >= model.EndOfService)
            {
                return false;
            }

            return true;
        }
    }
}
