using FitnessPlatform.Web.ViewModels.Trainer;
using FitnessPlatform.Web.ViewModels.User;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Services.Core.Contracts
{
    public interface ITrainerService
    {

        Task<PaginatedTrainersVM> GetAllTrainersAsync(int? gymId, int? specialtyId, int page, bool isAdmin);

        Task<TrainerDetailsVM> GetTrainerDetailsAsync(int trainerId,string userId, bool isAdmin);

        Task RemoveTrainer(int trainersId, bool isAdmin);

        Task UserSubscribeToTrainer(int trainerId, string userId);

        Task UnUserSubscribeToTrainer(int trainerId, string userId);
        Task<EditTrainerVM> GetTrainerForUpdate(int id, bool isAdmin);
        Task UpdateTrainerAsync(EditTrainerVM editTrainerVM, bool isAdmin);
        Task<TrainerClientsVM> GetClientsAsync(int id,string? userid);
        Task<TrainerEventsVM> GetEventsAsync(int id, string? userId);

        Task<IEnumerable<SelectListItem>> GetAllGymsForDropdownAsync();
        Task<IEnumerable<SelectListItem>> GetAllSpecialtiesForDropdownAsync();
    }
}
