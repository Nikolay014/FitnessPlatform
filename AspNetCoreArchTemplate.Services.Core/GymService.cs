using AspNetCoreArchTemplate.Data;
using FitnessPlatform.Data.Models;
using FitnessPlatform.Services.Core.Contracts;
using FitnessPlatform.Web.ViewModels.Gym;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Services.Core
{
    public class GymService:IGymService
    {
        private readonly FitnessDbContext dbContext; 
        public GymService(FitnessDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task CreateGymAsync(CreateGymVM model)
        {
            var gym = new Gym
            {
                Name = model.Name,
                Location = model.Location,
                Description = model.Description,
                
            };

            dbContext.Gym.Add(gym);
            await dbContext.SaveChangesAsync();

            var mainImage = new GymImage
            {
                GymId = gym.Id,
                ImageUrl = model.MainImageUrl,
                IsPrimary = true
            };

            dbContext.GymImage.Add(mainImage);
            await dbContext.SaveChangesAsync();
        }

      

        public async Task<GymDetailsVM> GetGymDetailsAsync(int gymId, string userId, bool isAdmin)
        {
            var gym = await dbContext.Gym
                .Include(g => g.Images)
                .Include(g => g.Subscribers)
                .FirstOrDefaultAsync(g => g.Id == gymId);

            if (gym == null) throw new ArgumentException("Gym not found.");

            GymDetailsVM gymDetails = new GymDetailsVM
            {
                Id = gym.Id,
                Name = gym.Name,
                Location = gym.Location,
                Description = gym.Description,
                Images = gym.Images.Select(i => i.ImageUrl).ToList(),
                IsUserSubscribed = gym.Subscribers.Any(s => s.UserId == userId),
                IsAdmin = isAdmin
            };
            if(gymDetails.IsUserSubscribed)
            {
                var validUntil = dbContext.UserGymSubscription
                    .Where(ugs => ugs.GymId == gymId && ugs.UserId == userId)
                    .Select(ugs => ugs.ValidUntil)
                    .FirstOrDefault();
                
                
                    gymDetails.SubscriptionEndDate = validUntil;
                
            }

            return gymDetails;
        }

        public async Task<PaginatedGymsVM> GetGymsAsync(string? userId, string? location = null, int page = 1, int pageSize = 3)
        {
            var query = dbContext.Gym
         .Select(g => new GymVM
         {
             Id = g.Id,
             Name = g.Name,
             Location = g.Location,
             PrimaryImage = g.Images
                 .Where(i => i.IsPrimary)
                 .Select(i => i.ImageUrl)
                 .FirstOrDefault()
         });

            if (!string.IsNullOrEmpty(location))
            {
                query = query.Where(g => g.Location.ToLower().Contains(location.ToLower()));
            }

            int totalGyms = await query.CountAsync();

            var gyms = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedGymsVM
            {
                Gyms = gyms,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalGyms / (double)pageSize),
                Location = location
            };
        
        }
        

        public async Task<DeleteGymVM> GetGymForDeleteAsync(int id, string? userId, bool isAdmin)
        {
            var gym = await dbContext.Gym
              .AsNoTracking()
              .Where(g => g.Id == id)
              .Select(g => new DeleteGymVM
              {
                  Id = g.Id,
                  Name = g.Name,
                  IsAdmin = isAdmin,
                  
              })
              .FirstOrDefaultAsync();

            // Проверка за съществуване и достъп
            if (gym == null || (!isAdmin))
            {
                return null;
            }

            return gym;
        }
        public async Task DeleteGymAsync(int gymId)
        {
            Gym? gym = await dbContext.Gym
             .Where(g => g.Id == gymId)
             .FirstOrDefaultAsync();

            if (gym == null)
                return;

            // Проверка за достъп, ако userId е подаден
           

            gym.IsDeleted = true;
            await dbContext.SaveChangesAsync();
        }

        public async Task<EditGymVM> GetGymForEditAsync(int id)
        {
            Gym gym = await dbContext.Gym
                .Include(g => g.Images)
                .FirstOrDefaultAsync(g => g.Id == id);

            return new EditGymVM
            {
                Id = gym.Id,
                Name = gym.Name,
                Location = gym.Location,
                Description = gym.Description,
                MainImageUrl = gym.Images.FirstOrDefault(i => i.IsPrimary)?.ImageUrl ?? string.Empty
            };

        }

        public async Task EditGymAsync(EditGymVM model)
        {
           Gym  gym =  await dbContext.Gym.Include(g => g.Images).
                Where(g=>g.Id == model.Id).FirstOrDefaultAsync();

            if (gym == null)
                return;

            gym.Name = model.Name;
            gym.Location = model.Location;
            gym.Description = model.Description;
            gym.Images.Where(i => i.IsPrimary).FirstOrDefault().ImageUrl = model.MainImageUrl;

           
            await dbContext.SaveChangesAsync();
            

        }

        public async Task<SubscribeGymVM> GetSubscriptionPlansAsync(int id)
        {
            var plans = await dbContext.SubscriptionPlans.ToListAsync();

            var vm = new SubscribeGymVM
            {
                GymId = id,
                AvailablePlans = plans
            };
            return vm;
        }

        public async Task SubscribeToGymAsync(int gymId, string userId, int planId)
        {
            var plan = await dbContext.SubscriptionPlans
                .FirstOrDefaultAsync(p => p.Id == planId);
            var planDuration = plan.DurationInDays;
            UserGymSubscription userGymSubscription = new UserGymSubscription
            {
                GymId = gymId,
                UserId = userId,
                SubscriptionPlanId = planId,
                ValidUntil = DateTime.UtcNow.AddDays(planDuration) // Примерно 1 месец
            };

            dbContext.UserGymSubscription.Add(userGymSubscription);
            await dbContext.SaveChangesAsync();
        }

        public async  Task<GymWithSubscribersVM> GetSubscribedUsersAsync(int id, string userId)
        {
            if (id == null)
            {
                throw new ArgumentException("Invalid gym ID.");
            }
            var gym = await dbContext.Gym.FirstOrDefaultAsync(g => g.Id == id);

           

            var users = await dbContext.UserGymSubscription
                .Where(s => s.GymId == id)
                .Include(s => s.User)
                .Select(s => new SubscribeUserVM
                {
                    Id = s.UserId,
                    FullName = s.User.FirstName + " " + s.User.LastName,
                    Image = s.User.ImageUrl,
                    PhoneNumber = s.User.PhoneNumber,
                    Gender = s.User.Gender,
                    SubscriptionEndDate = s.ValidUntil,
                    
                })
                .ToListAsync();

            var vm = new GymWithSubscribersVM
            {
                GymName = gym.Name,
                Users = users
            };

            return vm;
        }

        public async Task<GymWithTrainersVM> GetGymTrainersAsync(int id, string userId)
        {
            if (id == null)
            {
                throw new ArgumentException("Invalid gym ID.");
            }
            var gym = await dbContext.Gym
                 .Include(g => g.Trainers)
                     .ThenInclude(t => t.User)
                 .Include(g => g.Trainers)
                     .ThenInclude(t => t.Specialty)
                 .FirstOrDefaultAsync(g => g.Id == id);



            var trainers =  gym.Trainers
                .Select(s => new GymTrainersVM
                {
                    TrainerId = s.Id,
                    FullName = s.User.FirstName + " " + s.User.LastName,
                    Image = s.TrainerImage,
                    Specialty = s.Specialty.Name,
                    PhoneNumber = s.User.PhoneNumber,
                   
                    

                })
                .ToList();

            var vm = new GymWithTrainersVM
            {
                GymName = gym.Name,
                Trainers = trainers
            };

            return vm;
        }
    }
}
