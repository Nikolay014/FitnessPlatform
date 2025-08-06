using AspNetCoreArchTemplate.Data;
using Azure;
using FitnessPlatform.Data.Models;
using FitnessPlatform.Services.Core.Contracts;
using FitnessPlatform.Web.ViewModels.Gym;
using FitnessPlatform.Web.ViewModels.Trainer;
using FitnessPlatform.Web.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        public async Task<PaginatedTrainersVM> GetAllTrainersAsync(int? gymId, int? specialtyId, int page, bool isAdmin)
        {
            const int PageSize = 3;
            var trainersQuery = dbContext.Trainers
                .Include(t => t.User)
                .Include(t => t.Gym)
                .Include(t => t.Specialty)
                .AsQueryable();

            if (gymId.HasValue)
                trainersQuery = trainersQuery.Where(t => t.GymId == gymId.Value);

            if (specialtyId.HasValue)
                trainersQuery = trainersQuery.Where(t => t.SpecialtyId == specialtyId.Value);

            var totalTrainers = await trainersQuery.CountAsync();

            var trainers = await trainersQuery
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .Select(t => new TrainerVM
                {
                    TrainerId = t.Id,
                    Gym = t.Gym.Name,
                    FullName = $"{t.User.FirstName} {t.User.LastName}",
                    Image = t.TrainerImage,
                    Specialty = t.Specialty.Name,
                    PhoneNumber = t.User.PhoneNumber
                })
                .ToListAsync();

            return new PaginatedTrainersVM
            {
                Trainers = trainers,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)totalTrainers / PageSize)
            };
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

        public async Task UnUserSubscribeToTrainer(int trainerId, string userId)
        {
            var trainerClient = await dbContext.TrainerClients
                .FirstOrDefaultAsync(tc => tc.TrainerId == trainerId && tc.ClientId == userId);

            if (trainerClient == null)
            {
                throw new ArgumentException("Subscription not found.");
            }

            dbContext.TrainerClients.Remove(trainerClient);
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
        public async Task<EditTrainerVM> GetTrainerForUpdate(int id, bool isAdmin)
        {
            if (!isAdmin)
            {
                throw new ArgumentException("Admin not found.");
            }
            Trainer trainer = await dbContext.Trainers.FirstOrDefaultAsync(t => t.Id == id);

            if (trainer == null)
            {
                throw new ArgumentException("Invalid trainer.");
            }
            EditTrainerVM editTrainerVM = new EditTrainerVM 
            {
                TrainerId = id,
                GymId = trainer.GymId,
                SpecialtyId = trainer.SpecialtyId,
                Image = trainer.TrainerImage,
                Gyms = dbContext.Gym.ToList(),
                Specialties = dbContext.Specialties.ToList(),

            };
            return editTrainerVM;


        }

        public async Task UpdateTrainerAsync(EditTrainerVM editTrainerVM, bool isAdmin)
        {
            if (!isAdmin)
            {
                throw new ArgumentException("Admin not found.");
            }
            Trainer trainer = await dbContext.Trainers.FirstOrDefaultAsync(t=>t.Id == editTrainerVM.TrainerId);

            trainer.TrainerImage = editTrainerVM.Image;
            trainer.GymId = editTrainerVM.GymId;
            trainer.SpecialtyId = editTrainerVM.SpecialtyId;

            await dbContext.SaveChangesAsync();
        }

        public async Task<TrainerClientsVM> GetClientsAsync(int id, string? userid)
        {
            if (id == 0)
            {
                

                var trainerid = await dbContext.Trainers
                    .FirstOrDefaultAsync(t => t.UserId == userid);

               

                
                id = trainerid.Id;
            }

            Trainer trainer = await dbContext.Trainers.Include(t=>t.User).FirstOrDefaultAsync(t => t.Id == id);

            if (trainer == null)
            {
                throw new ArgumentException("Invalid trainer.");
            }

            var users = await dbContext.TrainerClients
                .Where(t => t.TrainerId == id)
                .Include(t=>t.Client)
                .Select(s => new TrainerClientVM
                {
                    Id = s.ClientId,
                    FullName = s.Client.FirstName + " " + s.Client.LastName,
                    Image = s.Client.ImageUrl,
                    PhoneNumber = s.Client.PhoneNumber,
                    Gender = s.Client.Gender,
                   

                })
                .ToListAsync();

            var vm = new TrainerClientsVM
            {
                TrainerName = trainer.User.FirstName + " " + trainer.User.LastName,
                Clients = users
            };

            return vm;

        }

        public async Task<TrainerEventsVM> GetEventsAsync(int id, string? userId)
        {
            if (id == 0)
            {
                var trainerEntity = await dbContext.Trainers
                    .FirstOrDefaultAsync(t => t.UserId == userId);

                if (trainerEntity == null)
                {
                    throw new ArgumentException("Trainer not found by user id.");
                }

                id = trainerEntity.Id;
            }

            var trainer = await dbContext.Trainers
                .Include(t => t.Events)
                    .ThenInclude(e => e.Gym)
                .Include(t => t.Events)
                    .ThenInclude(e => e.Trainer)
                        .ThenInclude(tr => tr.User)
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (trainer == null)
            {
                throw new ArgumentException("Invalid trainer.");
            }

            var events = trainer.Events.Where(e => e.StartDate > DateTime.Now)
                .Select(e => new TrainerEventVM
                {
                    Id = e.Id,
                    Title = e.Title,
                    Image = e.Image,
                    TrainerId = e.TrainerId,
                    Trainer = $"{e.Trainer.User.FirstName} {e.Trainer.User.LastName}",
                    Gym = e.Gym.Name,
                    GymId = e.GymId,
                    StartDate = e.StartDate.ToString("dd/MM/yyyy HH:mm"),
                    EndDate = e.EndDate.ToString("dd/MM/yyyy HH:mm"),
                })
                .ToList();

            var vm = new TrainerEventsVM
            {
                TrainerName = $"{trainer.User.FirstName} {trainer.User.LastName}",
                Events = events
            };

            return vm;
        }

        public async Task<IEnumerable<SelectListItem>> GetAllGymsForDropdownAsync()
        {
            return await dbContext.Gym
                .Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.Name })
                .ToListAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetAllSpecialtiesForDropdownAsync()
        {
            return await dbContext.Specialties
                .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name })
                .ToListAsync();
        }
    }
    
}
