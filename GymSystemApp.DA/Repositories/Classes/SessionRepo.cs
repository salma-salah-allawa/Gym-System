using GymSystemApp.Contexts;
using GymSystemApp.DAL.Entities;
using GymSystemApp.DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystemApp.DAL.Repositories.Classes
{
    public class SessionRepo : GenericRepo<Session>, ISessionRepo
    {
        private readonly GymAppDbContext _context;
        public SessionRepo(GymAppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(CancellationToken ct)
        {
            var Sessions = _context.Sessions.AsNoTracking().Include(s => s.Trainer).Include(s => s.Category);
            return await Sessions.ToListAsync(ct);
        }

        public async Task<int> GetCountOfBookedSlotsAsync(int sessionId, CancellationToken ct)
        {
            return await _context.Bookings.AsNoTracking().CountAsync(s => s.SessionId == sessionId, ct);
        }

        public async Task<Session> GetSessionByIdWithTrainerAndCategoryAsync(int sessionId, CancellationToken ct)
        {
            var Session = _context.Sessions.Include(s => s.Trainer).Include(s => s.Category).FirstOrDefaultAsync(s=>s.Id == sessionId);
            return await Session;
        }
    }
}
