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
        //--------------------------------------------------------------
        //The are seeded data for gyms,users and admin BUT DONT HAVE FOR THE TRAINERS AND EVENTS.It is necessary to make a trainer from users with 
        //MAKE A TRAINER BUTTON FROM USER MANU AND AFTER THAT YOU CAN MAKE A EVENT.
        //------------------------------------------------------------------


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
        public static async Task SeedSampleUsersAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles = { "Admin", "Trainer", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var sampleUsers = new List<(string Email, string Password, string Role, ApplicationUser User)>
            {
                (
                    "peter.dimitrov@elitegym.com",
                    "User123!",
                    "User",
                    new ApplicationUser
                    {
                        UserName = "peter.dimitrov@elitegym.com",
                        Email = "peter.dimitrov@elitegym.com",
                        EmailConfirmed = true,
                        FirstName = "Peter",
                        LastName = "Dimitrov",
                        Gender = "Male",
                        PhoneNumber = "0888123001",
                        DateOfBirth = new DateTime(1990, 5, 14),
                        HeightCm = 180,
                        WeightKg = 82
                    }
                ),
                (
                    "maria.ilieva@elitegym.com",
                    "User123!",
                    "User",
                    new ApplicationUser
                    {
                        UserName = "maria.ilieva@elitegym.com",
                        Email = "maria.ilieva@elitegym.com",
                        EmailConfirmed = true,
                        FirstName = "Maria",
                        LastName = "Ilieva",
                        Gender = "Female",
                        PhoneNumber = "0888123002",
                        DateOfBirth = new DateTime(1995, 8, 22),
                        HeightCm = 165,
                        WeightKg = 58
                    }
                ),
                (
                    "ivan.georgiev@elitegym.com",
                    "User123!",
                    "User",
                    new ApplicationUser
                    {
                        UserName = "ivan.georgiev@elitegym.com",
                        Email = "ivan.georgiev@elitegym.com",
                        EmailConfirmed = true,
                        FirstName = "Ivan",
                        LastName = "Georgiev",
                        Gender = "Male",
                        PhoneNumber = "0888123003",
                        DateOfBirth = new DateTime(1988, 11, 5),
                        HeightCm = 175,
                        WeightKg = 76
                    }
                ),
                (
                    "elena.stoyanova@elitegym.com",
                    "User123!",
                    "User",
                    new ApplicationUser
                    {
                        UserName = "elena.stoyanova@elitegym.com",
                        Email = "elena.stoyanova@elitegym.com",
                        EmailConfirmed = true,
                        FirstName = "Elena",
                        LastName = "Stoyanova",
                        Gender = "Female",
                        PhoneNumber = "0888123004",
                        DateOfBirth = new DateTime(1993, 2, 10),
                        HeightCm = 170,
                        WeightKg = 62
                    }
                ),
                (
                    "nikolay.popov@elitegym.com",
                    "User123!",
                    "User",
                    new ApplicationUser
                    {
                        UserName = "nikolay.popov@elitegym.com",
                        Email = "nikolay.popov@elitegym.com",
                        EmailConfirmed = true,
                        FirstName = "Nikolay",
                        LastName = "Popov",
                        Gender = "Male",
                        PhoneNumber = "0888123005",
                        DateOfBirth = new DateTime(1985, 7, 18),
                        HeightCm = 185,
                        WeightKg = 88
                    }
                ),
                (
                    "diana.markova@elitegym.com",
                    "User123!",
                    "User",
                    new ApplicationUser
                    {
                        UserName = "diana.markova@elitegym.com",
                        Email = "diana.markova@elitegym.com",
                        EmailConfirmed = true,
                        FirstName = "Diana",
                        LastName = "Markova",
                        Gender = "Female",
                        PhoneNumber = "0888123006",
                        DateOfBirth = new DateTime(1990, 10, 30),
                        HeightCm = 168,
                        WeightKg = 59
                    }
                ),
                (
                    "georgi.kolev@elitegym.com",
                    "User123!",
                    "User",
                    new ApplicationUser
                    {
                        UserName = "georgi.kolev@elitegym.com",
                        Email = "georgi.kolev@elitegym.com",
                        EmailConfirmed = true,
                        FirstName = "Georgi",
                        LastName = "Kolev",
                        Gender = "Male",
                        PhoneNumber = "0888123007",
                        DateOfBirth = new DateTime(1996, 9, 12),
                        HeightCm = 177,
                        WeightKg = 79
                    }
                ),
                (
                "teodora.ivanova@elitegym.com",
                "User123!",
                "User",
                new ApplicationUser
                {
                    UserName = "teodora.ivanova@elitegym.com",
                    Email = "teodora.ivanova@elitegym.com",
                    EmailConfirmed = true,
                    FirstName = "Teodora",
                    LastName = "Ivanova",
                    Gender = "Female",
                    PhoneNumber = "0888123008",
                    DateOfBirth = new DateTime(1992, 3, 15),
                    HeightCm = 160,
                    WeightKg = 55
                }
            ),
            (
                "stefan.nikolov@elitegym.com",
                "User123!",
                "User",
                new ApplicationUser
                {
                    UserName = "stefan.nikolov@elitegym.com",
                    Email = "stefan.nikolov@elitegym.com",
                    EmailConfirmed = true,
                    FirstName = "Stefan",
                    LastName = "Nikolov",
                    Gender = "Male",
                    PhoneNumber = "0888123009",
                    DateOfBirth = new DateTime(1991, 6, 8),
                    HeightCm = 182,
                    WeightKg = 86
                }
            ),
            (
                "victoria.pavlova@elitegym.com",
                "User123!",
                "User",
                new ApplicationUser
                {
                    UserName = "victoria.pavlova@elitegym.com",
                    Email = "victoria.pavlova@elitegym.com",
                    EmailConfirmed = true,
                    FirstName = "Victoria",
                    LastName = "Pavlova",
                    Gender = "Female",
                    PhoneNumber = "0888123010",
                    DateOfBirth = new DateTime(1997, 12, 1),
                    HeightCm = 172,
                    WeightKg = 63
                }
            ),
            (
                "kiril.stanchev@elitegym.com",
                "User123!",
                "User",
                new ApplicationUser
                {
                    UserName = "kiril.stanchev@elitegym.com",
                    Email = "kiril.stanchev@elitegym.com",
                    EmailConfirmed = true,
                    FirstName = "Kiril",
                    LastName = "Stanchev",
                    Gender = "Male",
                    PhoneNumber = "0888123011",
                    DateOfBirth = new DateTime(1983, 1, 25),
                    HeightCm = 178,
                    WeightKg = 84
                }
            ),
            (
                "kameliya.marinova@elitegym.com",
                "User123!",
                "User",
                new ApplicationUser
                {
                    UserName = "kameliya.marinova@elitegym.com",
                    Email = "kameliya.marinova@elitegym.com",
                    EmailConfirmed = true,
                    FirstName = "Kameliya",
                    LastName = "Marinova",
                    Gender = "Female",
                    PhoneNumber = "0888123012",
                    DateOfBirth = new DateTime(1994, 4, 4),
                    HeightCm = 169,
                    WeightKg = 60
                }
            ),
            (
                "aleksandar.hristov@elitegym.com",
                "User123!",
                "User",
                new ApplicationUser
                {
                    UserName = "aleksandar.hristov@elitegym.com",
                    Email = "aleksandar.hristov@elitegym.com",
                    EmailConfirmed = true,
                    FirstName = "Aleksandar",
                    LastName = "Hristov",
                    Gender = "Male",
                    PhoneNumber = "0888123013",
                    DateOfBirth = new DateTime(1993, 9, 30),
                    HeightCm = 181,
                    WeightKg = 78
                }
            ),
            (
                "desislava.todorova@elitegym.com",
                "User123!",
                "User",
                new ApplicationUser
                {
                    UserName = "desislava.todorova@elitegym.com",
                    Email = "desislava.todorova@elitegym.com",
                    EmailConfirmed = true,
                    FirstName = "Desislava",
                    LastName = "Todorova",
                    Gender = "Female",
                    PhoneNumber = "0888123014",
                    DateOfBirth = new DateTime(1989, 11, 11),
                    HeightCm = 167,
                    WeightKg = 59
                }
            ),
            (
                "martin.petkov@elitegym.com",
                "User123!",
                "User",
                new ApplicationUser
                {
                    UserName = "martin.petkov@elitegym.com",
                    Email = "martin.petkov@elitegym.com",
                    EmailConfirmed = true,
                    FirstName = "Martin",
                    LastName = "Petkov",
                    Gender = "Male",
                    PhoneNumber = "0888123015",
                    DateOfBirth = new DateTime(1986, 2, 6),
                    HeightCm = 183,
                    WeightKg = 90
                }
            )
            };

            foreach (var (email, password, role, user) in sampleUsers)
            {
                var existingUser = await userManager.FindByEmailAsync(email);
                if (existingUser == null)
                {
                    var result = await userManager.CreateAsync(user, password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, role);
                    }
                }
            }
        }
        public static async Task SeedGymsAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<FitnessDbContext>();

            if (await dbContext.Gym.AnyAsync())
                return;

            var gyms = new List<Gym>
    {
        new Gym
        {
            Name = "Pulse Fitness",
            Location = "Sofia, Studentski Grad, Blvd. 8th December 15",
            Description = "Titan Gym offers modern fitness equipment, group classes, and a relaxing SPA zone.",
            Images = new List<GymImage>
            {
                new GymImage
                {
                    ImageUrl = "https://pulsefit.bg/wp-content/uploads/s-logo-scaled.jpg",
                    IsPrimary = true
                },
                new GymImage
                {
                    ImageUrl = "https://pulsefit.bg/wp-content/uploads/dsc_6098-scaled.jpg",
                    IsPrimary = false
                }
            }
        },
        new Gym
        {
            Name = "WestGym",
            Location = "Plovdiv, Central District, Vasil Aprilov Blvd. 93",
            Description = "FitZone provides a full-body workout experience with certified trainers and flexible hours.",
            Images = new List<GymImage>
            {
                new GymImage
                {
                    ImageUrl = "https://fitness.west-gym.com/wp-content/uploads/2022/09/west-gym-plovdiv-5.jpg",
                    IsPrimary = true
                }
            }
        },
        new Gym
        {
            Name = "Pulse Active",
            Location = "Varna, Sea Garden, Al. Stamboliyski 1",
            Description = "Pulse Active is located near the beach and offers outdoor classes, cardio zone, and yoga.",
            Images = new List<GymImage>
            {
                new GymImage
                {
                    ImageUrl = "https://www.fitness.bg/upload/nav/full/nav_z1MmOR.JPG",
                    IsPrimary = true
                }
            }
        },
        new Gym
        {
            Name = "Iron Temple",
            Location = "Burgas, Slaveykov District, Blvd. Stefan Stambolov 120",
            Description = "Iron Temple is a strength-focused gym equipped for serious lifters and athletes.",
            Images = new List<GymImage>
            {
                new GymImage
                {
                    ImageUrl = "https://irongym.hr/wp-content/uploads/2020/01/02-1.jpg",
                    IsPrimary = true
                }
            }
        },
        new Gym
        {
            Name = "Energy Fitness",
            Location = "Ruse, Center, Tsar Osvoboditel Blvd. 56",
            Description = "Energy Fitness offers affordable memberships and functional training equipment for all levels.",
            Images = new List<GymImage>
            {
                new GymImage
                {
                    ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRPyX3jSHuTwwb7w6EAphjiprJ7CB09n4xPfQ&s",
                    IsPrimary = true
                }
            }
        }
    };

            dbContext.Gym.AddRange(gyms);
            await dbContext.SaveChangesAsync();
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
                    new Specialty { Name = "Cardio", Description = "Endurance training and heart health" },
                    new Specialty { Name = "Strength Training", Description = "Building muscle mass" },
                    new Specialty { Name = "Yoga", Description = "Flexibility and mindfulness" },
                    new Specialty { Name = "Pilates", Description = "Core strength, posture, and flexibility" },
                    new Specialty { Name = "CrossFit", Description = "High-intensity functional training" },
                    new Specialty { Name = "HIIT", Description = "High-Intensity Interval Training for fat burning" }
                };

                dbContext.Specialties.AddRange(specialties);
                await dbContext.SaveChangesAsync();
            }
        }

    }
}
