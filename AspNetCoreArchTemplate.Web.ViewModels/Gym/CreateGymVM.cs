using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Web.ViewModels.Gym
{
    public class CreateGymVM
    {
        [Required]
        [MaxLength(50)]
        [Display(Name = "Gym Name")]
        public string Name { get; set; }

        [Required]
        [MaxLength(80)]
        [Display(Name = "Location")]
        public string Location { get; set; }

        
        [MaxLength(350)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Main Image URL")]
        [MaxLength(200)]
        [Url(ErrorMessage = "Please enter a valid URL.")]
        public string MainImageUrl { get; set; }
    }
}
