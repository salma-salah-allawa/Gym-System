using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GymSystemApp.DAL.Entities
{
    public class HealthRecord:BaseEntity
    {
        public decimal Height { get; set; }

        public decimal Weight { get; set; }

        [Required, MaxLength(5)]
        public string BloodType { get; set; } = null!;

        [MaxLength(500)]
        public string? Note { get; set; }

        public Member Member { get; set; }=null!;

        public int MemberId { get; set; }
    }
}
