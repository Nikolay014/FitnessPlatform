//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace FitnessPlatform.Data.Models
//{
//    [Comment("Defines a scheduled time block for a trainer in a specific gym.")]
//    public class TrainerSchedule
//    {
//        [Key]
//        [Comment("ID of the trainer")]
//        public int TrainerId { get; set; }

//        [ForeignKey("TrainerId")]
//        public virtual Trainer Trainer { get; set; } = null!;

//        [Required]
//        [Comment("ID of the gym where the trainer is scheduled")]
//        public int GymId { get; set; }

//        [ForeignKey("GymId")]
//        public virtual Gym Gym { get; set; } = null!;

//        [Key]
//        [Comment("Start date and time of the schedule")]
//        public DateTime StartTime { get; set; }

//        [Required]
//        [Comment("End date and time of the schedule")]
//        public DateTime EndTime { get; set; }

        
        
//    }
//}