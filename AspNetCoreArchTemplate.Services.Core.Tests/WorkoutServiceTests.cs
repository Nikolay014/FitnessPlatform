using AspNetCoreArchTemplate.Data;
using FitnessPlatform.Data.Models;
using FitnessPlatform.Services.Core.Contracts;
using FitnessPlatform.Web.ViewModels.Workout;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Services.Core.Tests
{
    public class WorkoutServiceTests
    {
        private FitnessDbContext context;
        private IWorkoutService workoutService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<FitnessDbContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString()) // <- променливо име!
        .Options;

            this.context = new FitnessDbContext(options);
            this.workoutService = new WorkoutService(this.context);
        }

        [TearDown]
        public void TearDown()
        {
            this.context.Dispose();
        }
        [Test]
        public async Task AddWorkoutSessionAsync_ShouldCreateLogAndAddWorkout_WhenNoLogExists()
        {
            // Arrange
            var userId = "user1";
            var sessionModel = new WorkoutSessionVM
            {
                Entries = new List<WorkoutEntryVM>
                {
                    new WorkoutEntryVM { Exercise = "Squat", Repetitions = 10, Sets = 3, WeightKg = 80 },
                    new WorkoutEntryVM { Exercise = "Run", DistanceKm = 2 }
                }
            };

            // Act
            await workoutService.AddWorkoutSessionAsync(userId, sessionModel);

            // Assert
            var log = await context.DailyLog
                .Include(d => d.WorkoutSessions)
                .ThenInclude(ws => ws.Entries)
                .FirstOrDefaultAsync(d => d.UserId == userId && d.Date.Date == DateTime.UtcNow.Date);

            Assert.That(log, Is.Not.Null);
            Assert.That(log.WorkoutSessions.Count, Is.EqualTo(1));
            Assert.That(log.WorkoutSessions.First().Entries.Count, Is.EqualTo(2));
            Assert.That(log.WorkoutSessions.First().Entries.Any(e => e.Exercise == "Squat"));
            Assert.That(log.WorkoutSessions.First().Entries.Any(e => e.Exercise == "Run"));
        }

        [Test]
        public async Task AddWorkoutSessionAsync_ShouldAddWorkout_WhenLogAlreadyExists()
        {
            // Arrange
            var userId = "user2";
            var existingLog = new DailyLog { UserId = userId, Date = DateTime.UtcNow };
            context.DailyLog.Add(existingLog);
            await context.SaveChangesAsync();

            var sessionModel = new WorkoutSessionVM
            {
                Entries = new List<WorkoutEntryVM>
                {
                    new WorkoutEntryVM { Exercise = "Push-Up", Repetitions = 20, Sets = 2 }
                }
            };

            // Act
            await workoutService.AddWorkoutSessionAsync(userId, sessionModel);

            // Assert
            var log = await context.DailyLog
                .Include(d => d.WorkoutSessions)
                .ThenInclude(ws => ws.Entries)
                .FirstOrDefaultAsync(d => d.UserId == userId && d.Date.Date == DateTime.UtcNow.Date);

            Assert.That(log.WorkoutSessions.Count, Is.EqualTo(1));
            Assert.That(log.WorkoutSessions.First().Entries.First().Exercise, Is.EqualTo("Push-Up"));
        }

        [Test]
        public async Task AddWorkoutSessionAsync_ShouldAddSession_WithEmptyEntries()
        {
            // Arrange
            var userId = "user3";
            var sessionModel = new WorkoutSessionVM
            {
                Entries = new List<WorkoutEntryVM>() // празен списък
            };

            // Act
            await workoutService.AddWorkoutSessionAsync(userId, sessionModel);

            // Assert
            var log = await context.DailyLog
                .Include(d => d.WorkoutSessions)
                .ThenInclude(ws => ws.Entries)
                .FirstOrDefaultAsync(d => d.UserId == userId && d.Date.Date == DateTime.UtcNow.Date);

            Assert.That(log, Is.Not.Null);
            Assert.That(log.WorkoutSessions.Count, Is.EqualTo(1));
            Assert.That(log.WorkoutSessions.First().Entries.Count, Is.EqualTo(0));
        }

        [Test]
        public void AddWorkoutSessionAsync_ShouldThrowException_WhenModelIsNull()
        {
            // Arrange
            var userId = "user4";

            // Act & Assert
            Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await workoutService.AddWorkoutSessionAsync(userId, null);
            });
        }
        [Test]
        public async Task DeleteWorkoutEntryAsync_ShouldDeleteEntry_WhenUserOwnsIt()
        {
            // Arrange
            var userId = "user1";
            var log = new DailyLog { UserId = userId, Date = DateTime.UtcNow };
            var session = new WorkoutSession { DailyLog = log };
            var entry = new WorkoutEntry { Exercise = "Squat", Repetitions = 10, Sets = 3, WorkoutSession = session };

            context.WorkoutEntries.Add(entry);
            await context.SaveChangesAsync();

            // Act
            await workoutService.DeleteWorkoutEntryAsync(entry.Id, userId);

            // Assert
            var deletedEntry = await context.WorkoutEntries.FindAsync(entry.Id);
            Assert.That(deletedEntry, Is.Null);
        }

        [Test]
        public async Task DeleteWorkoutEntryAsync_ShouldDeleteSession_IfNoEntriesLeft()
        {
            // Arrange
            var userId = "user2";
            var log = new DailyLog { UserId = userId, Date = DateTime.UtcNow };
            var session = new WorkoutSession { DailyLog = log, Entries = new List<WorkoutEntry>() };
            var entry = new WorkoutEntry { Exercise = "Run", DistanceKm = 5, WorkoutSession = session };

            context.WorkoutEntries.Add(entry);
            await context.SaveChangesAsync();

            // Act
            await workoutService.DeleteWorkoutEntryAsync(entry.Id, userId);

            // Assert
            var deletedSession = await context.WorkoutSessions.FindAsync(session.Id);
            Assert.That(deletedSession, Is.Null);
        }

        [Test]
        public async Task DeleteWorkoutEntryAsync_ShouldNotDelete_WhenUserIsNotOwner()
        {
            // Arrange
            var log = new DailyLog { UserId = "realUser", Date = DateTime.UtcNow };
            var session = new WorkoutSession { DailyLog = log };
            var entry = new WorkoutEntry { Exercise = "Push-up", WorkoutSession = session };

            context.WorkoutEntries.Add(entry);
            await context.SaveChangesAsync();

            // Act
            await workoutService.DeleteWorkoutEntryAsync(entry.Id, "fakeUser");

            // Assert
            var stillExists = await context.WorkoutEntries.FindAsync(entry.Id);
            Assert.That(stillExists, Is.Not.Null);
        }

        [Test]
        public async Task DeleteWorkoutEntryAsync_ShouldNotFail_WhenEntryDoesNotExist()
        {
            // Arrange
            var userId = "user4";

            // Act & Assert
            Assert.DoesNotThrowAsync(async () =>
            {
                await workoutService.DeleteWorkoutEntryAsync(999, userId);
            });
        }

    }
}
