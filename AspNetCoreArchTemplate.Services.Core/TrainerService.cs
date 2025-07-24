using AspNetCoreArchTemplate.Data;
using FitnessPlatform.Data.Models;
using FitnessPlatform.Services.Core.Contracts;
using FitnessPlatform.Web.ViewModels.Trainer;
using FitnessPlatform.Web.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Services.Core
{
    public class TrainerService : ITrainerService
    {
        private readonly FitnessDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;

        public TrainerService(FitnessDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }
        public async Task<IEnumerable<TrainerVM>> GetAllTrainersAsync(bool isAdmin)
        {
            var trainers = await dbContext.Trainers
                .Include(t => t.User)
                .Include(t => t.Gym)
                .Include(t => t.Specialty)
                .ToListAsync();
            IEnumerable<TrainerVM> trainer = trainers.Select(u => new TrainerVM
            {
                TrainerId = u.Id,
                Gym = u.Gym.Name,
                FullName = $"{u.User.FirstName} {u.User.LastName}",
                Image = u.TrainerImage,
                Specialty = u.Specialty.Name,
                PhoneNumber = u.User.PhoneNumber
            });
            return trainer;
        }

        public async Task<TrainerDetailsVM> GetTrainerDetailsAsync(int trainerId,string userId, bool isAdmin)
        {
            var trainer = await dbContext.Trainers.Where(t => t.Id == trainerId)
                .Include(t => t.User)
                .Include(t => t.Gym)
                .Include(t => t.Specialty)
                .Include(t => t.Clients)
                .FirstOrDefaultAsync();
            if (trainer == null)
            {
                return null; // или хвърли грешка, ако е критично
            }


            TrainerDetailsVM trainerDetails = new TrainerDetailsVM
            {
                TrainerId = trainer.Id,
                Gym = trainer.Gym.Name,
                FullName = $"{trainer.User.FirstName} {trainer.User.LastName}",
                Image = trainer.TrainerImage,
                Specialty = trainer.Specialty.Name,
                SpecialtyDescription = trainer.Specialty.Description,
                PhoneNumber = trainer.User.PhoneNumber,
                IsUserSubscribe = trainer.Clients.Any(c=>c.ClientId == userId)
            };
            return trainerDetails;

        }

        public async Task RemoveTrainer(int trainersId, bool isAdmin)
        {
           var trainer = await dbContext.Trainers.Include(t => t.User).FirstOrDefaultAsync(t => t.Id == trainersId);
            if (trainer == null)
            {
                throw new ArgumentException("Trainer not found.");
            }

            //Take userId from trainer
            ApplicationUser user = trainer.User;
            if (user != null)
            {
                var roles = await userManager.GetRolesAsync(user);
                await userManager.RemoveFromRolesAsync(user, roles);
                await userManager.AddToRoleAsync(user, "User");
            }

            dbContext.Trainers.Remove(trainer);
            await dbContext.SaveChangesAsync();
        }

        public async Task UserSubscribeToTrainer(int trainerId, string userId)
        {
            var trainer = await dbContext.Trainers
         .Include(t => t.User)
         .FirstOrDefaultAsync(t => t.Id == trainerId);

            if (trainer == null)
            {
                throw new ArgumentException("Trainer not found.");
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            TrainerClient trainerClient = new TrainerClient
            {
                TrainerId = trainer.Id,
                Trainer = trainer, // това е ключово
                ClientId = userId
            };

            dbContext.TrainerClients.Add(trainerClient);
            await dbContext.SaveChangesAsync();
        }

    }
    
}
