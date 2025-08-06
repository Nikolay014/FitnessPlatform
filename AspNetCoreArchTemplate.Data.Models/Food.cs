using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FitnessPlatform.GCommon.ValidationConstraints.FoodConst;

namespace FitnessPlatform.Data.Models
{
    public class Food
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Comment("Id of the collection food for current day")]
        public int DailyLogId { get; set; }

        [ForeignKey("DailyLogId")]
        public virtual DailyLog DailyLog { get; set; } = null!;

        [Comment("Image of the food item")]
        public string? Image { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;


        [Required]
        public int Calories { get; set; }
    }
}
