using FitnessPlatform.Services.Core;
using FitnessPlatform.Services.Core.Contracts;
using FitnessPlatform.Web.ViewModels.Trainer;
using FitnessPlatform.Web.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Security.Claims;

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

        public async Task<IActionResult> AllTrainers(int? gymId, int? specialtyId, int page = 1)
        {
            bool isAdmin = User.IsInRole("Admin");
            var trainers = await trainerService.GetAllTrainersAsync(gymId, specialtyId, page, isAdmin);
            var gyms = await trainerService.GetAllGymsForDropdownAsync();
            var specialties = await trainerService.GetAllSpecialtiesForDropdownAsync();

            var viewModel = new PaginatedTrainersVM
            {
                Trainers = trainers.Trainers,
                CurrentPage = trainers.CurrentPage,
                TotalPages = trainers.TotalPages,
                Gyms = gyms,
                Specialties = specialties,
                SelectedGymId = gymId,
                SelectedSpecialtyId = specialtyId
            };

            return View(viewModel);
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
            try
            {
                await trainerService.RemoveTrainer(Id, isAdmin);
                TempData["SuccessMessage"] = "Trainer removed successfully.";
            }
            catch (DbUpdateException)
            {
                TempData["ErrorMessage"] = "Trainer cannot be removed because they are assigned to events.";
            }

            return RedirectToAction("AllTrainers");
        }
        [Authorize(Roles = "User")]
        public async Task<IActionResult> SubscribeToTrainer(int id)
        {
            string? userId = GetUserId();
            
            await trainerService.UserSubscribeToTrainer(id, userId);
            return RedirectToAction("AllTrainers","Trainer");
        }
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UnsubscribeToTrainer(int id)
        {
            string? userId = GetUserId();

            await trainerService.UnUserSubscribeToTrainer(id, userId);
            return RedirectToAction("AllTrainers", "Trainer");
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id)
        {
            bool isAdmin = User.IsInRole("Admin");

            EditTrainerVM editTrainerVM = await trainerService.GetTrainerForUpdate(id, isAdmin);
            if (editTrainerVM == null)
            {
                throw new ArgumentException("Invalid trainer");
            }
            return View(editTrainerVM);

        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(EditTrainerVM editTrainerVM)
        {
            bool isAdmin = User.IsInRole("Admin");
            await trainerService.UpdateTrainerAsync(editTrainerVM,isAdmin);

            return RedirectToAction("AllTrainers", "Trainer");
        }
        [Authorize(Roles = "Admin,Trainer")]
        public async Task<IActionResult> GetTrainerClients(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

           

           
            TrainerClientsVM users = await trainerService.GetClientsAsync(id,userId);
            return View(users);
        }

        [Authorize(Roles = "Admin,Trainer")]
        public async Task<IActionResult> GetTrainerEvents(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);




            TrainerEventsVM events = await trainerService.GetEventsAsync(id, userId);
            return View(events);
        }

    }
}
