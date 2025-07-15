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
    }
}
