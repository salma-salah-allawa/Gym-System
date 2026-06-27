using GymSystemApp.BLL.Services.Classes;
using GymSystemApp.BLL.Services.Contracts;
using GymSystemApp.BLL.ViewModels.MembershipsViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymSystemApp.PL.Controllers
{
    [Authorize]
    public class MembershipController : Controller
    {
        private readonly IMembershipService membershipService;

        public MembershipController(IMembershipService membershipService)
        {
            this.membershipService = membershipService;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var memberships = await membershipService.GetAllMembershipsAsync(ct);
            return View(memberships);
        }
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await PopulateDropDownListsAsync(ct);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateMembershipViewModel model,CancellationToken ct)
        {
            if(!ModelState.IsValid)
            {
                await PopulateDropDownListsAsync(ct);
                TempData["ErrorMessage"] = "Feild to Create Membership.";
                return View(model);  
            }
            var membership = await membershipService.CreateMembershipAsync(model, ct);
            if (!membership.IsSuccess)
            {
                await PopulateDropDownListsAsync(ct);
                TempData["ErrorMessage"] = membership.ErorrMessage;
                return View(model);
            }
            
            TempData["SuccessMessage"] = "Membership Created Successfully.";
            return RedirectToAction(nameof(Index));
        }
        private async Task PopulateDropDownListsAsync(CancellationToken ct)
        {
            ViewBag.Plans = new SelectList(await membershipService.GetPlanForDropDownListAsync(ct) ,"Id","Name");
            ViewBag.Members = new SelectList(await membershipService.GetMemberForDropDownListAsync(ct), "Id", "Name");
        }
        [HttpPost]
        public async Task<IActionResult> Cancel(int id,CancellationToken ct)
        {
             var result = await membershipService.DeleteActiveMembershipAsync(id,ct);
            TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] = result.IsSuccess ? "Membership Deleted Successfully." : result.ErorrMessage;
            return RedirectToAction(nameof(Index));
        }
    }
}
