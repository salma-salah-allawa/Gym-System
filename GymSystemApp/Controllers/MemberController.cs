using GymSystemApp.BLL.Services.Classes;
using GymSystemApp.BLL.Services.Contracts;
using GymSystemApp.BLL.ViewModels.MembersViewModels;
using GymSystemApp.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymSystemApp.PL.Controllers
{
    [Authorize(Roles ="SuperAdmin")]
    public class MemberController : Controller
    {
        private readonly IMemberService memberService;
        private readonly IAttatchmentService attatchment;

        public MemberController(IMemberService memberService,IAttatchmentService attatchment)
        {
            this.memberService = memberService;
            this.attatchment = attatchment;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var Members = await memberService.GetAllMembersAsync(ct);
            return View(Members);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMemberViewModel memberVM, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(memberVM);

            var result = await memberService.CreateMemberAsync(memberVM, ct);
            if (result.IsSuccess) TempData["Success"] = "Member created successfully.";
            else TempData["Failed"] = result.ErorrMessage;
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> MemberDetails(int id, CancellationToken ct)
        {
            var member = await memberService.GetMemberDetailsAsync(id, ct);
            if (member is null)
            {
                TempData["Failed"] = "Member not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        [HttpGet]
        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken ct)
        {
            var healthRecord = await memberService.GetMemberHealthRecordAsync(id, ct);
            if (healthRecord is null)
            {
                TempData["Failed"] = "Health Record not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(healthRecord);
        }
        [HttpGet]
        public async Task<IActionResult> EditMember(int id, CancellationToken ct)
        {
            var member = await memberService.GetMemberToUpdateAsync(id, ct);
            if (member is null)
            {
                TempData["Failed"] = "Member not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }
        [HttpPost]
        public async Task<IActionResult> EditMember(int id, MemberToUpdateViewModel memberToUpdate, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(memberToUpdate);
            var result = await memberService.UpdateMemberDetailsAysnc(id, memberToUpdate, ct);

            if (result.IsSuccess)
            {
                TempData["Success"] = "Member updated successfully.";
                return RedirectToAction(nameof(Index));

            }
                TempData["Failed"] = result.ErorrMessage;
                return View(memberToUpdate);
                //TempData[result.IsSuccess ? "Success" : "Failed"] = result.IsSuccess
                //    ? "Member updated successfully."
                //    : result.ErorrMessage;

                //return RedirectToAction(nameof(Index));
            }

        [HttpGet]
        public async Task<IActionResult> DeleteMember(CancellationToken ct)
        {
            return View(ct);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMember(int id, CancellationToken ct)
        {
            var result = await memberService.DeleteMemberAsync(id, ct);
            TempData[result.IsSuccess ? "Success" : "Failed"] =
                result.IsSuccess
                ? "Member deleted successfully."
                : result.ErorrMessage;
            return RedirectToAction(nameof(Index));
        }
        public IActionResult GetPhoto(string fileName)
        {
            var result = attatchment.GetAsync(fileName, "MemberPictures");

            if (result == null)
                return NotFound();

            return File(result.Value.stream, result.Value.contentType);
        }
    }
}

