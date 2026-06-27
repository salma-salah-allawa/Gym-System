using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystemApp.DAL.Entities
{
    public class Session:BaseEntity
    {
        public string Description { get; set; } = null!;
        public int Capacity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Trainer Trainer { get; set; } = null!;
        public int TrainerId { get; set; }

        public Category Category { get; set; } = null!;
        public int CategoryId { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
    }
}
