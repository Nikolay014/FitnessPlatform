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

            return new GymDetailsVM
            {
                Id = gym.Id,
                Name = gym.Name,
                Location = gym.Location,
                Description = gym.Description,
                Images = gym.Images.Select(i => i.ImageUrl).ToList(),
                IsUserSubscribed = gym.Subscribers.Any(s => s.UserId == userId),
                IsAdmin = isAdmin
            };
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

        public async Task<EditGymVM> GetRecipeForEditAsync(int id)
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

        public Task EditRecipeAsync(EditGymVM model)
        {
            throw new NotImplementedException();
        }
    }
}
