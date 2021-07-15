using Core.ViewModels.Subnet;

namespace Core.Repositories.Subnet
{
    public interface ISubnetRepository
    {
        Models.Subnet Get(int userId);
        void Add(Models.Subnet model);
        void Update(Models.Subnet model);
        void Delete(int userId);
    }
}
