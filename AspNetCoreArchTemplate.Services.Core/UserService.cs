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


        public async Task<IEnumerable<UserVM>>GetAllUsersAsync(bool isAdmin)
        {
            var usersInRole = await userManager.GetUsersInRoleAsync("User");
            return usersInRole.Select(u => new UserVM
            {
                Id = u.Id,
                FullName = $"{u.FirstName} {u.LastName}",
                Gender = u.Gender,
                PhoneNumber = u.PhoneNumber,
                Image = u.ImageUrl,
            });

            
        }

        public async Task<UserDetailsVM> GetUserDetailsAsync(string userId, bool isAdmin)
        {
            if(!isAdmin || string.IsNullOrEmpty(userId))
            {
                return null; // If not admin and no userId provided, return null
            }
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
    }
}
