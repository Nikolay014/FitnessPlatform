using FitnessPlatform.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Web.ViewModels.Gym
{
    public class SubscribeGymVM
    {
        public int GymId { get; set; }
        public int SelectedPlanId { get; set; }

        public IEnumerable<SubscriptionPlan> AvailablePlans { get; set; }
    }
}
