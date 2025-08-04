using AspNetCoreArchTemplate.Data;
using FitnessPlatform.Data.Models;
using FitnessPlatform.Services.Core.Contracts;
using FitnessPlatform.Web.ViewModels.Gym;
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
    public class UserService:IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly FitnessDbContext dbContext;
        public UserService(FitnessDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        

        public async Task<PaginatedUserVM>GetAllUsersAsync(int page, bool isAdmin)
        {
            const int PageSize = 3; 

            var usersInRole = await userManager.GetUsersInRoleAsync("User");

            var userVMs = usersInRole
                .Select(u => new UserVM
                {
                    Id = u.Id,
                    FullName = $"{u.FirstName} {u.LastName}",
                    Gender = u.Gender,
                    PhoneNumber = u.PhoneNumber,
                    Image = u.ImageUrl,
                })
                .ToList();

            var totalUsers = userVMs.Count;
            var totalPages = (int)Math.Ceiling(totalUsers / (double)PageSize);

            var usersForCurrentPage = userVMs
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            return new PaginatedUserVM
            {
                Users = usersForCurrentPage,
                CurrentPage = page,
                TotalPages = totalPages
            };


        }

        public async Task<UserDetailsVM> GetUserDetailsAsync(string userId)
        {
            
            var user = await userManager.FindByIdAsync(userId);

            
            
                UserDetailsVM details = new UserDetailsVM 
                { 
                   UserId = userId,
                   FirstName = user.FirstName,
                   LastName = user.LastName,
                   Gender = user.Gender,
                   PhoneNumber = user.PhoneNumber,
                   DateOfBirth = user.DateOfBirth,
                   HeightCm = user.HeightCm,
                   WeightKg = user.WeightKg,
                   ImageUrl = user.ImageUrl,
                   

                
                };
               

            
            return details;

        }
        public async Task CreateTrainerAsync(CreateTrainerUserVM model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);
            if (user != null)
            {
                var roles = await userManager.GetRolesAsync(user);
                await userManager.RemoveFromRolesAsync(user, roles);
                
                var result = await userManager.AddToRoleAsync(user, "Trainer");
                if (!result.Succeeded)
                    throw new InvalidOperationException("Failed to add user to Trainer role.");
            }
            Trainer trainer = new Trainer
            {
                UserId = model.UserId,
                GymId = model.GymId,
                SpecialtyId = model.SpecialtyId,
                TrainerImage = model.Image
            };

            dbContext.Trainers.Add(trainer);
            await dbContext.SaveChangesAsync();
        }
        public async Task<CreateTrainerUserVM> GetUserForTrainerAsync(string userId, bool isAdmin)
        {

            if (!isAdmin || string.IsNullOrEmpty(userId))
            {
                return null; // If not admin and no userId provided, return null
            }

            
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }
            CreateTrainerUserVM createTrainerUserVM = new CreateTrainerUserVM
            {
                UserId = userId,
                Image = user.ImageUrl,
                Gyms = dbContext.Gym.ToList(),
                Specialties = dbContext.Specialties.ToList(),
            };
            return createTrainerUserVM;

        }

    }
}
