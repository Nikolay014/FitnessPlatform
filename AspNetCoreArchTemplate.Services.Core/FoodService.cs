using AspNetCoreArchTemplate.Data;
using FitnessPlatform.Data.Models;
using FitnessPlatform.Services.Core.Contracts;
using FitnessPlatform.Web.ViewModels.Food;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Services.Core
{
    public class FoodService:IFoodService
    {
        private readonly FitnessDbContext dbContext;

        public FoodService(FitnessDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext)); ;
        }

        public async Task AddMealsAsync(string userId, FoodLogVM model)
        {
            var dailyLog = await dbContext.DailyLog
                .Include(d => d.Foods)
                .FirstOrDefaultAsync(d => d.UserId == userId && d.Date.Date == DateTime.UtcNow.Date);

            if (dailyLog == null)
            {
                dailyLog = new DailyLog
                {
                    UserId = userId,
                    Date = DateTime.UtcNow,
                    Foods = new List<Food>()
                };

                dbContext.DailyLog.Add(dailyLog);
                await dbContext.SaveChangesAsync();
            }

            foreach (var meal in model.Meals)
            {
                var food = new Food
                {
                    Description = meal.Description,
                    Calories = meal.Calories,
                    Image = meal.Image,
                    DailyLogId = dailyLog.Id
                };

                dbContext.Food.Add(food);
            }

            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteMealAsync(int id, string currentUserId)
        {
            var meal = await dbContext.Food.Include(f=>f.DailyLog).FirstOrDefaultAsync(f=> f.Id == id);
            if (meal != null && meal.DailyLog.UserId == currentUserId)
            {
                dbContext.Food.Remove(meal);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
