using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Web.ViewModels.Trainer
{
    public class TrainerEventVM
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Image { get; set; }

        public int GymId { get; set; }

        public string Gym { get; set; }

        public string Trainer { get; set; }

        public int TrainerId { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }
    }
}
