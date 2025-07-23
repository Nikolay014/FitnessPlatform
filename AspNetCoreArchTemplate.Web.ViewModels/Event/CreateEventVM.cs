using FitnessPlatform.Web.Views.Event;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Web.ViewModels.Event
{
    public class CreateEventVM
    {
        [Required, MaxLength(200)]
        public string Title { get; set; }

        [Required, MaxLength(300)]
        public string Image { get; set; }
        [Required]
        public int GymId { get; set; }


        [Required]
        public int TrainerId { get; set; }

        public string StartDate { get; set; }
        public string StartTime { get; set; }

        public string EndDate { get; set; }
        public string EndTime { get; set; }

        public List<FitnessPlatform.Data.Models.Gym> Gyms { get; set; } = new List<FitnessPlatform.Data.Models.Gym>();

        public List<TrainerDropdownVM> TrainersDropdown { get; set; } = new List<TrainerDropdownVM>();
    }
}
