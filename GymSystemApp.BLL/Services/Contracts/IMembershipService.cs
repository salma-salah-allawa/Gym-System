using GymSystemApp.BLL.Common;
using GymSystemApp.BLL.ViewModels.MembershipsViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystemApp.BLL.Services.Contracts
{
    public interface IMembershipService
    {
        Task<IEnumerable<MembershipViewModel>> GetAllMembershipsAsync(CancellationToken ct = default);
        Task<IEnumerable<PlanSelectListViewModel>> GetPlanForDropDownListAsync(CancellationToken ct = default);
        Task<IEnumerable<MemberSelectListViewModel>> GetMemberForDropDownListAsync(CancellationToken ct = default);

        Task<Result> CreateMembershipAsync(CreateMembershipViewModel model, CancellationToken ct = default);
        Task<Result> DeleteActiveMembershipAsync(int membershipId, CancellationToken ct = default);
    }
}
