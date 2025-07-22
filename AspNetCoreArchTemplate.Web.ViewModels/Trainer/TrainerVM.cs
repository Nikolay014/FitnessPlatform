using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Web.ViewModels.Trainer
{
    public class TrainerVM
    {
        public int TrainerId { get; set; }

        public string Gym  { get; set; }

        public string FullName { get; set; }

        public string Image { get; set; }

        public string Specialty { get; set; }

        public string PhoneNumber { get; set; }
    }
}
