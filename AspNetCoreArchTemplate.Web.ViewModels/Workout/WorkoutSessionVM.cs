using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Web.ViewModels.Workout
{
    public class WorkoutSessionVM
    {
        public List<WorkoutEntryVM> Entries { get; set; } = new List<WorkoutEntryVM>();
    }
}
