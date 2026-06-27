using GymManagementSystem.BLL.ViewModels.SessionsViewModels;
using GymSystemApp.BLL.Common;
using GymSystemApp.BLL.ViewModels.BookingsViewModels;
using GymSystemApp.BLL.ViewModels.MembershipsViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystemApp.BLL.Services.Contracts
{
    public interface IBookingService
    {
        Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default);
        Task<IEnumerable<MemberForSessionViewModel>> GetMembersForUpComingSessionAsync(int sessionId,CancellationToken ct = default);
        Task<IEnumerable<MemberForSessionViewModel>> GetMembersForOnGoingSessionAsync(int sessionId, CancellationToken ct = default);
        Task<Result> CreateBookingAsync(CreateBookingViewModel model, CancellationToken ct = default);
        Task<IEnumerable<MemberSelectListViewModel>> GetMemberForDropDownListAsync(int sessionId, CancellationToken ct = default);
        Task<Result> CancelBookingAsync(int memberId,int sessionId,CancellationToken ct = default);
        Task<Result> MarkAttendedAsync(int memberId, int sessionId, CancellationToken ct = default);

    }
}
