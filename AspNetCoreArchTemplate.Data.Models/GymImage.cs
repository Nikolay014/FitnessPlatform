using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static FitnessPlatform.GCommon.ValidationConstraints.GymImageConst;

namespace FitnessPlatform.Data.Models
{
    public class GymImage
    {
        [Key]
        [Comment("Key of the Gym Image")]
        public int Id { get; set; }

        [Required]
        [Comment("Gym Id that the image belongs to")]
        public int GymId { get; set; }
        [ForeignKey("GymId")]
        public virtual Gym Gym { get; set; }

        [Required]
        [MaxLength  (ImageURLMaxLength)]
        [Comment("URL of the Gym Image")]
        public string ImageUrl { get; set; } 

        public bool IsPrimary { get; set; } 

        
    }
}