using GymSystemApp.Contexts;
using GymSystemApp.DAL.Entities;
using GymSystemApp.DAL.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystemApp.DAL.Repositories.Classes
{
    public class MemberRepo:GenericRepo<Member>,IMemberRepo
    {
        private readonly GymAppDbContext _context;
        public MemberRepo(GymAppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
