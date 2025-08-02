using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FitnessPlatform.Web.ViewModels.Trainer
{
    public class PaginatedTrainersVM
    {
        public int? SelectedGymId { get; set; }
        public int? SelectedSpecialtyId { get; set; }

        public IEnumerable<SelectListItem> Gyms { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Specialties { get; set; } = new List<SelectListItem>();

        public IEnumerable<TrainerVM> Trainers { get; set; } = new List<TrainerVM>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
