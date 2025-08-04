using AspNetCoreArchTemplate.Data;
using FitnessPlatform.Data.Models;
using FitnessPlatform.Services.Core.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Services.Core.Tests
{
    public class DailyLogServiceTests
    {
        private FitnessDbContext context;
        private IDailyLogService dailyLogService;
        private Mock<UserManager<ApplicationUser>> userManagerMock;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<FitnessDbContext>()
        .UseInMemoryDatabase("TestDb_" + Guid.NewGuid())
        .Options;

            this.context = new FitnessDbContext(options);

            // Създаваме mock за UserManager<ApplicationUser>
            var store = new Mock<IUserStore<ApplicationUser>>();
            userManagerMock = new Mock<UserManager<ApplicationUser>>(
                store.Object, null, null, null, null, null, null, null, null
            );

            // Подаваме context и mock в конструктора
            this.dailyLogService = new DailyLogService(this.context, userManagerMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            this.context.Dispose();
        }
        [Test]
        public async Task GetUserLogAsync_ShouldCreateLog_WhenNoneExists()
        {
            // Arrange
            var userId = "user123";
            var date = DateTime.UtcNow.Date;

            // Act
            var result = await dailyLogService.GetUserLogAsync(userId, date);

            // Assert
            var logInDb = await context.DailyLog.FirstOrDefaultAsync(l => l.UserId == userId && l.Date.Date == date);
            Assert.That(logInDb, Is.Not.Null);
            Assert.That(result.DailyLogId, Is.EqualTo(logInDb.Id));
            Assert.That(result.Foods, Is.Empty);
            Assert.That(result.Workouts, Is.Empty);
        }
        [Test]
        public async Task GetUserLogAsync_ShouldReturnExistingLogWithData()
        {
            // Arrange
            var userId = "user456";
            var date = DateTime.UtcNow.Date;

            var log = new DailyLog
            {
                UserId = userId,
                Date = date,
                Foods = new List<Food>
        {
            new Food { Image = "img.png", Description = "Banana", Calories = 100 }
        },
                WorkoutSessions = new List<WorkoutSession>
        {
            new WorkoutSession
            {
                Notes = "Leg Day",
                Entries = new List<WorkoutEntry>
                {
                    new WorkoutEntry { Exercise = "Squat", Repetitions = 10, Sets = 3, WeightKg = 80 }
                }
            }
        }
            };

            context.DailyLog.Add(log);
            await context.SaveChangesAsync();

            // Act
            var result = await dailyLogService.GetUserLogAsync(userId, date);

            // Assert
            Assert.That(result.Foods.Count, Is.EqualTo(1));
            Assert.That(result.Foods[0].Description, Is.EqualTo("Banana"));
            Assert.That(result.Workouts.Count, Is.EqualTo(1));
            Assert.That(result.Workouts[0].Entries[0].Exercise, Is.EqualTo("Squat"));
        }




    }
}
