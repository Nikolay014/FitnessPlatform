using FitnessPlatform.Services.Core;
using FitnessPlatform.Services.Core.Contracts;
using FitnessPlatform.Web.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace FitnessPlatform.Web.Controllers
{
    
    public class TrainerController : BaseController
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

            string? userId = GetUserId();
            bool isAdmin = User.IsInRole("Admin");
            var trainerDetails = await trainerService.GetTrainerDetailsAsync(Id,userId, isAdmin);
            if (trainerDetails == null)
            {
                return NotFound("Trainer not found.");
            }

            return View(trainerDetails);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveTrainer(int Id)
        {

            bool isAdmin = User.IsInRole("Admin");
            await trainerService.RemoveTrainer(Id, isAdmin);

           return RedirectToAction("AllTrainers", "Trainer");
        }
        [Authorize(Roles = "User")]
        public async Task<IActionResult> SubscribeToTrainer(int id)
        {
            string? userId = GetUserId();
            
            await trainerService.UserSubscribeToTrainer(id, userId);
            return RedirectToAction("AllTrainers","Trainer");
        }
    }
}
