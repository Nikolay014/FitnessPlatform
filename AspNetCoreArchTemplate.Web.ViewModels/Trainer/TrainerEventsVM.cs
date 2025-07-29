using FitnessPlatform.Web.ViewModels.Gym;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Web.ViewModels.Trainer
{
    public class TrainerEventsVM
    {
        public string TrainerName { get; set; }
        public List<TrainerEventVM> Events { get; set; }
    }
}
