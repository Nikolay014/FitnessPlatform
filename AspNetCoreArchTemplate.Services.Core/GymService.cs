using AspNetCoreArchTemplate.Data;
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
    }
}
