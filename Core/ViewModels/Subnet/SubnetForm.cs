using System;

namespace Core.ViewModels.Subnet
{
    public class SubnetForm
    {
        public string IP { get; set; }

        public DateTime StartOfService { get; set; }

        public DateTime EndOfService { get; set; }

        public Core.Models.User User { get; set; }
    }
}
