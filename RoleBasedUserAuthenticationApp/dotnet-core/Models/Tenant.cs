using System;
using System.ComponentModel.DataAnnotations;

namespace startup_kit_api.Models
{
    public class Tenant
    {
        [Key, Required]
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}