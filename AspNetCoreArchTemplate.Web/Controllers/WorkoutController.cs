using FitnessPlatform.Services.Core.Contracts;
using FitnessPlatform.Web.ViewModels.Workout;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessPlatform.Web.Controllers
{
    [Authorize(Roles ="User")]
    public class WorkoutController : BaseController
    {
        private readonly IWorkoutService workoutService;
        private readonly IUserService userService;

        public WorkoutController(IWorkoutService workoutService, IUserService userService)
        {
            this.workoutService = workoutService ?? throw new ArgumentNullException(nameof(workoutService)); ; ;
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService)); ; ;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Add()
        {
            var model = new WorkoutSessionVM();

            // Поне 1 упражнение по подразбиране
            model.Entries.Add(new WorkoutEntryVM());

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(WorkoutSessionVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string userId = GetUserId();
            await workoutService.AddWorkoutSessionAsync(userId, model);

            return RedirectToAction("MyLog", "DailyLog");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEntry(int id)
        {
            string currUserId = GetUserId();    
            await workoutService.DeleteWorkoutEntryAsync(id,currUserId);
            return RedirectToAction("MyLog", "DailyLog");
        }
    }
}
