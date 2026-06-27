using GymSystemApp.BLL.Services.Contracts;
using GymSystemApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GymSystemApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IAnalyticsService analyticsService;

        public HomeController(IAnalyticsService analyticsService)
        {
            this.analyticsService = analyticsService;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var Data = await analyticsService.GetAnalyticsDataAsync(ct);
            return View(Data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
