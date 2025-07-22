using FitnessPlatform.Web.ViewModels.Trainer;
using FitnessPlatform.Web.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Services.Core.Contracts
{
    public interface ITrainerService
    {

        Task<IEnumerable<TrainerVM>> GetAllTrainersAsync(bool isAdmin);

        Task<TrainerDetailsVM> GetTrainerDetailsAsync(int userId, bool isAdmin);

        Task RemoveTrainer(int trainersId, bool isAdmin);

        
    }
}
