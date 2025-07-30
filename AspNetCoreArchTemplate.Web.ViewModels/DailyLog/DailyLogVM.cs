using FitnessPlatform.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Web.ViewModels.DailyLog
{
    public class DailyLogVM
    {
        public int DailyLogId { get; set; }
        public DateTime Date { get; set; }

        public List<FoodVM> Foods { get; set; } = new();
        public List<WorkoutSessionVM> Workouts { get; set; } = new();
    }
}
