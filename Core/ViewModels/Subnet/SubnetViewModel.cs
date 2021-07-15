using System;
using System.Net;

namespace Core.ViewModels.Subnet
{
    public class SubnetViewModel
    {
        public int Id { get; set; }

        public string IP { get; set; }

        public string Mask { get; set; }

        public DateTime StartOfService { get; set; }

        public DateTime EndOfService { get; set; }

        public int UserId { get; set; }

        public Models.User User { get; set; }
    }
}
