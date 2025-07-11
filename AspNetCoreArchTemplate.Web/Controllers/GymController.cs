using FitnessPlatform.Services.Core.Contracts;
using FitnessPlatform.Web.ViewModels.Gym;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessPlatform.Web.Controllers
{
    public class GymController : BaseController
    {
        private readonly IGymService gymService;
        public GymController(IGymService gymService)
        {
            this.gymService = gymService ?? throw new ArgumentNullException(nameof(gymService));
        }
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]

        public async Task<IActionResult> AllGyms()
        {
           string? userId = GetUserId();
           IEnumerable<GymVM> gyms = await gymService.GetGymsAsync(userId);
            return View(gyms);
            
        }

    }
}
