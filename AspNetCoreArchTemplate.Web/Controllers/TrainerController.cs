using FitnessPlatform.Services.Core;
using FitnessPlatform.Services.Core.Contracts;
using FitnessPlatform.Web.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessPlatform.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TrainerController : Controller
    {
        private readonly ITrainerService trainerService;
        public TrainerController(ITrainerService trainerService)
        {
            this.trainerService = trainerService ?? throw new ArgumentNullException(nameof(trainerService)); ;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AllTrainers()
        {
            // Check if the user is an admin



            bool isAdmin = User.IsInRole("Admin");
            var trainers = await trainerService.GetAllTrainersAsync(isAdmin);
            return View(trainers);
        }

        public async Task<IActionResult> Details(int Id)
        {
            

            bool isAdmin = User.IsInRole("Admin");
            var trainerDetails = await trainerService.GetTrainerDetailsAsync(Id, isAdmin);
            if (trainerDetails == null)
            {
                return NotFound("Trainer not found.");
            }

            return View(trainerDetails);
        }
        
        public async Task<IActionResult> RemoveTrainer(int Id)
        {

            bool isAdmin = User.IsInRole("Admin");
            await trainerService.RemoveTrainer(Id, isAdmin);

           return RedirectToAction("AllTrainers", "Trainer");
        }
    }
}
