using GymSystemApp.DAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystemApp.DAL.Entities
{
    public class Trainer:GymUser
    {
        public Specialties Specialties { get; set; }
        public DateTime HiringDate { get; set; }

        public ICollection<Session> Sessions { get; set; }=new HashSet<Session>();
    }
}
