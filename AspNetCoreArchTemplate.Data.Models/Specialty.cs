using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FitnessPlatform.GCommon.ValidationConstraints.SpecialtyConst;

namespace FitnessPlatform.Data.Models
{
    public class Specialty
    {
        [Key]
        [Comment("Unique identifier for the specialty")]
        public int Id { get; set; }

        
        [Required, MaxLength(NameMaxLength), Comment("Name of the specialty")]
        public string Name { get; set; } = null!;

        
        [MaxLength(DescriptionMaxLength), Comment("Description of the specialty")]
        public string? Description { get; set; } = null!;

       
        public virtual ICollection<ApplicationUser> Trainers { get; set; } = new HashSet<ApplicationUser>();
    }
}
