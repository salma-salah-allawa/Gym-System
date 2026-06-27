using AutoMapper;
using GymSystemApp.BLL.Common;
using GymSystemApp.BLL.Services.Contracts;
using GymSystemApp.BLL.ViewModels.MembershipsViewModels;
using GymSystemApp.DAL.Entities;
using GymSystemApp.DAL.Repositories.Contracts;
using Member = GymSystemApp.DAL.Entities.Member;

namespace GymSystemApp.BLL.Services.Classes
{
    public class MembershipService : IMembershipService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public MembershipService(IUnitOfWork unitOfWork,IMapper mapper)
        { 
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<Result> CreateMembershipAsync(CreateMembershipViewModel model, CancellationToken ct = default)
        {
            var memberExiest = await unitOfWork.GetRepo<Member>().AnyAsync(m => m.Id == model.MemberId,ct);
            if (!memberExiest) return Result.NotFound("Member Not Found.");

            var PlanExiest = await unitOfWork.GetRepo<Plan>().GetById(model.PlanId,ct);
            if (PlanExiest is null) return Result.NotFound("Plan Not Found.");
            if (!PlanExiest.IsActive) return Result.Fail("Plan Is Not Active.");
            var hasActive = await unitOfWork.MembershipRepo.AnyAsync(m=>m.MemberId == model.MemberId && m.EndDate > DateTime.Now,ct);
            if (hasActive) return Result.Fail("Member Already Has an Active Membership.");

            var mappedMembership = new Membership
            {
                MemberId = model.MemberId,
                PlanId = model.PlanId,
                CreatedAt = DateTime.Now,
                EndDate = (model.StartDate ?? DateTime.Now).AddDays(PlanExiest.DurationDays)
            };
            unitOfWork.MembershipRepo.Add(mappedMembership);

            var rowEffected = await unitOfWork.SaveChangesAsync();
            return rowEffected > 0 ? Result.Ok() : Result.Fail("Failed to Create Membership.");
        }

        public async Task<Result> DeleteActiveMembershipAsync(int membershipId, CancellationToken ct = default)
        {
           var activemembership=await unitOfWork.MembershipRepo.FirstOrDefaultAsync(m=>m.MemberId == membershipId && m.EndDate >DateTime.Now,true,ct);
            if (activemembership is null) return Result.NotFound("No Active Membership Found For Member.");

            unitOfWork.MembershipRepo.Delete(activemembership.Id);

            var rowEffected = await unitOfWork.SaveChangesAsync();
            return rowEffected > 0 ? Result.Ok() : Result.Fail("Failed to Delete active Membership.");
        }

        public async Task<IEnumerable<MembershipViewModel>> GetAllMembershipsAsync(CancellationToken ct = default)
        {
            var memberships = await unitOfWork.MembershipRepo.GetMembershipsWithMembersAndPlansAsync(ct);
            if (!memberships.Any()) return [];
            
            var MappedMemberships = mapper.Map<IEnumerable<Membership>,IEnumerable<MembershipViewModel>>(memberships);
            return  MappedMemberships;
        }

        public async Task<IEnumerable<MemberSelectListViewModel>> GetMemberForDropDownListAsync(CancellationToken ct = default)
        {
            var members = await unitOfWork.GetRepo<Member>().GetAll(false, ct);
            return mapper.Map<IEnumerable<Member>,IEnumerable<MemberSelectListViewModel>>(members);
        }

        public async Task<IEnumerable<PlanSelectListViewModel>> GetPlanForDropDownListAsync(CancellationToken ct = default)
        {
            var plans = await unitOfWork.GetRepo<Plan>().GetAll(false, ct);
            return mapper.Map<IEnumerable<Plan>, IEnumerable<PlanSelectListViewModel>>(plans);
        }
    }
}
