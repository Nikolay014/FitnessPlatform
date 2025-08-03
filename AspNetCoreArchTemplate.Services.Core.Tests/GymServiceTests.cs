using AspNetCoreArchTemplate.Data;
using FitnessPlatform.Data.Models;
using FitnessPlatform.Services.Core.Contracts;
using FitnessPlatform.Web.ViewModels.Gym;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.EntityFrameworkCore;


namespace FitnessPlatform.Services.Core.Tests
{
    [TestFixture]
    public class GymServiceTests
    {
        private FitnessDbContext context;
        private IGymService gymService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<FitnessDbContext>()
                .UseInMemoryDatabase("TestDb_" + Guid.NewGuid().ToString())
                .Options;

            this.context = new FitnessDbContext(options);
            this.gymService = new GymService(this.context);
        }

        [TearDown]
        public void TearDown()
        {
            this.context.Dispose();
        }

        [Test]
        public async Task CreateGymAsync_ShouldAddGymAndImage()
        {
            // Arrange
            var model = new CreateGymVM
            {
                Name = "Test Gym",
                Location = "Test City",
                Description = "Test Desc",
                MainImageUrl = "http://image.com/img.jpg"
            };

            // Act
            await gymService.CreateGymAsync(model);

            // Assert
            var gym = await context.Gym.FirstOrDefaultAsync();
            Assert.IsNotNull(gym);
            Assert.AreEqual(model.Name, gym.Name);

            var image = await context.GymImage.FirstOrDefaultAsync();
            Assert.IsNotNull(image);
            Assert.AreEqual(gym.Id, image.GymId);
            Assert.IsTrue(image.IsPrimary);
            Assert.AreEqual(model.MainImageUrl, image.ImageUrl);
        }
        [Test]
        public void CreateGymAsync_ShouldThrow_WhenModelIsNull()
        {
            CreateGymVM model = null;

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await gymService.CreateGymAsync(model));

            Assert.That(ex.ParamName, Is.EqualTo("model"));
        }
        [Test]
        public async Task CreateGymAsync_ShouldThrow_WhenNameIsNull()
        {
            // Arrange
            var model = new CreateGymVM
            {
                Name = null,
                Location = "Test",
                Description = "Test",
                MainImageUrl = "https://valid.url/image.jpg"
            };

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await gymService.CreateGymAsync(model));

            Assert.That(ex.ParamName, Is.EqualTo("Name"));
        }
        [Test]
        public async Task CreateGymAsync_ShouldThrow_WhenMainImageUrlIsNull()
        {
            var model = new CreateGymVM
            {
                Name = "Test Gym",
                Location = "Varna",
                Description = "Valid desc",
                MainImageUrl = null
            };

            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await gymService.CreateGymAsync(model));

            Assert.That(ex.ParamName, Is.EqualTo("MainImageUrl"));
        }
        [Test]
        public async Task CreateGymAsync_ShouldThrow_WhenLocationIsEmpty()
        {
            var model = new CreateGymVM
            {
                Name = "Some name",
                Location = null,
                Description = "description",
                MainImageUrl = "https://img.com/image.jpg"
            };

            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await gymService.CreateGymAsync(model));

            Assert.That(ex.ParamName, Is.EqualTo("Location"));
        }
        [Test]
        public async Task GetGymsAsync_ShouldReturnAllGyms_WhenNoFilter()
        {
            context.Gym.AddRange(
                new Gym
                {
                    Name = "A",
                    Location = "Sofia",
                    Description = "Desc A",
                    Images = new List<GymImage> { new GymImage { ImageUrl = "img1", IsPrimary = true } }
                },
                new Gym
                {
                    Name = "B",
                    Location = "Plovdiv",
                    Description = "Desc B",
                    Images = new List<GymImage> { new GymImage { ImageUrl = "img2", IsPrimary = true } }
                }
            );
            await context.SaveChangesAsync();

            var result = await gymService.GetGymsAsync(null, null, 1, 3);

            Assert.That(result.Gyms.Count, Is.EqualTo(2));
            Assert.That(result.TotalPages, Is.EqualTo(1));
        }

        [Test]
        public async Task GetGymsAsync_ShouldFilterByLocation()
        {
            context.Gym.AddRange(
                new Gym
                {
                    Name = "A",
                    Location = "Sofia",
                    Description = "Desc A",
                    Images = new List<GymImage> { new GymImage { ImageUrl = "img1", IsPrimary = true } }
                },
                new Gym
                {
                    Name = "B",
                    Location = "Plovdiv",
                    Description = "Desc B",
                    Images = new List<GymImage> { new GymImage { ImageUrl = "img2", IsPrimary = true } }
                }
            );
            await context.SaveChangesAsync();

            var result = await gymService.GetGymsAsync(null, location: "sofia");

            Assert.That(result.Gyms.Count, Is.EqualTo(1));
            Assert.That(result.Gyms.First().Location.ToLower(), Is.EqualTo("sofia"));
        }

        [Test]
        public async Task GetGymsAsync_ShouldReturnEmpty_WhenNoGyms()
        {
            var result = await gymService.GetGymsAsync(null, null, 1, 3);

            Assert.That(result.Gyms, Is.Empty);
            Assert.That(result.TotalPages, Is.EqualTo(0));
        }

        [Test]
        public async Task GetGymsAsync_ShouldPaginateCorrectly()
        {
            for (int i = 0; i < 10; i++)
            {
                context.Gym.Add(new Gym
                {
                    Name = $"Gym {i}",
                    Location = "Sofia",
                    Description = $"Description for gym {i}",
                    Images = new List<GymImage>
            {
                new GymImage { ImageUrl = $"img{i}", IsPrimary = true }
            }
                });
            }

            await context.SaveChangesAsync();

            var result = await gymService.GetGymsAsync(null, null, page: 2, pageSize: 3);

            Assert.That(result.Gyms.Count, Is.EqualTo(3));      // gyms 3–5
            Assert.That(result.CurrentPage, Is.EqualTo(2));
            Assert.That(result.TotalPages, Is.EqualTo(4));      // 10 gyms / 3 per page = 4 pages
        }
        [Test]
        public async Task GetGymDetailsAsync_ShouldReturnGymDetails_WhenUserNotSubscribed()
        {
            var gym = new Gym
            {
                Id = 1,
                Name = "Test Gym",
                Location = "Sofia",
                Description = "Desc",
                Images = new List<GymImage>
        {
            new GymImage { ImageUrl = "url1" }
        }
            };
            context.Gym.Add(gym);
            await context.SaveChangesAsync();

            var result = await gymService.GetGymDetailsAsync(1, userId: "123", isAdmin: false);

            Assert.That(result.Name, Is.EqualTo("Test Gym"));
            Assert.False(result.IsUserSubscribed);
            Assert.False(result.IsAdmin);
            Assert.That(result.Images.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetGymDetailsAsync_ShouldReturnGymDetails_WhenUserSubscribed()
        {
            var userId = "user1";
            var gym = new Gym
            {
                Id = 2,
                Name = "Gym B",
                Location = "Plovdiv",
                Description = "Desc",
                Subscribers = new List<UserGymSubscription>
        {
            new UserGymSubscription
            {
                UserId = userId,
                ValidUntil = DateTime.UtcNow.AddDays(30),
                SubscriptionPlanId = 1
            }
        }
            };
            context.Gym.Add(gym);
            context.UserGymSubscription.AddRange(gym.Subscribers);
            await context.SaveChangesAsync();

            var result = await gymService.GetGymDetailsAsync(2, userId, isAdmin: false);

            Assert.True(result.IsUserSubscribed);
            Assert.NotNull(result.SubscriptionEndDate);
        }

        [Test]
        public void GetGymDetailsAsync_ShouldThrow_WhenGymNotFound()
        {
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await gymService.GetGymDetailsAsync(999, "user", false));

            Assert.That(ex.Message, Does.Contain("Gym not found"));
        }
        [Test]
        public async Task GetGymForDeleteAsync_ShouldReturnViewModel_WhenGymExistsAndIsAdmin()
        {
            context.Gym.Add(new Gym
            {
                Id = 1,
                Name = "Test Gym",
                Location = "Sofia",
                Description = "Nice gym"
            });
            await context.SaveChangesAsync();

            var result = await gymService.GetGymForDeleteAsync(1, "user1", true);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Name, Is.EqualTo("Test Gym"));
            Assert.That(result.IsAdmin, Is.True);
        }
        [Test]
        public async Task GetGymForDeleteAsync_ShouldReturnNull_WhenGymDoesNotExist()
        {
            var result = await gymService.GetGymForDeleteAsync(999, "user1", true);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetGymForDeleteAsync_ShouldReturnNull_WhenUserIsNotAdmin()
        {
            context.Gym.Add(new Gym
            {
                Id = 2,
                Name = "User Gym",
                Location = "Plovdiv",
                Description = "Some gym"
            });
            await context.SaveChangesAsync();

            var result = await gymService.GetGymForDeleteAsync(2, "user2", false);

            Assert.That(result, Is.Null);
        }
        [Test]
        public async Task DeleteGymAsync_ShouldMarkGymAsDeleted_WhenGymExists()
        {
            var gym = new Gym
            {
                Id = 1,
                Name = "To Be Deleted",
                Location = "Varna",
                Description = "Test",
                IsDeleted = false
            };
            context.Gym.Add(gym);
            await context.SaveChangesAsync();

            await gymService.DeleteGymAsync(1);

            var updatedGym = await context.Gym.FindAsync(1);
            Assert.That(updatedGym.IsDeleted, Is.True);
        }
        [Test]
        public async Task DeleteGymAsync_ShouldDoNothing_WhenGymDoesNotExist()
        {
            // Няма нужда да добавяме фитнес

            // Просто да не хвърля грешка
            Assert.DoesNotThrowAsync(async () => await gymService.DeleteGymAsync(999));
        }
        [Test]
        public async Task GetGymForEditAsync_ShouldReturnEditGymVM_WhenGymExists()
        {
            var gym = new Gym
            {
                Id = 1,
                Name = "Test Gym",
                Location = "Sofia",
                Description = "Description",
                Images = new List<GymImage>
        {
            new GymImage { ImageUrl = "https://img.com/1.jpg", IsPrimary = true },
            new GymImage { ImageUrl = "https://img.com/2.jpg", IsPrimary = false }
        }
            };
            context.Gym.Add(gym);
            await context.SaveChangesAsync();

            var result = await gymService.GetGymForEditAsync(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Name, Is.EqualTo("Test Gym"));
            Assert.That(result.Location, Is.EqualTo("Sofia"));
            Assert.That(result.MainImageUrl, Is.EqualTo("https://img.com/1.jpg"));
        }
        [Test]
        public async Task GetGymForEditAsync_ShouldReturnEmptyImageUrl_WhenNoPrimaryImageExists()
        {
            var gym = new Gym
            {
                Id = 2,
                Name = "No Primary Image",
                Location = "Plovdiv",
                Description = "Test",
                Images = new List<GymImage>
        {
            new GymImage { ImageUrl = "https://img.com/only.jpg", IsPrimary = false }
        }
            };
            context.Gym.Add(gym);
            await context.SaveChangesAsync();

            var result = await gymService.GetGymForEditAsync(2);

            Assert.That(result.MainImageUrl, Is.EqualTo(string.Empty));
        }
        [Test]
        public void GetGymForEditAsync_ShouldThrow_WhenGymDoesNotExist()
        {
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await gymService.GetGymForEditAsync(999));

            Assert.That(ex, Is.Not.Null);
        }
        [Test]
        public async Task EditGymAsync_ShouldEditGym_WhenDataIsValid()
        {
            var gym = new Gym
            {
                Id = 1,
                Name = "Old Name",
                Location = "Old Location",
                Description = "Old Description",
                Images = new List<GymImage>
        {
            new GymImage { ImageUrl = "old.jpg", IsPrimary = true }
        }
            };
            context.Gym.Add(gym);
            await context.SaveChangesAsync();

            var model = new EditGymVM
            {
                Id = 1,
                Name = "New Name",
                Location = "New Location",
                Description = "New Description",
                MainImageUrl = "new.jpg"
            };

            await gymService.EditGymAsync(model);

            var updated = await context.Gym.Include(g => g.Images).FirstAsync();

            Assert.That(updated.Name, Is.EqualTo("New Name"));
            Assert.That(updated.Location, Is.EqualTo("New Location"));
            Assert.That(updated.Description, Is.EqualTo("New Description"));
            Assert.That(updated.Images.First(i => i.IsPrimary).ImageUrl, Is.EqualTo("new.jpg"));
        }

        [Test]
        public async Task EditGymAsync_ShouldDoNothing_WhenGymDoesNotExist()
        {
            var model = new EditGymVM
            {
                Id = 999,
                Name = "Doesn't matter",
                Location = "None",
                Description = "None",
                MainImageUrl = "none.jpg"
            };

            // Да не хвърли грешка
            Assert.DoesNotThrowAsync(async () => await gymService.EditGymAsync(model));
        }
        [Test]
        public async Task GetSubscriptionPlansAsync_ShouldReturnAllPlans()
        {
            context.SubscriptionPlans.AddRange(
                new SubscriptionPlan { Id = 1, Name = "1 Month", Price = 20 },
                new SubscriptionPlan { Id = 2, Name = "1 Year", Price = 200 }
            );
            await context.SaveChangesAsync();

            var result = await gymService.GetSubscriptionPlansAsync(10); // gymId може да е всякакъв

            Assert.That(result, Is.Not.Null);
            Assert.That(result.GymId, Is.EqualTo(10));
            Assert.That(result.AvailablePlans.Count, Is.EqualTo(2));
            Assert.That(result.AvailablePlans.Any(p => p.Name == "1 Month"), Is.True);
            Assert.That(result.AvailablePlans.Any(p => p.Name == "1 Year"), Is.True);
        }
        [Test]
        public async Task GetSubscriptionPlansAsync_ShouldReturnEmptyList_WhenNoPlansExist()
        {
            // Без добавени планове

            var result = await gymService.GetSubscriptionPlansAsync(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.GymId, Is.EqualTo(1));
            Assert.That(result.AvailablePlans, Is.Empty);
        }
        [Test]
        public async Task SubscribeToGymAsync_ShouldCreateSubscriptionCorrectly()
        {
            var gym = new Gym { Name = "Test Gym", Location = "Test", Description = "Desc" };
            var plan = new SubscriptionPlan { Id = 1, Name = "1 Month", DurationInDays = 30, Price = 50 };

            context.Gym.Add(gym);
            context.SubscriptionPlans.Add(plan);
            await context.SaveChangesAsync();

            string userId = "user-123";

            await gymService.SubscribeToGymAsync(gym.Id, userId, plan.Id);

            var subscription = await context.UserGymSubscription
                .FirstOrDefaultAsync(s => s.UserId == userId && s.GymId == gym.Id);

            Assert.That(subscription, Is.Not.Null);
            Assert.That(subscription.SubscriptionPlanId, Is.EqualTo(plan.Id));
            Assert.That(subscription.ValidUntil.Date, Is.EqualTo(DateTime.UtcNow.AddDays(30).Date).Within(1).Days);
        }
        
        [Test]
        public void SubscribeToGymAsync_ShouldThrow_WhenPlanDoesNotExist()
        {
            var gym = new Gym { Name = "Test Gym", Location = "Test", Description = "Desc" };
            context.Gym.Add(gym);
            context.SaveChanges();

            string userId = "user-123";

            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await gymService.SubscribeToGymAsync(gym.Id, userId, planId: 999));

            Assert.That(ex.Message, Is.EqualTo("Invalid subscription plan."));
        }

        [Test]
        public async Task SubscribeToGymAsync_ShouldAllowMultipleSubscriptionsForSameUserToDifferentGyms()
        {
            var userId = "user-456";
            var plan = new SubscriptionPlan { Id = 5, Name = "1 Year", DurationInDays = 365 };
            context.SubscriptionPlans.Add(plan);

            context.Gym.AddRange(
                new Gym { Id = 1, Name = "Gym A", Location = "Loc A", Description = "desc" },
                new Gym { Id = 2, Name = "Gym B", Location = "Loc B", Description = "desc" }
            );
            await context.SaveChangesAsync();

            await gymService.SubscribeToGymAsync(1, userId, 5);
            await gymService.SubscribeToGymAsync(2, userId, 5);

            var subs = await context.UserGymSubscription.Where(s => s.UserId == userId).ToListAsync();
            Assert.That(subs.Count, Is.EqualTo(2));
        }
        [Test]
        public void GetSubscribedUsersAsync_ShouldThrow_WhenGymIdIsInvalid()
        {
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await gymService.GetSubscribedUsersAsync(0, "user123"));

            Assert.That(ex.Message, Is.EqualTo("Invalid gym ID."));
        }
        [Test]
        public async Task GetSubscribedUsersAsync_ShouldReturnEmptyList_WhenNoSubscribers()
        {
            var gym = new Gym { Name = "Empty Gym", Location = "Varna", Description = "desc" };
            context.Gym.Add(gym);
            await context.SaveChangesAsync();

            var result = await gymService.GetSubscribedUsersAsync(gym.Id, "user123");

            Assert.That(result.GymName, Is.EqualTo("Empty Gym"));
            Assert.That(result.Users.Count, Is.EqualTo(0));
        }
        [Test]
        public async Task GetSubscribedUsersAsync_ShouldReturnSubscribedUsers()
        {
            var user = new ApplicationUser
            {
                Id = "user123",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                Gender = "Male"
            };

            var gym = new Gym { Name = "Test Gym", Location = "Sofia", Description = "desc" };
            context.Users.Add(user);
            context.Gym.Add(gym);
            await context.SaveChangesAsync();

            var subscription = new UserGymSubscription
            {
                GymId = gym.Id,
                UserId = user.Id,
                SubscriptionPlanId = 1,
                ValidUntil = DateTime.UtcNow.AddMonths(1)
            };
            context.UserGymSubscription.Add(subscription);
            await context.SaveChangesAsync();

            var result = await gymService.GetSubscribedUsersAsync(gym.Id, user.Id);

            Assert.That(result.GymName, Is.EqualTo("Test Gym"));
            Assert.That(result.Users.Count, Is.EqualTo(1));
            Assert.That(result.Users.First().FullName, Is.EqualTo("John Doe"));
        }
        [Test]
        public async Task GetGymTrainersAsync_ShouldReturnTrainers_WhenGymExists()
        {
            var gym = new Gym
            {
                Name = "Test Gym",
                Location = "Sofia", // Добавено
                Description = "Basic gym", // Добавено
                Trainers = new List<Trainer>
                {
                    new Trainer
                    {
                        User = new ApplicationUser
                        {
                            FirstName = "Ivan",
                            LastName = "Ivanov",
                            PhoneNumber = "12345",
                            Gender = "Male" 
                        },
                        TrainerImage = "img.jpg",
                        Specialty = new Specialty { Name = "Fitness" }
                    }
                }
            };
        

            context.Gym.Add(gym);
            await context.SaveChangesAsync();

            var result = await gymService.GetGymTrainersAsync(gym.Id, "user123");

            Assert.That(result.GymName, Is.EqualTo("Test Gym"));
            Assert.That(result.Trainers.Count, Is.EqualTo(1));
            Assert.That(result.Trainers.First().FullName, Is.EqualTo("Ivan Ivanov"));
            Assert.That(result.Trainers.First().Specialty, Is.EqualTo("Fitness"));
        }

        [Test]
        public void GetGymTrainersAsync_ShouldThrow_WhenGymDoesNotExist()
        {
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await gymService.GetGymTrainersAsync(999, "user123"));

            Assert.That(ex.Message, Is.EqualTo("Invalid gym ID."));
        }





















    }


}
