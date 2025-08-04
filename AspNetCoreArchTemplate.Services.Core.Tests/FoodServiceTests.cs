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

namespace FitnessPlatform.Services.Core.Tests
{
    public class FoodServiceTests
    {
        private FitnessDbContext context;
        private IFoodService foodService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<FitnessDbContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString()) // <- променливо име!
        .Options;

            this.context = new FitnessDbContext(options);
            this.foodService = new FoodService(this.context);
        }

        [TearDown]
        public void TearDown()
        {
            this.context.Dispose();
        }
        [Test]
        public async Task AddMealsAsync_ShouldCreateLogAndAddMeals_WhenNoLogExists()
        {
            var options = new DbContextOptionsBuilder<FitnessDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // <--- уникално име
                .Options;

            var context = new FitnessDbContext(options);
            var foodService = new FoodService(context);

            var userId = "user1";
            var foodLog = new FoodLogVM
            {
                Meals = new List<FoodVM>
        {
            new FoodVM { Description = "Apple", Calories = 50, Image = "apple.jpg" },
            new FoodVM { Description = "Bread", Calories = 100, Image = "bread.jpg" }
        }
            };

            // Act
            await foodService.AddMealsAsync(userId, foodLog);

            // Assert
            var log = await context.DailyLog
                .Include(d => d.Foods)
                .FirstOrDefaultAsync(d => d.UserId == userId && d.Date.Date == DateTime.UtcNow.Date);

            Assert.That(log, Is.Not.Null);
            Assert.That(log.Foods.Count, Is.EqualTo(2));
            Assert.That(log.Foods.Any(f => f.Description == "Apple"));
            Assert.That(log.Foods.Any(f => f.Description == "Bread"));
        }

        [Test]
        public async Task AddMealsAsync_ShouldNotAddMeals_WhenMealListIsEmpty()
        {
            // Arrange
            var userId = "user2";
            var foodLog = new FoodLogVM
            {
                Meals = new List<FoodVM>() // празно
            };

            // Act
            await foodService.AddMealsAsync(userId, foodLog);

            // Assert
            var log = await context.DailyLog
                .Include(l => l.Foods)
                .FirstOrDefaultAsync(l => l.UserId == userId && l.Date.Date == DateTime.UtcNow.Date);

            Assert.That(log, Is.Not.Null);
            Assert.That(log.Foods.Count, Is.EqualTo(0));
        }
        [Test]
        public async Task DeleteMealAsync_ShouldDeleteMeal_WhenUserOwnsMeal()
        {
            // Arrange
            var userId = "user1";
            var meal = new Food
            {
                Description = "Banana",
                Calories = 100,
                Image = "banana.jpg",
                DailyLog = new DailyLog
                {
                    UserId = userId,
                    Date = DateTime.UtcNow
                }
            };

            context.Food.Add(meal);
            await context.SaveChangesAsync();

            var foodService = new FoodService(context);

            // Act
            await foodService.DeleteMealAsync(meal.Id, userId);

            // Assert
            var deletedMeal = await context.Food.FindAsync(meal.Id);
            Assert.That(deletedMeal, Is.Null);
        }
        [Test]
        public async Task DeleteMealAsync_ShouldNotDeleteMeal_WhenUserIsNotOwner()
        {
            // Arrange
            var ownerId = "user1";
            var otherUserId = "user2";

            var meal = new Food
            {
                Description = "Pizza",
                Calories = 300,
                Image = "pizza.jpg",
                DailyLog = new DailyLog
                {
                    UserId = ownerId,
                    Date = DateTime.UtcNow
                }
            };

            context.Food.Add(meal);
            await context.SaveChangesAsync();

            var foodService = new FoodService(context);

            // Act
            await foodService.DeleteMealAsync(meal.Id, otherUserId);

            // Assert
            var existingMeal = await context.Food.FindAsync(meal.Id);
            Assert.That(existingMeal, Is.Not.Null);
        }
        [Test]
        public async Task DeleteMealAsync_ShouldNotThrow_WhenMealDoesNotExist()
        {
            // Arrange
            var foodService = new FoodService(context);
            var nonExistentMealId = 999;
            var userId = "user1";

            // Act & Assert
            Assert.DoesNotThrowAsync(async () =>
            {
                await foodService.DeleteMealAsync(nonExistentMealId, userId);
            });
        }








    }
}
