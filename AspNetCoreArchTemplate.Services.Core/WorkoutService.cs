using AspNetCoreArchTemplate.Data;
using FitnessPlatform.Data.Models;
using FitnessPlatform.Services.Core.Contracts;
using FitnessPlatform.Web.ViewModels.Workout;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Services.Core
{
    public class WorkoutService:IWorkoutService
    {
        private readonly FitnessDbContext dbContext;

        public WorkoutService(FitnessDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task AddWorkoutSessionAsync(string userId, WorkoutSessionVM model)
        {
            var dailyLog = await dbContext.DailyLog
                .Include(d => d.WorkoutSessions)
                .FirstOrDefaultAsync(d => d.UserId == userId && d.Date.Date == DateTime.UtcNow.Date);

            if (dailyLog == null)
            {
                dailyLog = new DailyLog
                {
                    UserId = userId,
                    Date = DateTime.UtcNow,
                    WorkoutSessions = new List<WorkoutSession>()
                };

                dbContext.DailyLog.Add(dailyLog);
                await dbContext.SaveChangesAsync();
            }

            var session = new WorkoutSession
            {
                DailyLogId = dailyLog.Id,
                Entries = model.Entries.Select(e => new WorkoutEntry
                {
                    Exercise = e.Exercise,
                    Repetitions = e.Repetitions,
                    Sets = e.Sets,
                    DistanceKm = e.DistanceKm,
                    WeightKg = e.WeightKg
                }).ToList()
            };

            dbContext.WorkoutSessions.Add(session);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteWorkoutEntryAsync(int id, string currentUserId)
        {
            var entry = await dbContext.WorkoutEntries
                .Include(w=>w.WorkoutSession)
                .ThenInclude(w=>w.DailyLog)
                .FirstOrDefaultAsync(w=>w.Id == id);
            if (entry != null && entry.WorkoutSession.DailyLog.UserId == currentUserId)
            {
                dbContext.WorkoutEntries.Remove(entry);

                // Ако искаш да премахнеш и WorkoutSession, когато няма останали entries:
                var session = await dbContext.WorkoutSessions
                    .Include(s => s.Entries)
                    .FirstOrDefaultAsync(s => s.Id == entry.WorkoutSessionId);

                await dbContext.SaveChangesAsync();

                if (session != null && !session.Entries.Any())
                {
                    dbContext.WorkoutSessions.Remove(session);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
