using GymManagementSystem.BLL.ViewModels.PlansViewModels;
using GymSystemApp.BLL.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystemApp.BLL.Services.Contracts
{
    public interface IPlanService
    {
        Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default);
        Task <PlanViewModel?> GetPlanByIdAsync(int planId, CancellationToken ct = default);
        Task<UpdatePlanViewModel> GetPlanToUpdateAsync(int planId, CancellationToken ct = default);

        Task<Result> UpdatePlanAsync(int planId, UpdatePlanViewModel model, CancellationToken ct = default);

        Task<Result> ActivatePlanAsync(int planId,CancellationToken ct = default);

        Task<Result> DeactivatePlanAsync(int planId,CancellationToken ct = default);
    }
}
