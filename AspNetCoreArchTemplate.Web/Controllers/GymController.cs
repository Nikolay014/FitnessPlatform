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

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return BadRequest("Gym ID cannot be null or empty.");
            }
            

            string? userId = GetUserId(); 
            bool isAdmin = User.IsInRole("Admin");

            var gymDetails = await gymService.GetGymDetailsAsync(id, userId, isAdmin);

            if (gymDetails == null)
            {
                return NotFound();
            }
            return View(gymDetails);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            string? userId = GetUserId();
            bool isAdmin = User.IsInRole("Admin");
            DeleteGymVM deleteGymVM = await gymService.GetGymForDeleteAsync(id, userId,isAdmin);
            if (deleteGymVM == null)
            {
                return RedirectToAction("AllGyms", "Gym");
            }
            return View(deleteGymVM);
        }
        [Authorize(Roles = "Admin")]
        public async  Task<IActionResult> ConfirmDelete(int id)
        {
            string? userId = GetUserId();
            bool isAdmin = User.IsInRole("Admin");

            if (!isAdmin  || userId == null)
            {
                return Unauthorized();
            }
            await gymService.DeleteGymAsync(id);
            return RedirectToAction("AllGyms", "Gym");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id)
        {
            EditGymVM editGymVM = await gymService.GetRecipeForEditAsync(id);
            if (editGymVM == null)
            {
                return NotFound();
            }
            return View(editGymVM);
        }



    }
}
