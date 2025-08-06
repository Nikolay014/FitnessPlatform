using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FitnessPlatform.GCommon.ValidationConstraints.GymConst;
using static FitnessPlatform.GCommon.ValidationConstraints.GymImageConst;

namespace FitnessPlatform.Web.ViewModels.Gym
{
    public class EditGymVM
    {
        public int Id { get; set; }


        [Required]
        [MaxLength(NameMaxLength, ErrorMessage = "Name must be between 3 and 50 characters.")]
        [MinLength(NameMinLength, ErrorMessage = "Name must be between 3 and 50 characters.")]
        [Display(Name = "Gym Name")]
        public string Name { get; set; }

        [Required]
        [MaxLength(LocationMaxLength, ErrorMessage = "Location must be between 4 and 80 characters.")]
        [MinLength(LocationMinLength, ErrorMessage = "Location must be between 4 and 80 characters.")]
        [Display(Name = "Location")]
        public string Location { get; set; }


        [MaxLength(DescriptionMaxLength)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Main Image URL")]
        [MaxLength(ImageURLMaxLength)]
        [Url(ErrorMessage = "Please enter a valid URL.")]
        public string MainImageUrl { get; set; }
    }
}
