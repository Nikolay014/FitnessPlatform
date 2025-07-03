using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessPlatform.Data.Models
{
    public class TrainerClient
    {


        [Key]
        [Comment("ID of the trainer")]
        public int TrainerId { get; set; }
        [ForeignKey("TrainerId")]
        public virtual Trainer Trainer { get; set; } = null!;

        [Key]
        [Comment("ID of the client")]
        public string ClientId { get; set; } = null!;
        [ForeignKey("ClientId")]
        public virtual ApplicationUser Client { get; set; } = null!;

        [Required]
        [Comment("Date when the trainer-client relationship was established")]
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
    }
}