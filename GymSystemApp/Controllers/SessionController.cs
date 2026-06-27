using GymManagementSystem.BLL.ViewModels.SessionsViewModels;
using GymSystemApp.BLL.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymSystemApp.PL.Controllers
{
    [Authorize]
    public class SessionController : Controller
    {
        private readonly ISessionService sessionService;

        public SessionController(ISessionService sessionService)
        {
            this.sessionService = sessionService;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var sessions = await sessionService.GetAllSessionsAsync(ct);
            return View(sessions);
        }
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await PopulateDropDownListsAsync(ct);
            return View();
        }

        private async Task PopulateDropDownListsAsync(CancellationToken ct)
        {
           ViewBag.Trainers = new SelectList(await sessionService.GetTrainerForDropDownListAsync(ct),"Id","Name");
           ViewBag.Categories = new SelectList(await sessionService.GetCategoryForDropDownListAsync(ct),"Id","CategoryName");
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSessionViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropDownListsAsync(ct);
                return View(model);
            }
            var result = await sessionService.CreateSessionAsync(model, ct);
            if (result.IsSuccess)
            {
               TempData["SuccessMessage"] = "Session created successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = result.ErorrMessage;
            await PopulateDropDownListsAsync(ct);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var session = await sessionService.GetSessionByIdAsync(id, ct);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(session);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var session = await sessionService.GetSessionToUpdateAsync(id, ct);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction(nameof(Index));
            }
            await PopulateDropDownListsAsync(ct);
            return View(session);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateSessionViewModel model,int id, CancellationToken ct)
        {
           if(!ModelState.IsValid)
            {
                await PopulateDropDownListsAsync(ct);
                return View(model);
            }
            var result = await sessionService.UpdateSessionAsync(id, model , ct);
            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Session updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = result.ErorrMessage;
            await PopulateDropDownListsAsync(ct);
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id,CancellationToken ct)
        {
            var Session = await sessionService.GetSessionByIdAsync(id, ct);
            if(Session is null)
            {
                TempData["ErorrMassage"] = "Session Not Found.";
                return RedirectToAction(nameof(Index));
            }
            return View(Session);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var Result= await sessionService.DeleteSessionAsync(id,ct);

            TempData[Result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] = Result.IsSuccess ? "Session Deleted successfully." : Result.ErorrMessage;
            return RedirectToAction(nameof(Index));
        }
    }
}
