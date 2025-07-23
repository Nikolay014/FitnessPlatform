using AspNetCoreArchTemplate.Data;
using FitnessPlatform.Data.Models;
using FitnessPlatform.Services.Core.Contracts;
using FitnessPlatform.Web.ViewModels.Event;
using FitnessPlatform.Web.ViewModels.Gym;
using FitnessPlatform.Web.ViewModels.User;
using FitnessPlatform.Web.Views.Event;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Services.Core
{
    public class EventService:IEventService
    {
        private readonly FitnessDbContext dbContext;

        public EventService(FitnessDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task CreateEventAsync(CreateEventVM model)
        {
            Event eventVM = new Event
            {
                Title = model.Title,
                Image = model.Image,
                GymId = model.GymId,
                TrainerId = model.TrainerId,
                StartDate = DateTime.Parse($"{model.StartDate} {model.StartTime}"),
                EndDate = DateTime.Parse($"{model.EndDate} {model.EndTime}"),
            };
            dbContext.Events.Add(eventVM);

            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<EventVM>> GetEventAsync(string? userId)
        {
            var eventVM = await dbContext.Events
               .Select(e => new EventVM
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
               .ToListAsync();

            return eventVM;
        }

        public Task<EventDetailsVM> GetEventDetailsAsync(int eventId, string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<CreateEventVM> GetGymsAndTrainersAsync()
        {
            var trainers = await dbContext.Trainers
        .Include(t => t.User)
        .Select(t => new TrainerDropdownVM
        {
            Id = t.Id,
            FullName = t.User.FirstName + " " + t.User.LastName
        })
        .ToListAsync();

            var gyms = await dbContext.Gym.ToListAsync();

            return new CreateEventVM
            {
                Gyms = gyms,
                TrainersDropdown = trainers
            };
        }
    }
}
