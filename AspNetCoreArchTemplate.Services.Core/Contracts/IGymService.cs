using FitnessPlatform.Web.ViewModels.Gym;
using FitnessPlatform.Web.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Services.Core.Contracts
{
    public interface IGymService
    {
        Task<IEnumerable<GymVM>> GetGymsAsync(string? userId);

        Task CreateGymAsync(CreateGymVM model);

        Task<GymDetailsVM> GetGymDetailsAsync(int gymId, string userId, bool isAdmin);

        Task<DeleteGymVM> GetGymForDeleteAsync(int id, string? userId,bool isAdmin);

        Task DeleteGymAsync(int gymId);

        Task<EditGymVM> GetGymForEditAsync(int id);

        Task EditGymAsync(EditGymVM model);

        Task<SubscribeGymVM> GetSubscriptionPlansAsync(int id);

        Task SubscribeToGymAsync(int gymId, string userId, int planId);

        Task<GymWithSubscribersVM> GetSubscribedUsersAsync(int id, string userId);

        Task<GymWithTrainersVM> GetGymTrainersAsync(int id, string userId);

    }
}
