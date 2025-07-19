using FitnessPlatform.Services.Core;
using FitnessPlatform.Services.Core.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessPlatform.Web.Controllers
{
    [Authorize(Roles = "Admin")]
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
    }
}
