using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystemApp.DAL.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
