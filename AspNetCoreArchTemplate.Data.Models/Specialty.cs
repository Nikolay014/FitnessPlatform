using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Data.Models
{
    public class Specialty
    {
        [Key]
        [Comment("Unique identifier for the specialty")]
        public int Id { get; set; }

        /// <summary>
        /// Name of the specialty (e.g., "Cardio", "Strength Training")
        /// </summary>
        [Required, MaxLength(100), Comment("Name of the specialty")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Description of the specialty
        /// </summary>
        [MaxLength(500), Comment("Description of the specialty")]
        public string? Description { get; set; } = null!;

        /// <summary>
        /// List of trainers who specialize in this area
        /// </summary>
        public virtual ICollection<ApplicationUser> Trainers { get; set; } = new HashSet<ApplicationUser>();
    }
}
