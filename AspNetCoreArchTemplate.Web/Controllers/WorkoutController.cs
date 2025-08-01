using FitnessPlatform.Services.Core.Contracts;
using FitnessPlatform.Web.ViewModels.Workout;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessPlatform.Web.Controllers
{
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

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await workoutService.AddWorkoutSessionAsync(userId, model);

            return RedirectToAction("MyLog", "DailyLog");
        }
    }
}
