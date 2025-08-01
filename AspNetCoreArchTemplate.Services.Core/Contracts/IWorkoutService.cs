using FitnessPlatform.Web.ViewModels.Workout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Services.Core.Contracts
{
    public interface IWorkoutService
    {
        Task AddWorkoutSessionAsync(string userId, WorkoutSessionVM model);
        Task DeleteWorkoutEntryAsync(int id,string currentUserId);
    }
}
