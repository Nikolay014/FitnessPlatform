using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FitnessPlatform.Data.Models
{
    [Comment("Represents working hours for a gym on a specific day of the week.")]
    public class GymWorkingHours
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Comment("The ID of the gym this working hour entry belongs to.")]

        public int GymId { get; set; }

        [ForeignKey("GymId")]
        public Gym Gym { get; set; } 

        [Required]
        [Comment("Day of the week (0 = Sunday, 6 = Saturday)")]
        public DayOfWeek DayOfWeek { get; set; }

        [Required]
        [Comment("Time the gym opens on the specified day")]
        public TimeSpan OpenTime { get; set; }

        [Required]
        [Comment("Time the gym closes on the specified day")]
        public TimeSpan CloseTime { get; set; }

        
    }
}