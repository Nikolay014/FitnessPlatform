using AspNetCoreArchTemplate.Data;
using FitnessPlatform.Data.Models;
using FitnessPlatform.Services.Core.Contracts;
using FitnessPlatform.Web.ViewModels.DailyLog;
using FitnessPlatform.Web.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FitnessPlatform.Services.Core
{
    public class DailyLogService : IDailyLogService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly FitnessDbContext dbContext;
        public DailyLogService(FitnessDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<DailyLogVM> GetUserLogAsync(string userId, DateTime date)
        {
            var log = await dbContext.DailyLog
                .Include(l => l.Foods)
                .Include(l => l.WorkoutSessions)
                    .ThenInclude(ws => ws.Entries)
                .FirstOrDefaultAsync(l => l.UserId == userId && l.Date.Date == date.Date);

            if (log == null)
            {
                log = new DailyLog
                {
                    UserId = userId,
                    Date = date,
                };
                dbContext.DailyLog.Add(log);
                await dbContext.SaveChangesAsync();
            }

            return new DailyLogVM
            {
                DailyLogId = log.Id,
                Date = date,
                Foods = log.Foods.Select(f => new FoodVM
                {
                    Image = f.Image,
                    Description = f.Description,
                    Calories = f.Calories
                }).ToList(),
                Workouts = log.WorkoutSessions.Select(ws => new WorkoutSessionVM
                {
                    Notes = ws.Notes,
                    Entries = ws.Entries.Select(e => new WorkoutEntryVM
                    {
                        Exercise = e.Exercise,
                        Repetitions = e.Repetitions,
                        Sets = e.Sets,
                        DistanceKm = e.DistanceKm,
                        WeightKg = e.WeightKg
                    }).ToList()
                }).ToList()
            };
        }
    }
}
