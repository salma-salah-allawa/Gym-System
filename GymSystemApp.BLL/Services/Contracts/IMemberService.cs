using GymSystemApp.BLL.Common;
using GymSystemApp.BLL.ViewModels.MembersViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystemApp.BLL.Services.Contracts
{
    public interface IMemberService
    {
        //GET
        Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct=default);
        Task<MemberViewModel?> GetMemberDetailsAsync(int memberId, CancellationToken ct = default);
        Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int memberId, CancellationToken ct = default);
        Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId, CancellationToken ct = default);

        //POST
        Task<Result> CreateMemberAsync(CreateMemberViewModel member, CancellationToken ct=default);
        Task<Result> UpdateMemberDetailsAysnc(int id, MemberToUpdateViewModel model, CancellationToken ct = default);
        Task<Result> DeleteMemberAsync(int memberId, CancellationToken ct = default);
    }
}
