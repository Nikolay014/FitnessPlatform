using FitnessPlatform.Services.Core;
using FitnessPlatform.Services.Core.Contracts;
using FitnessPlatform.Web.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessPlatform.Web.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class UserController : BaseController
    {
        private readonly IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService)); ;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AllUsers()
        {
            // Check if the user is an admin



            bool isAdmin = User.IsInRole("Admin");
            var users = await userService.GetAllUsersAsync(isAdmin);
            return View(users);
        }
        [HttpGet]
        [Authorize(Roles ="Admin,Trainer")]

        public async Task<IActionResult> Details(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return BadRequest("User ID cannot be null or empty.");
            }

            
            var userDetails = await userService.GetUserDetailsAsync(Id);
            if (userDetails == null)
            {
                return NotFound("User not found.");
            }

            return View(userDetails);
        }
        [HttpGet]
        public async Task<IActionResult> MakeItTrainer(string Id)
        {

            bool isAdmin = User.IsInRole("Admin");
            CreateTrainerUserVM user =  await userService.GetUserForTrainerAsync(Id, isAdmin);
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> MakeItTrainer(CreateTrainerUserVM user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            bool isAdmin = User.IsInRole("Admin");
            await userService.CreateTrainerAsync(user);
            return RedirectToAction("AllUsers", "User");
        }
    }
}
