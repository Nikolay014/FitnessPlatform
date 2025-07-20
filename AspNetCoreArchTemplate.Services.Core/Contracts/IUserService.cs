using FitnessPlatform.Web.ViewModels.Gym;
using FitnessPlatform.Web.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Services.Core.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<UserVM>> GetAllUsersAsync(bool isAdmin);
        Task<UserDetailsVM> GetUserDetailsAsync(string userId, bool isAdmin);

        Task<CreateTrainerUserVM> GetUserForTrainerAsync(string userId, bool isAdmin);

        Task CreateTrainerAsync(CreateTrainerUserVM model);
    }
}
