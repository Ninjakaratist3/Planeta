using Core.ViewModels.Subnet;

namespace Core.Services.Subnet
{
    public interface ISubnetService
    {
        public SubnetViewModel Get(int userId);
        public void Create(SubnetForm model);
        public void Update(SubnetForm model);
        public void Delete(int userId);
        public Models.Subnet ConvertSubnetFormToSubnet(SubnetForm model);
        public SubnetViewModel ConvertSubnetToSubnetViewModel(Models.Subnet model);
    }
}
