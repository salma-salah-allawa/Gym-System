using GymSystemApp.Contexts;
using GymSystemApp.DAL.Entities;
using GymSystemApp.DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace GymSystemApp.DAL.Repositories.Classes
{
    public class PlanRepo : GenericRepo<Plan>,IPlanRepo
    {
        private readonly GymAppDbContext _context;
        public PlanRepo(GymAppDbContext context): base(context) 
        {
            _context = context;
        }
    }
}
