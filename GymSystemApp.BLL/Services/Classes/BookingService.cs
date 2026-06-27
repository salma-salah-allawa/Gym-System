using AutoMapper;
using GymManagementSystem.BLL.ViewModels.SessionsViewModels;
using GymSystemApp.BLL.Common;
using GymSystemApp.BLL.Services.Contracts;
using GymSystemApp.BLL.ViewModels.BookingsViewModels;
using GymSystemApp.BLL.ViewModels.MembershipsViewModels;
using GymSystemApp.DAL.Entities;
using GymSystemApp.DAL.Repositories.Classes;
using GymSystemApp.DAL.Repositories.Contracts;


namespace GymSystemApp.BLL.Services.Classes
{
    public class BookingService:IBookingService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public BookingService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<Result> CancelBookingAsync(int memberId, int sessionId, CancellationToken ct = default)
        {
            var session = await unitOfWork.GetRepo<Session>().GetById(sessionId, ct);
            if (session is null) return Result.NotFound("Session Is Not Found.");
            if (session.StartDate <= DateTime.Now) return Result.Fail("Can't Cancel Session Has Already Started.");

            var booking= await unitOfWork.BookingRepo.FirstOrDefaultAsync(b=>b.SessionId == sessionId && b.MemberId == memberId,true,ct);
            if (booking is null) return Result.NotFound("Booking Is Not Found.");

            unitOfWork.BookingRepo.Delete(booking);
            var rowEffected = await unitOfWork.SaveChangesAsync();
            return rowEffected > 0 ? Result.Ok() : Result.Fail("Failed to Delete The Booking.");
        }

        public async Task<Result> CreateBookingAsync(CreateBookingViewModel model, CancellationToken ct = default)
        {
            var session = await unitOfWork.GetRepo<Session>().GetById(model.SessionId, ct);
            if (session is null) return Result.NotFound("Session Is Not Found.");
            if (session.StartDate <= DateTime.Now) return Result.Fail("Can't Cancel Session Has Already Started.");

            var hasActiveMembership= await unitOfWork.MembershipRepo.AnyAsync(m=>m.MemberId == model.MemberId && m.EndDate > DateTime.Now,ct);
            if (!hasActiveMembership) return Result.Fail("Member Dosen't have Active Membership.");

            var hasAlreadyBooked = await unitOfWork.BookingRepo.AnyAsync(b => b.SessionId == model.SessionId && b.MemberId == model.MemberId, ct);
            if(hasAlreadyBooked) return Result.Fail("Member Has Already Booked.");

            var booked = await unitOfWork.SessionRepo.GetCountOfBookedSlotsAsync(model.SessionId, ct);
            if (booked >= session.Capacity) return Result.Fail("Session Is Full");

            unitOfWork.BookingRepo.Add(new Booking
            {
                SessionId = model.SessionId,
                MemberId = model.MemberId,
                IsAttended =false,
                CreatedAt = DateTime.Now
            });

            var rowEffected = await unitOfWork.SaveChangesAsync();
            return rowEffected > 0 ? Result.Ok() : Result.Fail("Failed to Create The Booking.");
        }

        public async Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default)
        {
            var bookings= await unitOfWork.SessionRepo.GetAllSessionsWithTrainerAndCategoryAsync(ct);
            if (!bookings.Any()) return [];

            var mappedSessions = mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(bookings);
            foreach (var session in mappedSessions)
            {
                session.AvailableSlots = session.Capacity - await unitOfWork.SessionRepo.GetCountOfBookedSlotsAsync(session.Id, ct);
            }
            return mappedSessions;
        }

        public async Task<IEnumerable<MemberSelectListViewModel>> GetMemberForDropDownListAsync(int sessionId, CancellationToken ct = default)
        {
            var bookings = await unitOfWork.BookingRepo.GetAll(false,ct);
            var bookedMemberIds = bookings.Select(x => x.MemberId);
            var availableMembers = await unitOfWork.GetRepo<Member>().GetAll(false,ct);

            return mapper.Map<IEnumerable<Member>, IEnumerable<MemberSelectListViewModel>>(availableMembers);
        }

        public async Task<IEnumerable<MemberForSessionViewModel>> GetMembersForOnGoingSessionAsync(int sessionId, CancellationToken ct = default)
        {
            var bookings = await unitOfWork.BookingRepo.GetBookingsBySessionIdAsync(sessionId, ct);
            return bookings.Select(b => new MemberForSessionViewModel
            {
                MemberId = b.MemberId,
                SessionId = sessionId,
                MemberName = b.Member.Name,
                BookingDate = b.CreatedAt.ToString("yyyy-MM-dd HH:mm"),
                IsAttended = b.IsAttended,
            }).ToList();
        }

        public async Task<IEnumerable<MemberForSessionViewModel>> GetMembersForUpComingSessionAsync(int sessionId, CancellationToken ct = default)
        {
            var bookings = await unitOfWork.BookingRepo.GetBookingsBySessionIdAsync(sessionId, ct);
            return bookings.Select(b => new MemberForSessionViewModel
            {
                MemberId = b.MemberId,
                SessionId = sessionId,
                MemberName = b.Member.Name,
                BookingDate = b.CreatedAt.ToString("yyyy-MM-dd HH:mm"),
            }).ToList();
        }

        public async Task<Result> MarkAttendedAsync(int memberId, int sessionId, CancellationToken ct = default)
        {
            var booking = await unitOfWork.BookingRepo.FirstOrDefaultAsync(b => b.SessionId == sessionId && b.MemberId == memberId, true, ct);
            if (booking is null) return Result.NotFound("Booking Is Not Found.");
            booking.IsAttended = true;
            booking.UpdatedAt = DateTime.Now;

            unitOfWork.BookingRepo.Update(booking);

            var rowEffected = await unitOfWork.SaveChangesAsync();
            return rowEffected > 0 ? Result.Ok() : Result.Fail("Failed to Update The Booking.");
        }
    }
}
