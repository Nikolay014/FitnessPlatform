using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessPlatform.Data.Models
{
    [Comment("Defines a user's subscription to a specific gym.")]
    public class UserGymSubscription
    {
        [Required]
        [Comment("ID of the user who is subscribed to the gym")]
        public string UserId { get; set; } = null!;


        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } = null!;

        [Required]
        [Comment("ID of the gym the user is subscribed to")]
        public int GymId { get; set; }

        [ForeignKey("GymId")]
        public Gym Gym { get; set; }

        [Required]
        public int SubscriptionPlanId { get; set; }
        public SubscriptionPlan Plan { get; set; }

        public DateTime SubscribedOn { get; set; } = DateTime.UtcNow;
        public DateTime ValidUntil { get; set; }
    }
}