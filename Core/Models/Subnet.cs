using System;
using System.Net;

namespace Core.Models
{
    public class Subnet
    {
        public int Id { get; set; }

        public IPAddress IP { get; set; }

        public DateTime StartOfService { get; set; }

        public DateTime EndOfService { get; set; }

        public int UserId { get; set; }
    }
}
