using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Web.ViewModels.Gym
{
    public class DeleteGymVM
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public bool IsAdmin { get; set; }
    }
}
