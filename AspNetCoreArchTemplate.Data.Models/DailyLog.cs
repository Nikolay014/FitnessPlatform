using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessPlatform.Data.Models
{
    public class DailyLog
    {
        public int Id { get; set; }

        [Required]
        [Comment("Id of the user that the daily log belongs to")]
        public string UserId { get; set; } = null!;

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;

        [Comment("Date of the daily log")]
        public DateTime Date { get; set; }

        public virtual ICollection<Food> Foods { get; set; } = new List<Food>();


        public virtual ICollection<WorkoutSession> WorkoutSessions { get; set; } = new List<WorkoutSession>();
    }
}