using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FitnessPlatform.Data.Models
{
    public class Trainer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string TrainerImage { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = null!;
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } = null!;

        [Required]
        public int GymId { get; set; }
        [ForeignKey("GymId")]
        public Gym Gym { get; set; }

        [Required]
        [MaxLength(100)]
        [Comment("Specialty of the trainer (e.g. Cardio, Strength, Yoga)")]
        public string Specialty { get; set; } = null!;
        

        public virtual ICollection<TrainerClient> Clients { get; set; } = new List<TrainerClient>();
        //public virtual ICollection<TrainerSchedule> Schedules { get; set; } = new List<TrainerSchedule>();
        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
    }
}