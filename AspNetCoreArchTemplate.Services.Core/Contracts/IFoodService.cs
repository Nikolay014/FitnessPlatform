using FitnessPlatform.Web.ViewModels.Food;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Services.Core.Contracts
{
    public interface IFoodService
    {

        Task AddMealsAsync(string userId, FoodLogVM model);
        Task DeleteMealAsync(int id,string currentUserId);
    }
}
