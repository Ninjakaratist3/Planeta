namespace Core.Models
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string Surname { get; set; }

        public int Age { get; set; }

        public string Gender { get; set; }

        public int? SubnetId { get; set; }
    }
}
