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

        public async Task<IEnumerable<GymVM>> GetGymsAsync(string? userId)
        {
            var gyms = await dbContext.Gym
                .Select(g => new GymVM
                {
                    Id = g.Id,
                    Name = g.Name,
                    Location = g.Location,
                    PrimaryImage = g.Images.Where(i=>i.IsPrimary)
                        .Select(i => i.ImageUrl)
                        .FirstOrDefault()
                        
                   
                })
                .ToListAsync();

            return gyms;
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
    }
}
