using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Web.ViewModels.Gym
{
    public class GymVM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public string? PrimaryImage { get; set; }
    }
}
