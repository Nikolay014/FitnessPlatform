using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Web.ViewModels.Gym
{
    public class PaginatedGymsVM
    {
        public IEnumerable<GymVM> Gyms { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string? Location { get; set; }
    }
}
