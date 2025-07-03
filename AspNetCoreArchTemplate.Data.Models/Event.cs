using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FitnessPlatform.Data.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Comment("Title of the event")]
        public string Title { get; set; } = null!;

        public string? Image { get; set; }

        [Required]
        [Comment("ID of the gym where the event will be held")]
        public int GymId { get; set; }

        [ForeignKey("GymId")]
        public virtual Gym Gym { get; set; }

        [Required]
        [Comment("ID of the trainer")]
        public int TrainerId { get; set; }

        [ForeignKey("TrainerId")]
        public virtual Trainer Trainer { get; set; } = null!;

        [Required]
        [Comment("Start date and time of the event")]
        public DateTime StartDate { get; set; }

        [Required]
        [Comment("End date and time of the event")]
        public DateTime EndDate { get; set; }

        [MaxLength(500)]
        [Comment("Description of the event")]
        public string? Description { get; set; }

        public bool IsDeleted { get; set; } = false;


        public virtual ICollection<EventRegistration> Registrations { get; set; } = new List<EventRegistration>();
    }
}