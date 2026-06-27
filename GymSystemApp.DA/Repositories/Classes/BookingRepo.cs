using GymSystemApp.Contexts;
using GymSystemApp.DAL.Entities;
using GymSystemApp.DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymSystemApp.DAL.Repositories.Classes
{
    public class BookingRepo:GenericRepo<Booking> ,IBookingRepo
    {
        private readonly GymAppDbContext _context;

        public BookingRepo(GymAppDbContext context):base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Booking>> GetBookingsBySessionIdAsync(int sessionId, CancellationToken ct = default)
        {
            return await _context.Bookings.AsNoTracking().Include(b => b.Member).Where(b => b.SessionId == sessionId).ToListAsync(ct);
        }
    }
}
