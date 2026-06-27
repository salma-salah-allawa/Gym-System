using AutoMapper;
using GymManagementSystem.BLL.ViewModels.PlansViewModels;
using GymSystemApp.BLL.Common;
using GymSystemApp.BLL.Services.Contracts;
using GymSystemApp.DAL.Entities;
using GymSystemApp.DAL.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystemApp.BLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public PlanService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<Result> ActivatePlanAsync(int planId, CancellationToken ct = default)
        {
            var planRepo = unitOfWork.GetRepo<Plan>();
            var plan = await planRepo.GetById(planId, ct);
            if(plan is null) return Result.NotFound("Plan Not Found");
            if(plan.IsActive) return Result.Fail("Plan Is Already Active");
            
            plan.IsActive = true;
            planRepo.Update(plan);

            var rowAffected = await unitOfWork.SaveChangesAsync();
            return rowAffected > 0 ? Result.Ok() : Result.Fail("Failed To Activate Plan");
        }

        public async Task<Result> DeactivatePlanAsync(int planId, CancellationToken ct = default)
        {
            var planRepo = unitOfWork.GetRepo<Plan>();
            var plan = await planRepo.GetById(planId, ct);
            if (plan is null) return Result.NotFound("Plan Not Found");
            if (!plan.IsActive) return Result.Fail("Plan Is Already InActive");
            if(plan.Memberships.Any(m => m.IsActive)) return Result.Fail("Plan Cannot Be Deactivated Because It Has Active Memberships");

            plan.IsActive = false;
            planRepo.Update(plan);

            var rowAffected = await unitOfWork.SaveChangesAsync();
            return rowAffected > 0 ? Result.Ok() : Result.Fail("Failed To Deactivate Plan");
        }

        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default)
        {
            var plans = await unitOfWork.GetRepo<Plan>().GetAll(false,ct);
            if(!plans.Any()) return null;
            var MappedPlans = mapper.Map<IEnumerable<Plan>,IEnumerable<PlanViewModel>>(plans);
            return MappedPlans;
        }

        public async Task<PlanViewModel?> GetPlanByIdAsync(int planId, CancellationToken ct = default)
        {
            var plan = await unitOfWork.GetRepo<Plan>().GetById(planId, ct);
            if(plan is null) return null;

            var MappedPlan = mapper.Map<Plan, PlanViewModel>(plan);
            return MappedPlan;
        }

        public async Task<UpdatePlanViewModel> GetPlanToUpdateAsync(int planId, CancellationToken ct = default)
        {
            var plan = await unitOfWork.GetRepo<Plan>().GetById(planId, ct);
            if(plan is null) return null;

            var MappedPlan = mapper.Map<Plan, UpdatePlanViewModel>(plan);
            return MappedPlan;
        }

        public async Task<Result> UpdatePlanAsync(int planId, UpdatePlanViewModel model, CancellationToken ct = default)
        {
            var planRepo = unitOfWork.GetRepo<Plan>();
            var plan = await planRepo.GetById(planId, ct);
            if(plan is null) return Result.NotFound("Plan Not Found");

            if(model.PlanName != plan.Name) return Result.Fail("Plan Name Cannot Be Changed");
            if(plan.Memberships.Any(m=>m.IsActive)) return Result.Fail("Plan Cannot Be Updated Because It Has Active Memberships"); 

            plan.UpdatedAt = DateTime.Now;
            mapper.Map(model, plan);
            planRepo.Update(plan);

            var rowAffected = await unitOfWork.SaveChangesAsync();
            return rowAffected > 0 ? Result.Ok() : Result.Fail("Failed To Update Plan");
        }
    }
}
