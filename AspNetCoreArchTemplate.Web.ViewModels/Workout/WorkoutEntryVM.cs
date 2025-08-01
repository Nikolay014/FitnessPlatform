using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Web.ViewModels.Workout
{
    public class WorkoutEntryVM
    {
        [Required]
        [StringLength(100)]
        public string Exercise { get; set; }

        public int? Repetitions { get; set; }

        public int? Sets { get; set; }

        public double? DistanceKm { get; set; }

        public double? WeightKg { get; set; }
    }
}
