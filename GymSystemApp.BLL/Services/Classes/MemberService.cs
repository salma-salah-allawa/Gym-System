using AutoMapper;
using GymSystemApp.BLL.Common;
using GymSystemApp.BLL.Services.Contracts;
using GymSystemApp.BLL.ViewModels.MembersViewModels;
using GymSystemApp.DAL.Entities;
using GymSystemApp.DAL.Repositories.Contracts;


namespace GymSystemApp.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IAttatchmentService attatchment;

        public MemberService(IUnitOfWork unitOfWork,IMapper mapper,IAttatchmentService attatchment)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.attatchment = attatchment;
        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await unitOfWork.GetRepo<Member>().GetAll(false, ct);
            if (!members.Any()) return [];
            var membersViweModel = mapper.Map<IEnumerable<Member>, IEnumerable<MemberViewModel>>(members);
            return membersViweModel;
        }

        public async Task<MemberViewModel?> GetMemberDetailsAsync(int memberId, CancellationToken ct = default)
        {
            var member = await unitOfWork.GetRepo<Member>().GetById(memberId, ct);
            if (member is null) return null;
            var memberViweModel = mapper.Map<Member, MemberViewModel>(member);

            var activeMembership = await unitOfWork.GetRepo<Membership>().FirstOrDefaultAsync(ms => ms.MemberId == memberId && ms.EndDate > DateTime.Now ,false,ct);
            if(activeMembership is not null)
            {
                var activePlan = await unitOfWork.GetRepo<Plan>().GetById(activeMembership.PlanId, ct);

                memberViweModel.PlanName = activePlan?.Name;
                memberViweModel.MembershipStartDate = activeMembership.CreatedAt.ToString();
                memberViweModel.MembershipEndDate = activeMembership.EndDate.ToString();
            }
            return memberViweModel;
        }

        public async Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId, CancellationToken ct = default)
        {
            var memberHealthRec = await unitOfWork.GetRepo<HealthRecord>().FirstOrDefaultAsync(hr => hr.MemberId == memberId, false, ct);
            if (memberHealthRec is null) return null;

            return mapper.Map<HealthRecord, HealthRecordViewModel>(memberHealthRec);
        }

        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int memberId, CancellationToken ct = default)
        {
            var member = await unitOfWork.GetRepo<Member>().GetById(memberId, ct);
            if (member is null) return null;
            return mapper.Map<Member, MemberToUpdateViewModel>(member);
        }

        public async Task<Result> CreateMemberAsync(CreateMemberViewModel member, CancellationToken ct = default)
        {
            var EmailExsists = await unitOfWork.GetRepo<Member>().AnyAsync(m => m.Email== member.Email, ct);
            var PhoneExsists = await unitOfWork.GetRepo<Member>().AnyAsync(m => m.Phone== member.Phone, ct);

            if (EmailExsists || PhoneExsists) return Result.Validation("Email And Phone Must be Uinque.");

            var newMember = mapper.Map<CreateMemberViewModel, Member>(member);
            var newPhotoName = await attatchment.UploadAsync(member.PhotoFile.OpenReadStream(), member.PhotoFile.FileName, "MemberPictures",ct);
            if (string.IsNullOrEmpty(newPhotoName)) return Result.Fail("Failed To Upload Member Photo.");
            newMember.Photo = newPhotoName;

            unitOfWork.GetRepo<Member>().Add(newMember);

            var rowsAffected = await unitOfWork.SaveChangesAsync();
            return rowsAffected > 0 ? Result.Ok() : Result.Fail("Failed to create member.");
        }

        public async Task<Result> DeleteMemberAsync(int memberId, CancellationToken ct = default)
        {
            var member = await unitOfWork.GetRepo<Member>().GetById(memberId, ct);
            if (member is null) return Result.NotFound("Member Is Not Found.");

            var HasFutureSessions = await unitOfWork.GetRepo<Booking>().AnyAsync(b => b.MemberId == memberId && b.Session.EndDate > DateTime.Now, ct);
            if (HasFutureSessions) return Result.Validation("Can't Delete a Memeber That Have Future Sessions");
            unitOfWork.GetRepo<Member>().Delete(memberId);

            if (member.Photo is not null)
                attatchment.DeleteAsync(member.Photo, "MemberPictures");

            var rowsAffected = await unitOfWork.SaveChangesAsync();
            return rowsAffected > 0 ? Result.Ok() : Result.Fail("Failed to delete member.");
        }

        public async Task<Result> UpdateMemberDetailsAysnc(int id, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            var member = await unitOfWork.GetRepo<Member>().GetById(id, ct);
            if (member is null) return Result.NotFound("Member Not Found.");

            if(await unitOfWork.GetRepo<Member>().AnyAsync(m => m.Email == model.Email && m.Id != id, ct)) return Result.Validation("Email Must be Unique.");
            if (await unitOfWork.GetRepo<Member>().AnyAsync(m => m.Phone == model.Phone && m.Id != id, ct)) return Result.Validation("Phone Must be Unique.");
            
            mapper.Map(model, member);
            member.UpdatedAt = DateTime.Now;

            unitOfWork.GetRepo<Member>().Update(member);

            var rowsAffected = await unitOfWork.SaveChangesAsync();
            return rowsAffected > 0 ? Result.Ok() : Result.Fail("Failed to update member details.");
        }
    }
}
