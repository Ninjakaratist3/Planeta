using System.ComponentModel.DataAnnotations;

namespace Core.ViewModels.User
{
    public class UserForm
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public int Age { get; set; }

        public string Gender { get; set; }
    }
}
