using FitnessPlatform.Web.ViewModels.Event;
using FitnessPlatform.Web.ViewModels.Gym;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Services.Core.Contracts
{
    public interface IEventService
    {
        Task<IEnumerable<EventVM>> GetEventAsync(string? userId);

        Task CreateEventAsync(CreateEventVM model);

        Task<CreateEventVM> GetGymsAndTrainersAsync();

        Task<EventDetailsVM> GetEventDetailsAsync(int eventId,string userId,bool isAdmin);

        
    }
}
