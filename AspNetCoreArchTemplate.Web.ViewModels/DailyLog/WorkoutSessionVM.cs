using FitnessPlatform.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Web.ViewModels.DailyLog
{
    public class WorkoutSessionVM
    {
        public string Notes { get; set; }
        public List<WorkoutEntryVM> Entries { get; set; }
    }
}
