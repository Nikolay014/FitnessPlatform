using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Web.ViewModels.Gym
{
    public class GymImageVM
    {
        public int Id { get; set; }

        public int GymId { get; set; }

        public string ImageUrl { get; set; }

        public bool IsPrimary { get; set; }
    }
}
