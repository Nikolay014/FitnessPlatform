using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Data.Models
{
    public class Gym
    {
        [Key]
        [Comment("Key of the Gym")]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Comment("Name of the Gym")]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(80)]
        [Comment("Location of the Gym")]
        public string Location { get; set; } = null!;

        [Required]
        [StringLength(350)]
        [Comment("Description of the Gym")]
        public string Description { get; set; } = null!;



        [Required]    
        public virtual ICollection<GymWorkingHours> WorkingHours { get; set; } = new List<GymWorkingHours>();

        public virtual ICollection<GymImage> Images { get; set; } = new List<GymImage>();


        public virtual ICollection<Trainer> Trainers { get; set; } = new List<Trainer>();

        
        public virtual ICollection<UserGymSubscription> Subscribers { get; set; } = new List<UserGymSubscription>();

        
        public virtual ICollection<TrainerSchedule> Schedules { get; set; } = new List<TrainerSchedule>();


        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedOn { get; set; }

    }
}
