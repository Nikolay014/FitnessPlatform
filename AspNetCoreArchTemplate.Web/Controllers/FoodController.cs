using FitnessPlatform.Data.Models;
using FitnessPlatform.Services.Core.Contracts;
using FitnessPlatform.Web.ViewModels.Food;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessPlatform.Web.Controllers
{
    [Authorize(Roles ="User")]
    public class FoodController : BaseController
    {
        private readonly IFoodService foodService;
        private readonly IUserService userService;

        public FoodController(IFoodService foodService, IUserService userService)
        {
            this.foodService = foodService ?? throw new ArgumentNullException(nameof(foodService)); ; ;
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService)); ; ;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View(new FoodLogVM
            {
                Meals = new List<FoodVM> { new FoodVM() }
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add(FoodLogVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // връщаш същото view с грешки
            }

            string userId = GetUserId();

            await foodService.AddMealsAsync(userId, model); // твоя сървис

            
            return RedirectToAction("MyLog", "DailyLog");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            string currUserId = GetUserId();
            await foodService.DeleteMealAsync(id,currUserId);
            return RedirectToAction("MyLog", "DailyLog");
        }
    }
}
