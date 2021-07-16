using Core.ViewModels.Subnet;

namespace Core.ViewModels.User
{
    public class UserViewModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string Surname { get; set; }

        public int Age { get; set; }

        public string Gender { get; set; }

        public SubnetViewModel Subnet { get; set; }
    }
}
