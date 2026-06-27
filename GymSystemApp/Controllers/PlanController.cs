using GymManagementSystem.BLL.ViewModels.PlansViewModels;
using GymSystemApp.BLL.Services.Contracts;
using GymSystemApp.DAL.Entities;
using GymSystemApp.DAL.Repositories.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GymSystemApp.Controllers
{
    [Authorize]
    public class PlanController : Controller
    {
        private readonly IPlanService planService;

        public PlanController(IPlanService planService)
        {
            this.planService = planService;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var plans = await planService.GetAllPlansAsync(ct);
            return View(plans);
        }
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var plan = await planService.GetPlanByIdAsync(id,ct);
            if (plan == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var plan = await planService.GetPlanToUpdateAsync(id, ct);
            if(plan is null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(plan);  
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id,UpdatePlanViewModel model, CancellationToken ct)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await planService.UpdatePlanAsync(id, model, ct);
            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Plan Updated Successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = result.ErorrMessage;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Activate(int id, CancellationToken ct)
        {
            var result = await planService.ActivatePlanAsync(id, ct);
            TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] = result.IsSuccess ? "Plan Status Changed." : result.ErorrMessage;
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]

        public async Task<IActionResult> Deactivate(int id, CancellationToken ct)
        {
            var result = await planService.DeactivatePlanAsync(id,ct);

            if (!result.IsSuccess)
            {
               TempData["ErrorMessage"] = result.ErorrMessage;
            }
            TempData["SuccessMessage"] = "Plan Status Changed.";
            return RedirectToAction(nameof(Index));
        }
    }
}
