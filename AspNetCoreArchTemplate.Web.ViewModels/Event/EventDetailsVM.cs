using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Web.ViewModels.Event
{
    public class EventDetailsVM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }


        public string Image { get; set; }

        public string TrainerId { get; set; }

        public string TrainerFullName { get; set; }

        public string GymName { get; set; }

        public bool IsUserSubscribed { get; set; } // за потребител
        public bool IsAdmin { get; set; }
    }
}
