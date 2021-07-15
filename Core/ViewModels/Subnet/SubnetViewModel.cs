using System;
using System.Net;

namespace Core.ViewModels.Subnet
{
    class SubnetViewModel
    {
        public IPAddress IP { get; set; }

        public DateTime StartOfService { get; set; }

        public DateTime EndOfService { get; set; }

        public int UserId { get; set; }
    }
}
