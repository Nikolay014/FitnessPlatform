using AspNetCoreArchTemplate.Data;
using FitnessPlatform.Data.Models;
using FitnessPlatform.Services.Core.Contracts;
using FitnessPlatform.Web.ViewModels.Trainer;
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
    public class TrainerServiceTests
    {
        private FitnessDbContext context;
        private Mock<UserManager<ApplicationUser>> userManagerMock;
        private ITrainerService trainerService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<FitnessDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            this.context = new FitnessDbContext(options);

            // Създай Mock за UserManager
            var store = new Mock<IUserStore<ApplicationUser>>();
            this.userManagerMock = new Mock<UserManager<ApplicationUser>>(
                store.Object, null, null, null, null, null, null, null, null
            );

            this.trainerService = new TrainerService(this.context, this.userManagerMock.Object);
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
        private Mock<UserManager<ApplicationUser>> MockUserManager(List<ApplicationUser> users)
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            var mock = new Mock<UserManager<ApplicationUser>>(
                store.Object, null, null, null, null, null, null, null, null);

            mock.Setup(m => m.Users).Returns(users.AsQueryable());
            mock.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((string id) => users.FirstOrDefault(u => u.Id == id));

            return mock;
        }

        [Test]
        public async Task GetAllTrainersAsync_ShouldReturnEmpty_WhenNoTrainersMatchFilters()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var userManagerMock = MockUserManager(new List<ApplicationUser>());
            var service = new TrainerService(context, userManagerMock.Object);

            // Act
            var result = await service.GetAllTrainersAsync(99, 99, 1, false);

            // Assert
            Assert.That(result.Trainers, Is.Empty);
            Assert.That(result.TotalPages, Is.EqualTo(0));
        }

        [Test]
        public async Task GetAllTrainersAsync_ShouldFilterByGymId()
        {
            // Arrange
            var context = GetInMemoryDbContext();

            var gym1 = new Gym { Id = 1, Name = "Gym1", Description = "d", Location = "l" };
            var gym2 = new Gym { Id = 2, Name = "Gym2", Description = "d", Location = "l" };
            var specialty = new Specialty { Id = 1, Name = "Strength" };

            context.Gym.AddRange(gym1, gym2);
            context.Specialties.Add(specialty);

            var user1 = new ApplicationUser { Id = "u1", FirstName = "A", LastName = "B", PhoneNumber = "1", Gender = "Male" };
            var user2 = new ApplicationUser { Id = "u2", FirstName = "C", LastName = "D", PhoneNumber = "2", Gender = "Male" };

            context.Users.AddRange(user1, user2);

            context.Trainers.AddRange(
                new Trainer { Id = 1, UserId = "u1", GymId = 1, SpecialtyId = 1, TrainerImage = "img1.jpg" },
                new Trainer { Id = 2, UserId = "u2", GymId = 2, SpecialtyId = 1, TrainerImage = "img2.jpg" }
            );

            await context.SaveChangesAsync();

            var userManagerMock = MockUserManager(new List<ApplicationUser> { user1, user2 });
            var service = new TrainerService(context, userManagerMock.Object);

            // Act
            var result = await service.GetAllTrainersAsync(1, null, 1, false);

            // Assert
            Assert.That(result.Trainers.Count, Is.EqualTo(1));
            Assert.That(result.Trainers.First().Gym, Is.EqualTo("Gym1"));
        }
        [Test]
        public async Task GetTrainerDetailsAsync_ShouldReturnDetails_WhenTrainerExists()
        {
            // Arrange
            var context = GetInMemoryDbContext();

            var user = new ApplicationUser
            {
                Id = "user1",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "123456789",
                Gender = "Male"
            };

            var gym = new Gym { Id = 1, Name = "PowerGym", Description = "desc", Location = "loc" };

            var specialty = new Specialty { Id = 1, Name = "Strength", Description = "Strength Desc" };

            var trainer = new Trainer
            {
                Id = 1,
                UserId = user.Id,
                GymId = gym.Id,
                SpecialtyId = specialty.Id,
                TrainerImage = "image.jpg",
                Clients = new List<TrainerClient>
        {
            new TrainerClient { ClientId = "user1" }
        }
            };

            context.Users.Add(user);
            context.Gym.Add(gym);
            context.Specialties.Add(specialty);
            context.Trainers.Add(trainer);

            await context.SaveChangesAsync();

            var service = new TrainerService(context, MockUserManager(new List<ApplicationUser> { user }).Object);

            // Act
            var result = await service.GetTrainerDetailsAsync(1, "user1", false);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.TrainerId, Is.EqualTo(1));
            Assert.That(result.Gym, Is.EqualTo("PowerGym"));
            Assert.That(result.FullName, Is.EqualTo("John Doe"));
            Assert.That(result.Image, Is.EqualTo("image.jpg"));
            Assert.That(result.Specialty, Is.EqualTo("Strength"));
            Assert.That(result.SpecialtyDescription, Is.EqualTo("Strength Desc"));
            Assert.That(result.PhoneNumber, Is.EqualTo("123456789"));
            Assert.That(result.IsUserSubscribe, Is.True);
        }
        [Test]
        public async Task RemoveTrainer_ShouldRemoveTrainerAndUpdateRoles_WhenTrainerExists()
        {
            // Arrange
            var user = new ApplicationUser
            {
                Id = "u1",
                UserName = "trainer@example.com",
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                PhoneNumber = "123456789"
            };

            var trainer = new Trainer
            {
                Id = 1,
                UserId = user.Id,
                User = user,
                GymId = 1,
                SpecialtyId = 1,
                TrainerImage = "img.jpg"
            };

            var context = GetInMemoryDbContext();

            context.Users.Add(user);
            context.Trainers.Add(trainer);
            context.Gym.Add(new Gym { Id = 1, Name = "Gym1", Description = "Desc", Location = "Loc" });
            context.Specialties.Add(new Specialty { Id = 1, Name = "Strength", Description = "Desc" });

            await context.SaveChangesAsync();

            var userManagerMock = MockUserManager(new List<ApplicationUser> { user });
            userManagerMock.Setup(um => um.GetRolesAsync(user)).ReturnsAsync(new List<string> { "Trainer" });
            userManagerMock.Setup(um => um.RemoveFromRolesAsync(user, It.IsAny<IEnumerable<string>>())).ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(um => um.AddToRoleAsync(user, "User")).ReturnsAsync(IdentityResult.Success);

            var service = new TrainerService(context, userManagerMock.Object);

            // Act
            await service.RemoveTrainer(1, true);

            // Assert
            Assert.That(context.Trainers.Any(t => t.Id == 1), Is.False);
            userManagerMock.Verify(um => um.GetRolesAsync(user), Times.Once);
            userManagerMock.Verify(um => um.RemoveFromRolesAsync(user, It.IsAny<IEnumerable<string>>()), Times.Once);
            userManagerMock.Verify(um => um.AddToRoleAsync(user, "User"), Times.Once);
        }

        [Test]
        public async Task UnUserSubscribeToTrainer_ShouldRemoveSubscription_WhenExists()
        {
            // Arrange
            var context = GetInMemoryDbContext();

            var trainer = new Trainer
            {
                Id = 1,
                GymId = 1,
                SpecialtyId = 1,
                TrainerImage = "img.jpg",
                UserId = "trainer1"
            };

            var user = new ApplicationUser
            {
                Id = "user1",
                FirstName = "Ivan",
                LastName = "Ivanov",
                PhoneNumber = "111111111",
                Gender = "Male"
            };

            var gym = new Gym { Id = 1, Name = "TestGym", Description = "d", Location = "loc" };
            var specialty = new Specialty { Id = 1, Name = "Strength", Description = "desc" };

            var subscription = new TrainerClient
            {
                TrainerId = trainer.Id,
                ClientId = user.Id
            };

            context.Users.Add(user);
            context.Gym.Add(gym);
            context.Specialties.Add(specialty);
            context.Trainers.Add(trainer);
            context.TrainerClients.Add(subscription);

            await context.SaveChangesAsync();

            var service = new TrainerService(context, MockUserManager(new List<ApplicationUser> { user }).Object);

            // Act
            await service.UnUserSubscribeToTrainer(trainer.Id, user.Id);

            // Assert
            var result = context.TrainerClients.FirstOrDefault(tc => tc.TrainerId == trainer.Id && tc.ClientId == user.Id);
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task UserSubscribeToTrainer_ShouldCreateTrainerClient_WhenValidData()
        {
            // Arrange
            var context = GetInMemoryDbContext();

            var user = new ApplicationUser
            {
                Id = "user1",
                FirstName = "Ivan",
                LastName = "Ivanov",
                PhoneNumber = "123456789",
                Gender = "Male"
            };

            var trainerUser = new ApplicationUser
            {
                Id = "trainer1",
                FirstName = "Pesho",
                LastName = "Petrov",
                PhoneNumber = "987654321",
                Gender = "Male"
            };

            var gym = new Gym { Id = 1, Name = "PowerGym", Description = "desc", Location = "loc" };
            var specialty = new Specialty { Id = 1, Name = "Strength", Description = "desc" };

            var trainer = new Trainer
            {
                Id = 1,
                UserId = trainerUser.Id,
                GymId = gym.Id,
                SpecialtyId = specialty.Id,
                TrainerImage = "image.jpg"
            };

            context.Users.AddRange(user, trainerUser);
            context.Gym.Add(gym);
            context.Specialties.Add(specialty);
            context.Trainers.Add(trainer);
            await context.SaveChangesAsync();

            var service = new TrainerService(context, MockUserManager(new List<ApplicationUser> { user, trainerUser }).Object);

            // Act
            await service.UserSubscribeToTrainer(trainer.Id, user.Id);

            // Assert
            var sub = context.TrainerClients.FirstOrDefault(tc => tc.TrainerId == trainer.Id && tc.ClientId == user.Id);
            Assert.That(sub, Is.Not.Null);
        }


        [Test]
        public async Task UserSubscribeToTrainer_ShouldThrow_WhenUserNotFound()
        {
            // Arrange
            var context = GetInMemoryDbContext();

            var trainerUser = new ApplicationUser
            {
                Id = "trainer1",
                FirstName = "Trainer",
                LastName = "Last",
                PhoneNumber = "000",
                Gender = "Male"
            };

            var gym = new Gym { Id = 1, Name = "Gym", Description = "desc", Location = "loc" };
            var specialty = new Specialty { Id = 1, Name = "Strength", Description = "desc" };

            var trainer = new Trainer
            {
                Id = 1,
                UserId = trainerUser.Id,
                GymId = gym.Id,
                SpecialtyId = specialty.Id,
                TrainerImage = "img.jpg"
            };

            context.Users.Add(trainerUser);
            context.Gym.Add(gym);
            context.Specialties.Add(specialty);
            context.Trainers.Add(trainer);
            await context.SaveChangesAsync();

            var service = new TrainerService(context, MockUserManager(new List<ApplicationUser> { trainerUser }).Object);

            // Act + Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await service.UserSubscribeToTrainer(trainer.Id, "missingUser");
            });

            Assert.That(ex.Message, Is.EqualTo("User not found."));
        }



        [Test]
        public void GetTrainerForUpdate_ShouldThrow_WhenTrainerNotFound()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new TrainerService(context, MockUserManager(new List<ApplicationUser>()).Object);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await service.GetTrainerForUpdate(99, true);
            });

            Assert.That(ex.Message, Is.EqualTo("Invalid trainer."));
        }

        [Test]
        public void GetTrainerForUpdate_ShouldThrow_WhenUserIsNotAdmin()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new TrainerService(context, MockUserManager(new List<ApplicationUser>()).Object);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await service.GetTrainerForUpdate(1, false);
            });

            Assert.That(ex.Message, Is.EqualTo("Admin not found."));
        }
        

        [Test]
        public void UpdateTrainerAsync_ShouldThrow_WhenNotAdmin()
        {
            // Arrange
            var context = GetInMemoryDbContext();

            var service = new TrainerService(context, MockUserManager(new List<ApplicationUser>()).Object);

            var editVM = new EditTrainerVM
            {
                TrainerId = 1,
                GymId = 1,
                SpecialtyId = 1,
                Image = "image.jpg"
            };

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await service.UpdateTrainerAsync(editVM, false);
            });

            Assert.That(ex.Message, Is.EqualTo("Admin not found."));
        }
        

        [Test]
        public void GetClientsAsync_ShouldThrow_WhenTrainerNotFound()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new TrainerService(context, MockUserManager(new List<ApplicationUser>()).Object);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await service.GetClientsAsync(999, null);
            });

            Assert.That(ex.Message, Is.EqualTo("Invalid trainer."));
        }

        
        [Test]
        public async Task GetEventsAsync_ShouldReturnEvents_WhenTrainerIdIsZero()
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

            var gym = new Gym { Id = 1, Name = "Test Gym", Description = "desc", Location = "loc" };
            var trainer = new Trainer
            {
                Id = 1,
                UserId = "user1",
                User = user,
                Gym = gym,
                GymId = gym.Id,
                TrainerImage = "image.jpg"
            };

            var ev = new Event
            {
                Id = 1,
                Title = "Evening Workout",
                Image = "evening.jpg",
                GymId = gym.Id,
                Gym = gym,
                TrainerId = trainer.Id,
                Trainer = trainer,
                StartDate = new DateTime(2025, 8, 2, 18, 0, 0),
                EndDate = new DateTime(2025, 8, 2, 20, 0, 0),
            };

            trainer.Events.Add(ev);
            context.Users.Add(user);
            context.Gym.Add(gym);
            context.Trainers.Add(trainer);
            context.Events.Add(ev);
            await context.SaveChangesAsync();

            var service = new TrainerService(context, MockUserManager(new List<ApplicationUser> { user }).Object);

            // Act
            var result = await service.GetEventsAsync(0, "user1");

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("Anna Smith", result.TrainerName);
            Assert.That(result.Events, Has.Exactly(1).Items);
            Assert.AreEqual("Evening Workout", result.Events.First().Title);
        }

        [Test]
        public async Task GetAllGymsForDropdownAsync_ShouldReturnAllGymsAsSelectListItems()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Gym.AddRange(
                new Gym { Id = 1, Name = "Iron Gym", Description = "Desc1", Location = "Loc1" },
                new Gym { Id = 2, Name = "Power Gym", Description = "Desc2", Location = "Loc2" }
            );
            await context.SaveChangesAsync();

            var service = new TrainerService(context, MockUserManager(new List<ApplicationUser>()).Object);

            // Act
            var result = await service.GetAllGymsForDropdownAsync();

            // Assert
            Assert.That(result, Has.Exactly(2).Items);
            Assert.That(result.Any(g => g.Value == "1" && g.Text == "Iron Gym"));
            Assert.That(result.Any(g => g.Value == "2" && g.Text == "Power Gym"));
        }

        [Test]
        public async Task GetAllSpecialtiesForDropdownAsync_ShouldReturnAllSpecialtiesAsSelectListItems()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Specialties.AddRange(
                new Specialty { Id = 1, Name = "Yoga" },
                new Specialty { Id = 2, Name = "Strength Training" }
            );
            await context.SaveChangesAsync();

            var service = new TrainerService(context, MockUserManager(new List<ApplicationUser>()).Object);

            // Act
            var result = await service.GetAllSpecialtiesForDropdownAsync();

            // Assert
            Assert.That(result, Has.Exactly(2).Items);
            Assert.That(result.Any(s => s.Value == "1" && s.Text == "Yoga"));
            Assert.That(result.Any(s => s.Value == "2" && s.Text == "Strength Training"));
        }





















    }
}
