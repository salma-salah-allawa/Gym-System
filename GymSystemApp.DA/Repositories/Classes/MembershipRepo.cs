using GymSystemApp.Contexts;
using GymSystemApp.DAL.Entities;
using GymSystemApp.DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystemApp.DAL.Repositories.Classes
{
    public class MembershipRepo:GenericRepo<Membership>,IMembershipRepo
    {
        private readonly GymAppDbContext _context;

        public MembershipRepo(GymAppDbContext context):base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Membership>> GetMembershipsWithMembersAndPlansAsync(CancellationToken ct = default )
        {
            var memberships = _context.Memberships.AsNoTracking().Include(ms => ms.Plan).Include(ms => ms.Member);
            return await memberships.ToListAsync(ct);
        }
    }
}
