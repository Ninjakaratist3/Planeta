using System;

namespace Core.ViewModels.Subnet
{
    public class SubnetForm
    {
        public int Id { get; set; }

        public string IP { get; set; }

        public DateTime StartOfService { get; set; }

        public DateTime EndOfService { get; set; }

        public int UserId { get; set; }
    }
}
