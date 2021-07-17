using System;
using System.ComponentModel.DataAnnotations;

namespace Core.ViewModels.Subnet
{
    public class SubnetForm
    {
        public int Id { get; set; }

        [Required]
        public string IP { get; set; }

        public DateTime StartOfService { get; set; } = DateTime.Now;

        public DateTime EndOfService { get; set; } = DateTime.MaxValue;

        [Required]
        public int UserId { get; set; }
    }
}
