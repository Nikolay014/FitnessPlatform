using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Web.ViewModels.DailyLog
{
    public class WorkoutEntryVM
    {
        public int Id { get; set; }
        public string Exercise { get; set; }
        public int? Repetitions { get; set; }
        public int? Sets { get; set; }
        public double? DistanceKm { get; set; }
        public double? WeightKg { get; set; }
    }
}
