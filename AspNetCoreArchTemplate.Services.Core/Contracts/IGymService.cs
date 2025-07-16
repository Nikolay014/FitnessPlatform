using FitnessPlatform.Web.ViewModels.Gym;
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

        Task<SubscribeGymVM> GetSubscriptionPlans();

    }
}
