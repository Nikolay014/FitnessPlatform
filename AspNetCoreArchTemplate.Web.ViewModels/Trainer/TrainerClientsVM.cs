using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Web.ViewModels.Trainer
{
    public class TrainerClientsVM
    {
        public string TrainerName { get; set; }
        public List<TrainerClientVM> Clients { get; set; }
    }
}
