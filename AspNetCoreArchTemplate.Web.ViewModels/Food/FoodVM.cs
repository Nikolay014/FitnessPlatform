using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FitnessPlatform.GCommon.ValidationConstraints.FoodConst;

namespace FitnessPlatform.Web.ViewModels.Food
{
    public class FoodVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a description of the meal.")]
        [StringLength(DescriptionMaxLength, ErrorMessage = "Description cannot exceed 200 characters.")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Please enter calories.")]
        [Range(1, 5000, ErrorMessage = "Calories must be between 1 and 5000.")]
        public int Calories { get; set; }

        [Display(Name = "Image (optional)")]
        public string? Image { get; set; }
    }
}
