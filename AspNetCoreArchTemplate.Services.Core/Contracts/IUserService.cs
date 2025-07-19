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
    }
}
