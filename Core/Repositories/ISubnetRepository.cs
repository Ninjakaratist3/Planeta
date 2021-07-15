using Core.Models;
using Core.ViewModels.Subnet;
using System.Collections.Generic;

namespace Core.Repositories
{
    public interface ISubnetRepository
    {
        Subnet Get(int userId);
        void Add(SubnetForm model);
        void Update(SubnetForm model);
        void Delete(int userId);
    }
}
