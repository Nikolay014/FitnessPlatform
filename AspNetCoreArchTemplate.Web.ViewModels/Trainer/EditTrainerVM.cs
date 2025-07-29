using FitnessPlatform.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Web.ViewModels.Trainer
{
    public class EditTrainerVM
    {
        public int TrainerId { get; set; }

        [Required]
        public int GymId { get; set; }

        [Required, MaxLength(200)]
        public string Image { get; set; }
        [Required]
        public int SpecialtyId { get; set; }

        public List<Specialty> Specialties { get; set; } = new List<Specialty>();

        public ICollection<FitnessPlatform.Data.Models.Gym> Gyms = new HashSet<FitnessPlatform.Data.Models.Gym>();
    }
}
