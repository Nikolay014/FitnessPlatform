using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Web.ViewModels.Gym
{
    public class GymWithTrainersVM
    {
        public string GymName { get; set; }
        public List<GymTrainersVM> Trainers { get; set; }
    }
}
