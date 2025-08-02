using FitnessPlatform.Web.ViewModels.Trainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Web.ViewModels.User
{
    public class PaginatedUserVM
    {
        public IEnumerable<UserVM> Users { get; set; } = new List<UserVM>();

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
