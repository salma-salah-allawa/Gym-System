using GymSystemApp.BLL.Services.Classes;
using GymSystemApp.BLL.Services.Contracts;
using GymSystemApp.BLL.ViewModels.BookingsViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymSystemApp.PL.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly IBookingService bookingService;

        public BookingController(IBookingService bookingService)
        {
            this.bookingService = bookingService;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            return View(await bookingService.GetAllSessionsAsync());
        }
        [HttpGet]
        public async Task<IActionResult> Create(int id,CancellationToken ct)
        {
            var members= await bookingService.GetMemberForDropDownListAsync(id,ct);
            ViewBag.Members = new SelectList(members,"Id","Name");
            ViewBag.SessionId = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookingViewModel model, CancellationToken ct)
        {
            var result = await bookingService.CreateBookingAsync(model, ct);
            TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] =
                result.IsSuccess ? "Booking created successfully." : result.ErorrMessage;
            return RedirectToAction(nameof(GetMembersForUpcomingSession), new { id = model.SessionId });
        }

        [HttpGet]
        public async Task<IActionResult> GetMembersForUpcomingSession(int id, CancellationToken ct)
        {
           return View(await bookingService.GetMembersForUpComingSessionAsync(id, ct));
        }

        [HttpGet]
        public async Task<IActionResult> GetMembersForOngoingSessions(int id, CancellationToken ct)
        {
            return View(await bookingService.GetMembersForOnGoingSessionAsync(id, ct));
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int memberId, int sessionId, CancellationToken ct)
        {
            var result = await bookingService.CancelBookingAsync(memberId, sessionId, ct);
            TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] =
                result.IsSuccess ? "Booking cancelled successfully." : result.ErorrMessage;
            return RedirectToAction(nameof(GetMembersForUpcomingSession), new { id = sessionId });
        }

        [HttpPost]
        public async Task<IActionResult> Attended(int memberId, int sessionId, CancellationToken ct)
        {
            var result = await bookingService.MarkAttendedAsync(memberId, sessionId, ct);
            TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] =
                result.IsSuccess ? "Attendance recorded." : result.ErorrMessage;
            return RedirectToAction(nameof(GetMembersForOngoingSessions), new { id = sessionId });
        }

    }
}
