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
                Description = model.Description,
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

        public async Task<EventDetailsVM> GetEventDetailsAsync(int eventId, string userId,bool isAdmin)
        {
            var @event = await dbContext.Events
                .Include(e => e.Gym)
                .Include(e => e.Trainer)
                    .ThenInclude(t => t.User) 
                .Include(e => e.Registrations)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(e => e.Id == eventId);


            if (@event == null) throw new ArgumentException("Gym not found.");

            EventDetailsVM eventDetails = new EventDetailsVM
            {
                Id = @event.Id,
                Name = @event.Title,
                GymName = @event.Gym.Name,
                TrainerFullName = $"{@event.Trainer.User.FirstName} {@event.Trainer.User.LastName}",
                StartDate = @event.StartDate.ToString("dd/MM/yyyy HH:mm"),
                EndDate = @event.EndDate.ToString("dd/MM/yyyy HH:mm"),
                Description = @event.Description,
                Image = @event.Image,
                IsUserSubscribed = @event.Registrations.Any(s => s.UserId == userId),
                IsAdmin = isAdmin
            };
           

            return eventDetails;
        }

        public async Task<EditEventVM> GetEventForEditAsync(int id)
        {
            Event @event = await dbContext.Events
            
            .FirstOrDefaultAsync(e => e.Id == id);

            var trainers = await dbContext.Trainers
               .Include(t => t.User)
               .Select(t => new TrainerDropdownVM
               {
                   Id = t.Id,
                   FullName = t.User.FirstName + " " + t.User.LastName
               })
               .ToListAsync();

            var gyms = await dbContext.Gym.ToListAsync();

            return new EditEventVM
            {
                Id = @event.Id,
                Title = @event.Title,
                Image = @event.Image,
                Description = @event.Description,
                GymId = @event.GymId,
                TrainerId = @event.TrainerId,
                StartDate = @event.StartDate.ToString("yyyy-MM-dd"),
                StartTime = @event.StartDate.ToString("HH:mm"),
                EndDate = @event.EndDate.ToString("yyyy-MM-dd"),
                EndTime = @event.EndDate.ToString("HH:mm"),
                Gyms = gyms,
                TrainersDropdown = trainers,
            };
        }
        public async Task EditEventAsync(EditEventVM model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "Event model cannot be null.");
            }
            Event @event = dbContext.Events.FirstOrDefault(e => e.Id == model.Id);

            @event.Title = model.Title;
            @event.Image = model.Image;
            @event.Description = model.Description;
            @event.GymId = model.GymId;
            @event.TrainerId = model.TrainerId;
            @event.StartDate = DateTime.Parse($"{model.StartDate} {model.StartTime}");
            @event.EndDate = DateTime.Parse($"{model.EndDate} {model.EndTime}");



            await dbContext.SaveChangesAsync();
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
