using GymSystemApp.Contexts;
using GymSystemApp.DAL.Entities;
using GymSystemApp.DAL.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystemApp.DAL.Repositories.Classes
{
    public class TrainerRepo:GenericRepo<Trainer>, ITrainerRepo
    {
        private readonly GymAppDbContext _context;

        public TrainerRepo(GymAppDbContext context): base(context)
        {
            _context = context;
        }
    }
}
