using GymManagementSystem.BLL.ViewModels.AccountsViewModels;
using GymSystemApp.Controllers;
using GymSystemApp.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace GymSystemApp.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model,CancellationToken ct)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await userManager.FindByEmailAsync(model.Email);
            if(user is null || string.IsNullOrEmpty(user.UserName))
            {
                ModelState.AddModelError(string.Empty, "Invalid Email Or Password.");
                return View(model);
            }
            var reslut = await signInManager.PasswordSignInAsync(user.UserName,model.Password,model.RememberMe,true);
            if(reslut.Succeeded)
            {
                return RedirectToAction(nameof(HomeController.Index),"Home");
            }
            if (reslut.IsLockedOut)
                ModelState.AddModelError(string.Empty, "Account LockedOut.");
            if (reslut.IsNotAllowed)
                ModelState.AddModelError(string.Empty, "Un Authrized.");
            else
                ModelState.AddModelError(string.Empty, "Invalid Email Or Password.");

            return View();
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login),"Account");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
