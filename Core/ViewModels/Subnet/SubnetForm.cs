using System;
using System.ComponentModel.DataAnnotations;

namespace Core.ViewModels.Subnet
{
    public class SubnetForm
    {
        public int Id { get; set; }

        [Required]
        public string IP { get; set; }

        public DateTime StartOfService { get; set; }

        public DateTime EndOfService { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
