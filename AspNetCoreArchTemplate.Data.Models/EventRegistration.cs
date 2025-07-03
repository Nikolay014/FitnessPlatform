using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessPlatform.Data.Models
{
    public class EventRegistration
    {

        [Key]
        [Comment("ID of the event being registered for")]
        public int EventId { get; set; }

        [ForeignKey("EventId")]
        public virtual Event Event { get; set; } = null!;

        [Key]
        [Comment("ID of the user registering for the event")]
        public string UserId { get; set; } = null!;

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;

        [Required]
        [Comment("Date and time when the registration was made")]
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    }
}