using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Web.ViewModels.Gym
{
    public class GymDetailsVM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public string? Description { get; set; }
        

        public bool IsUserSubscribed { get; set; } // за потребител
        public DateTime? SubscriptionEndDate { get; set; }
        public bool IsAdmin { get; set; }

        public List<string> Images { get; set; } = new List<string>();
    }
}
