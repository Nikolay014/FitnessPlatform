using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessPlatform.Data.Models
{
    [Comment("Contains a single exercise or activity performed as part of a workout session.")]
    public class WorkoutEntry
    {
        [Key]
        [Comment("Primary key of the workout entry")]
        public int Id { get; set; }

        [Required]
        [Comment("Foreign key to the workout session this entry belongs to")]
        public int WorkoutSessionId { get; set; }

        [ForeignKey("WorkoutSessionId")]
        public WorkoutSession WorkoutSession { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        [Comment("Name of the exercise (e.g. Squat, Running, Pull-ups)")]
        public string Exercise { get; set; } = null!;

        
        [Comment("Number of repetitions per set (if applicable)")]
        public int? Repetitions { get; set; }

        
        [Comment("Number of sets (if applicable)")]
        public int? Sets { get; set; }

        
        [Comment("Distance in kilometers (for cardio exercises like running, biking)")]
        public double? DistanceKm { get; set; }

        
        [Comment("Weight used in kilograms (for strength training)")]
        public double? WeightKg { get; set; }

        
    }
}