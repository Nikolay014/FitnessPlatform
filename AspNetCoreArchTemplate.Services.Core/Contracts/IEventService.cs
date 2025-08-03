using FitnessPlatform.Web.ViewModels.Event;
using FitnessPlatform.Web.ViewModels.Gym;
using FitnessPlatform.Web.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Services.Core.Contracts
{
    public interface IEventService
    {
        Task<PaginatedEventsVM> GetEventAsync(int? gymId, int page,string? userId);

        Task CreateEventAsync(CreateEventVM model);

        Task<CreateEventVM> GetGymsAndTrainersAsync();

        Task<EventDetailsVM> GetEventDetailsAsync(int eventId,string userId,bool isAdmin);

        Task<EditEventVM> GetEventForEditAsync(int id);

        Task EditEventAsync(EditEventVM model);

        Task<DeleteEventVM> GetEventForDeleteAsync(int id, string? userId, bool isAdmin);

        Task DeleteEventAsync(int eventId);
        Task SubscribeEventAsync(int id, string userId);
        Task RemoveSubscriptionAsync(int id, string userId);
        Task<EventWithSubscribersVM> GetSubscribedUsersAsync(int id,string userId);
    }
}
