using FitnessPlatform.Data.Models;
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
        [Authorize(Roles = "Admin")]

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            CreateGymVM createGymVM = new CreateGymVM();
           return View(createGymVM);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateGymVM createGymVM)
        {
            if (!ModelState.IsValid)
            {
                return View(createGymVM);
            }
            await gymService.CreateGymAsync(createGymVM);

            return RedirectToAction("AllGyms", "Gym");
        }

    }
}
