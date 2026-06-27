using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GymSystemApp.DAL.Entities
{
    public class Membership:BaseEntity
    {
        public Member Member { get; set; } = null!;
        public int MemberId { get; set; }

        public Plan Plan { get; set; } = null!;
        public int PlanId { get; set; }

        public DateTime EndDate {  get; set; }

        [NotMapped]
        public string Status => IsActive ? "Active" : "Expired";

        [NotMapped]
        public bool IsActive => EndDate > DateTime.Now;
    }
}
