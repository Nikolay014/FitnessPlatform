using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FitnessPlatform.GCommon.ValidationConstraints.WorkoutSessionConst

namespace FitnessPlatform.Data.Models
{
    public class WorkoutSession
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int DailyLogId { get; set; }

        [ForeignKey("DailyLogId")]
        public virtual DailyLog DailyLog { get; set; } = null!;


        [MaxLength(NotesMaxLength)]
        public string? Notes { get; set; }  

        
        public virtual ICollection<WorkoutEntry> Entries { get; set; } = new List<WorkoutEntry>();
    }
}
