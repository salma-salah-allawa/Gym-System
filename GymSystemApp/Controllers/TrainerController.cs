using GymManagementSystem.BLL.ViewModels.TrainersViewModels;
using GymSystemApp.BLL.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymSystemApp.PL.Controllers
{
    [Authorize]
    public class TrainerController : Controller
    {
        private readonly ITrainerService trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            this.trainerService = trainerService;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var trainers = await trainerService.GetAllTrainersAsync(ct);
            return View(trainers);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Create(CreateTrainerViewModel trainer, CancellationToken ct)
        {
            if(!ModelState.IsValid)
            {
                return View(trainer);
            }
           var result = await trainerService.CreateTrainerAsync(trainer, ct);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.ErorrMessage;
                return View(trainer);
            }
            TempData["SuccessMessage"] = "Trainer created successfully.";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var trainer = await trainerService.GetTrainerDetailsAsync(id, ct);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var trainer = await trainerService.GetTrainerToUpdateAsync(id, ct);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TrainerToUpdateViewModel trainer, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(trainer);
            }
            var result = await trainerService.UpdateTrainerAsync(id, trainer, ct);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.ErorrMessage;
                return View(trainer);
            }
            TempData["SuccessMessage"] = "Trainer updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await trainerService.DeleteTrainerAsync(id, ct);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.ErorrMessage;
                return RedirectToAction(nameof(Index));
            }
            TempData["SuccessMessage"] = "Trainer deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
