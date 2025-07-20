using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Web.ViewModels.User
{
    public class CreateTrainerUserVM
    {
        public string UserId { get; set; }

        [Required]
        public int GymId { get; set; }

        [Required]
        public string Specialty { get; set; } = null!;
    }
}
