using AspNetCoreArchTemplate.Data;
using FitnessPlatform.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Data.Seeding
{
    public static class DataSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            // Роли
            string[] roles = { "Admin", "Trainer", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // Админ акаунт
            string adminEmail = "admin@elitegym.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = "admin@elitegym.com",
                    Email = "admin@elitegym.com",
                    EmailConfirmed = true,
                    FirstName = "System",
                    LastName = "Admin",
                    Gender = "Other",
                    PhoneNumber = "0000000000",
                    DateOfBirth = DateTime.UtcNow.AddYears(-30),
                    HeightCm = 180,
                    WeightKg = 75
                };

                var result = await userManager.CreateAsync(newAdmin, "Admin123!");

                if (result.Succeeded)
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
            }
        }
        public static async Task SeedSubscriptionPlansAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<FitnessDbContext>();

            if (!await dbContext.SubscriptionPlans.AnyAsync())
            {
                var plans = new List<SubscriptionPlan>
                {
                    new SubscriptionPlan
                    {
                        Name = "1 Month - Standard Plan",
                        DurationInDays = 30,
                        Price = 45,
                        Description = "Unlimited gym access, 1 SPA visit/week, 2 group sessions/month"
                    },
                    new SubscriptionPlan
                    {
                        Name = "1 Year - Standard Plan",
                        DurationInDays = 365,
                        Price = 399,
                        Description = "Unlimited gym access, unlimited SPA, 4 group sessions/month"
                    }
                };

                dbContext.SubscriptionPlans.AddRange(plans);
                await dbContext.SaveChangesAsync();
            }
        }
        public static async Task SeedSpecialtiesAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<FitnessDbContext>();

            if (!await dbContext.Specialties.AnyAsync())
            {
                var specialties = new List<Specialty>
                {
                    new Specialty { Id = 1, Name = "Cardio", Description = "Endurance training and heart health" },
                    new Specialty { Id = 2, Name = "Strength Training", Description = "Building muscle mass" },
                    new Specialty { Id = 3, Name = "Yoga", Description = "Flexibility and mindfulness" },
                    new Specialty { Id = 4, Name = "Pilates", Description = "Core strength, posture, and flexibility" },
                    new Specialty { Id = 5, Name = "CrossFit", Description = "High-intensity functional training" },
                    new Specialty { Id = 6, Name = "HIIT", Description = "High-Intensity Interval Training for fat burning" }
                };

                dbContext.Specialties.AddRange(specialties);
                await dbContext.SaveChangesAsync();
            }
        }

    }
}
