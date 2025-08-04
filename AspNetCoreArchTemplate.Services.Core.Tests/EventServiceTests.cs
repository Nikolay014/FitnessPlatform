using AspNetCoreArchTemplate.Data;
using FitnessPlatform.Data.Models;
using FitnessPlatform.Services.Core.Contracts;
using FitnessPlatform.Web.ViewModels.Event;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Services.Core.Tests
{
    public class EventServiceTests
    {
        private FitnessDbContext context;
        private IEventService eventService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<FitnessDbContext>()
                .UseInMemoryDatabase("TestDb_" + Guid.NewGuid().ToString())
                .Options;

            this.context = new FitnessDbContext(options);
            this.eventService = new EventService(this.context);
        }

        [TearDown]
        public void TearDown()
        {
            this.context.Dispose();
        }

        private FitnessDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<FitnessDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new FitnessDbContext(options);
        }

        [Test]
        public async Task CreateEventAsync_ShouldAddEventToDatabase_WhenValidModel()
        {
            // Arrange
            var context = GetInMemoryDbContext();

            var trainer = new Trainer
            {
                Id = 1,
                UserId = "user1",
                GymId = 1,
                SpecialtyId = 1,
                TrainerImage = "img.jpg"

            };

            var gym = new Gym
            {
                Id = 1,
                Name = "Test Gym",
                Description = "desc",
                Location = "loc"
            };

            context.Gym.Add(gym);
            context.Trainers.Add(trainer);
            await context.SaveChangesAsync();

            var service = new EventService(context);

            var model = new CreateEventVM
            {
                Title = "Test Event",
                Image = "img.png",
                GymId = 1,
                TrainerId = 1,
                Description = "Training",
                StartDate = "05/08/2025",
                StartTime = "18:00",
                EndDate = "05/08/2025",
                EndTime = "20:00"
            };

            // Act
            await service.CreateEventAsync(model);

            // Assert
            var createdEvent = context.Events.FirstOrDefault();
            Assert.NotNull(createdEvent);
            Assert.AreEqual("Test Event", createdEvent.Title);
            Assert.AreEqual("img.png", createdEvent.Image);
            Assert.AreEqual(1, createdEvent.GymId);
            Assert.AreEqual(1, createdEvent.TrainerId);
            Assert.AreEqual("Training", createdEvent.Description);
            Assert.AreEqual(DateTime.Parse("05/08/2025 18:00"), createdEvent.StartDate);
            Assert.AreEqual(DateTime.Parse("05/08/2025 20:00"), createdEvent.EndDate);
        }
        [Test]
        public async Task GetEventAsync_ShouldReturnPaginatedEvents_WhenEventsExist()
        {
            // Arrange
            var context = GetInMemoryDbContext();

            var user = new ApplicationUser
            {
                Id = "user1",
                FirstName = "Anna",
                LastName = "Smith",
                 Gender = "Female", 
                PhoneNumber = "1234567890"
            };

            var gym = new Gym
            {
                Id = 1,
                Name = "Pulse",
                Description = "Desc",
                Location = "Loc"
            };

            var trainer = new Trainer
            {
                Id = 1,
                UserId = "user1",
                User = user,
                GymId = gym.Id,
                SpecialtyId = 1,
                TrainerImage = "img.png"
            };

            var futureEvent = new Event
            {
                Id = 1,
                Title = "HIIT Training",
                Image = "hiit.jpg",
                Gym = gym,
                GymId = gym.Id,
                Trainer = trainer,
                TrainerId = trainer.Id,
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(1).AddHours(1),
                Description = "Fast paced training"
            };

            context.Users.Add(user);
            context.Gym.Add(gym);
            context.Trainers.Add(trainer);
            context.Events.Add(futureEvent);
            await context.SaveChangesAsync();

            var service = new EventService(context);

            // Act
            var result = await service.GetEventAsync(null, 1, "user1");

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Events.Count(), Is.EqualTo(1));

            Assert.AreEqual("HIIT Training", result.Events.First().Title);
            Assert.AreEqual("Pulse", result.Events.First().Gym);
            Assert.AreEqual("Anna Smith", result.Events.First().Trainer);
            Assert.AreEqual(1, result.CurrentPage);
            Assert.AreEqual(1, result.TotalPages);
        }
        


        [Test]
        public void GetEventDetailsAsync_ShouldThrow_WhenEventDoesNotExist()
        {
            var context = GetInMemoryDbContext();
            var service = new EventService(context);

            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await service.GetEventDetailsAsync(999, "user1", false);
            });

            Assert.AreEqual("Gym not found.", ex.Message);
        }

        [Test]
        public async Task EditEventAsync_ShouldUpdateEvent_WhenModelIsValid()
        {
            // Arrange
            var context = GetInMemoryDbContext();

            var user = new ApplicationUser
            {
                Id = "user1",
                FirstName = "Anna",
                LastName = "Smith",
                Gender = "Female",
                PhoneNumber = "0888123456"
            };

            var trainer = new Trainer
            {
                Id = 1,
                UserId = user.Id,
                User = user,
                TrainerImage = "trainer.jpg"
            };

            var gym = new Gym
            {
                Id = 1,
                Name = "Pulse",
                Location = "Room 2",
                Description = "Relax gym"
            };

            var @event = new Event
            {
                Id = 1,
                Title = "Old Title",
                Image = "old.jpg",
                Description = "Old description",
                Gym = gym,
                GymId = gym.Id,
                Trainer = trainer,
                TrainerId = trainer.Id,
                StartDate = new DateTime(2025, 10, 1, 8, 0, 0),
                EndDate = new DateTime(2025, 10, 1, 9, 0, 0)
            };

            context.Users.Add(user);
            context.Trainers.Add(trainer);
            context.Gym.Add(gym);
            context.Events.Add(@event);
            await context.SaveChangesAsync();

            var service = new EventService(context);

            var model = new EditEventVM
            {
                Id = 1,
                Title = "New Title",
                Image = "new.jpg",
                Description = "New description",
                GymId = 1,
                TrainerId = 1,
                StartDate = "2025-10-02",
                StartTime = "10:00",
                EndDate = "2025-10-02",
                EndTime = "11:00"
            };

            // Act
            await service.EditEventAsync(model);

            // Assert
            var updatedEvent = await context.Events.FirstOrDefaultAsync(e => e.Id == 1);
            Assert.NotNull(updatedEvent);
            Assert.AreEqual("New Title", updatedEvent.Title);
            Assert.AreEqual("new.jpg", updatedEvent.Image);
            Assert.AreEqual("New description", updatedEvent.Description);
            Assert.AreEqual(new DateTime(2025, 10, 2, 10, 0, 0), updatedEvent.StartDate);
            Assert.AreEqual(new DateTime(2025, 10, 2, 11, 0, 0), updatedEvent.EndDate);
        }

        [Test]
        public void EditEventAsync_ShouldThrowArgumentNullException_WhenModelIsNull()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new EventService(context);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await service.EditEventAsync(null);
            });

            Assert.That(ex.Message, Does.Contain("Event model cannot be null"));
        }

        [Test]
        public async Task GetEventForDeleteAsync_ShouldReturnVM_WhenEventExistsAndUserIsAdmin()
        {
            // Arrange
            var context = GetInMemoryDbContext();

            var @event = new Event
            {
                Id = 1,
                Title = "Event Title",
                Description = "Desc",
                Image = "event.jpg",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddHours(1),
                Gym = new Gym { Id = 1, Name = "Test Gym", Location = "Hall A", Description = "Nice" },
                GymId = 1,
                Trainer = new Trainer
                {
                    Id = 1,
                    UserId = "user1",
                    User = new ApplicationUser
                    {
                        Id = "user1",
                        FirstName = "Anna",
                        LastName = "Smith",
                        Gender = "Female",
                        PhoneNumber = "0888123456"
                    },
                    TrainerImage = "image.jpg"
                },
                TrainerId = 1
            };

            context.Events.Add(@event);
            await context.SaveChangesAsync();

            var service = new EventService(context);

            // Act
            var result = await service.GetEventForDeleteAsync(1, "user1", isAdmin: true);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Event Title", result.Title);
            Assert.IsTrue(result.IsAdmin);
        }

        [Test]
        public async Task GetEventForDeleteAsync_ShouldReturnNull_WhenEventDoesNotExist()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new EventService(context);

            // Act
            var result = await service.GetEventForDeleteAsync(999, "user1", isAdmin: true);

            // Assert
            Assert.Null(result);
        }

        [Test]
        public async Task GetEventForDeleteAsync_ShouldReturnNull_WhenUserIsNotAdmin()
        {
            // Arrange
            var context = GetInMemoryDbContext();

            var @event = new Event
            {
                Id = 1,
                Title = "Private Event",
                Description = "Desc",
                Image = "event.jpg",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddHours(1),
                Gym = new Gym { Id = 1, Name = "Test Gym", Location = "Hall A", Description = "Nice" },
                GymId = 1,
                Trainer = new Trainer
                {
                    Id = 1,
                    UserId = "user1",
                    User = new ApplicationUser
                    {
                        Id = "user1",
                        FirstName = "Anna",
                        LastName = "Smith",
                        Gender = "Female",
                        PhoneNumber = "0888123456"
                    },
                    TrainerImage = "image.jpg"
                },
                TrainerId = 1
            };

            context.Events.Add(@event);
            await context.SaveChangesAsync();

            var service = new EventService(context);

            // Act
            var result = await service.GetEventForDeleteAsync(1, "user1", isAdmin: false);

            // Assert
            Assert.Null(result);
        }

        [Test]
        public async Task DeleteEventAsync_ShouldRemoveEvent_WhenEventExists()
        {
            // Arrange
            var context = GetInMemoryDbContext();

            var @event = new Event
            {
                Id = 1,
                Title = "Yoga",
                Description = "Relax",
                Image = "img.jpg",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddHours(1),
                Gym = new Gym
                {
                    Id = 1,
                    Name = "Pulse",
                    Location = "Hall 1",
                    Description = "Test"
                },
                GymId = 1,
                Trainer = new Trainer
                {
                    Id = 1,
                    UserId = "user1",
                    User = new ApplicationUser
                    {
                        Id = "user1",
                        FirstName = "Anna",
                        LastName = "Smith",
                        Gender = "Female",
                        PhoneNumber = "0888123456"
                    },
                    TrainerImage = "trainer.jpg"
                },
                TrainerId = 1
            };

            context.Events.Add(@event);
            await context.SaveChangesAsync();

            var service = new EventService(context);

            // Act
            await service.DeleteEventAsync(1);

            // Assert
            var deleted = await context.Events.FindAsync(1);
            Assert.Null(deleted);
        }

        [Test]
        public async Task DeleteEventAsync_ShouldDoNothing_WhenEventDoesNotExist()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new EventService(context);

            // Act
            await service.DeleteEventAsync(999); // Несъществуващ ID

            // Assert
            Assert.AreEqual(0, context.Events.Count());
        }

        [Test]
        public async Task SubscribeEventAsync_ShouldAddEventRegistration_WhenCalled()
        {
            // Arrange
            var context = GetInMemoryDbContext();

            var user = new ApplicationUser
            {
                Id = "user1",
                FirstName = "Anna",
                LastName = "Smith",
                Gender = "Female",
                PhoneNumber = "0888123456"
            };

            var gym = new Gym
            {
                Id = 1,
                Name = "Pulse",
                Location = "Room 2",
                Description = "Relaxation"
            };

            var trainer = new Trainer
            {
                Id = 1,
                UserId = "user1",
                User = user,
                TrainerImage = "trainer.jpg"
            };

            var @event = new Event
            {
                Id = 1,
                Title = "Morning Yoga",
                Description = "Relaxing session",
                Image = "yoga.jpg",
                Gym = gym,
                GymId = gym.Id,
                Trainer = trainer,
                TrainerId = trainer.Id,
                StartDate = new DateTime(2025, 10, 1, 8, 0, 0),
                EndDate = new DateTime(2025, 10, 1, 9, 0, 0)
            };

            context.Users.Add(user);
            context.Gym.Add(gym);
            context.Trainers.Add(trainer);
            context.Events.Add(@event);
            await context.SaveChangesAsync();

            var service = new EventService(context);

            // Act
            await service.SubscribeEventAsync(1, "user1");

            // Assert
            var registration = await context.EventRegistrations
                .FirstOrDefaultAsync(r => r.EventId == 1 && r.UserId == "user1");

            Assert.NotNull(registration);
            Assert.AreEqual(1, registration.EventId);
            Assert.AreEqual("user1", registration.UserId);
        }

        

        [Test]
        public void RemoveSubscriptionAsync_ShouldThrow_WhenRegistrationNotFound()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new EventService(context);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentNullException>(() => service.RemoveSubscriptionAsync(1, "nonexistent-user"));
            Assert.That(ex.Message, Does.Contain("Invalid event"));
        }














    }
}
