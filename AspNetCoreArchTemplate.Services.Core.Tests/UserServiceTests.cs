using AspNetCoreArchTemplate.Data;
using FitnessPlatform.Data.Models;
using FitnessPlatform.Services.Core.Contracts;
using FitnessPlatform.Web.ViewModels.User;
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
    public class UserServiceTests
    {
        private FitnessDbContext context;
        private Mock<UserManager<ApplicationUser>> userManagerMock;
        private IUserService userService;

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

            this.userService = new UserService(this.context, this.userManagerMock.Object);
        }


        [TearDown]
        public void TearDown()
        {
            this.context.Dispose();
        }
        private Mock<UserManager<ApplicationUser>> MockUserManager(List<ApplicationUser> users)
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            var mock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            mock.Setup(u => u.Users).Returns(users.AsQueryable());
            return mock;
        }
        private FitnessDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<FitnessDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new FitnessDbContext(options);
        }


        [Test]
        public async Task GetAllUsersAsync_ShouldReturnPaginatedUsers()
        {
            // Arrange
            var users = new List<ApplicationUser>
    {
        new ApplicationUser { Id = "1", FirstName = "John", LastName = "Doe", Gender = "Male", PhoneNumber = "123", ImageUrl = "img1.jpg" },
        new ApplicationUser { Id = "2", FirstName = "Jane", LastName = "Smith", Gender = "Female", PhoneNumber = "456", ImageUrl = "img2.jpg" },
        new ApplicationUser { Id = "3", FirstName = "Mike", LastName = "Brown", Gender = "Male", PhoneNumber = "789", ImageUrl = "img3.jpg" },
        new ApplicationUser { Id = "4", FirstName = "Anna", LastName = "White", Gender = "Female", PhoneNumber = "000", ImageUrl = "img4.jpg" }
    };

            userManagerMock
                .Setup(um => um.GetUsersInRoleAsync("User"))
                .ReturnsAsync(users);

            // Act
            var result = await userService.GetAllUsersAsync(1, false);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Users.Count, Is.EqualTo(3)); // PageSize = 3
            Assert.That(result.TotalPages, Is.EqualTo(2));
            Assert.That(result.Users.Any(u => u.FullName == "John Doe"));
            Assert.That(result.Users.Any(u => u.FullName == "Jane Smith"));
        }

        [Test]
        public async Task GetUserDetailsAsync_ShouldReturnCorrectDetails_WhenUserExists()
        {
            // Arrange
            var userId = "user123";
            var user = new ApplicationUser
            {
                Id = userId,
                FirstName = "Ivan",
                LastName = "Ivanov",
                Gender = "Male",
                PhoneNumber = "0888123123",
                DateOfBirth = new DateTime(1990, 1, 1),
                HeightCm = 180,
                WeightKg = 80,
                ImageUrl = "ivan.jpg"
            };

            var userManagerMock = MockUserManager(new List<ApplicationUser> { user });
            userManagerMock.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);

            var service = new UserService(context, userManagerMock.Object);

            // Act
            var result = await service.GetUserDetailsAsync(userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.UserId, Is.EqualTo(userId));
            Assert.That(result.FirstName, Is.EqualTo("Ivan"));
            Assert.That(result.LastName, Is.EqualTo("Ivanov"));
            Assert.That(result.Gender, Is.EqualTo("Male"));
            Assert.That(result.PhoneNumber, Is.EqualTo("0888123123"));
            Assert.That(result.DateOfBirth, Is.EqualTo(new DateTime(1990, 1, 1)));
            Assert.That(result.HeightCm, Is.EqualTo(180));
            Assert.That(result.WeightKg, Is.EqualTo(80));
            Assert.That(result.ImageUrl, Is.EqualTo("ivan.jpg"));
        }
        [Test]
        public async Task GetUserDetailsAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = "nonexistent";
            var userManagerMock = MockUserManager(new List<ApplicationUser>());
            userManagerMock.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync((ApplicationUser)null);

            var service = new UserService(context, userManagerMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                var result = await service.GetUserDetailsAsync(userId);
            });
        }

        [Test]
        public async Task CreateTrainerAsync_ShouldDoNothing_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = "nonexistent";
            var model = new CreateTrainerUserVM
            {
                UserId = userId,
                GymId = 1,
                SpecialtyId = 2,
                Image = "img.jpg"
            };

            var userManagerMock = MockUserManager(new List<ApplicationUser>());
            userManagerMock.Setup(m => m.FindByIdAsync(userId)).ReturnsAsync((ApplicationUser)null);

            var context = GetInMemoryDbContext();
            var service = new TrainerService(context, userManagerMock.Object);

            // Act
            await userService.CreateTrainerAsync(model);

            // Assert
            var trainer = await context.Trainers.FirstOrDefaultAsync(t => t.UserId == userId);
            Assert.That(trainer, Is.Null);
        }

        [Test]
        public async Task CreateTrainerAsync_ShouldThrow_WhenAddToRoleFails()
        {
            // Arrange
            var user = new ApplicationUser { Id = "user1" };
            var model = new CreateTrainerUserVM
            {
                UserId = user.Id,
                GymId = 1,
                SpecialtyId = 2,
                Image = "img.jpg"
            };

            var userManagerMock = MockUserManager(new List<ApplicationUser> { user });
            userManagerMock.Setup(m => m.FindByIdAsync(user.Id)).ReturnsAsync(user);
            userManagerMock.Setup(m => m.GetRolesAsync(user)).ReturnsAsync(new List<string>());
            userManagerMock.Setup(m => m.RemoveFromRolesAsync(user, It.IsAny<IEnumerable<string>>())).ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(m => m.AddToRoleAsync(user, "Trainer")).ReturnsAsync(IdentityResult.Failed());

            var context = GetInMemoryDbContext();
            var service = new UserService(context, userManagerMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await service.CreateTrainerAsync(model));
        }
        
        [Test]
        public async Task GetUserForTrainerAsync_ShouldReturnNull_WhenNotAdmin()
        {
            // Arrange
            var userManagerMock = MockUserManager(new List<ApplicationUser>());
            var context = GetInMemoryDbContext();
            var service = new UserService(context, userManagerMock.Object);

            // Act
            var result = await service.GetUserForTrainerAsync("someId", false);

            // Assert
            Assert.That(result, Is.Null);
        }
        [Test]
        public async Task GetUserForTrainerAsync_ShouldReturnNull_WhenUserIdIsNull()
        {
            // Arrange
            var userManagerMock = MockUserManager(new List<ApplicationUser>());
            var context = GetInMemoryDbContext();
            var service = new UserService(context, userManagerMock.Object);

            // Act
            var result = await service.GetUserForTrainerAsync(null, true);

            // Assert
            Assert.That(result, Is.Null);
        }
        [Test]
        public async Task GetUserForTrainerAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            var userManagerMock = MockUserManager(new List<ApplicationUser>());
            userManagerMock.Setup(m => m.FindByIdAsync("user123")).ReturnsAsync((ApplicationUser)null);

            var context = GetInMemoryDbContext();
            var service = new UserService(context, userManagerMock.Object);

            // Act
            var result = await service.GetUserForTrainerAsync("user123", true);

            // Assert
            Assert.That(result, Is.Null);
        }









    }
}
