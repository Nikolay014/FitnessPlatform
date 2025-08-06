using System.ComponentModel.DataAnnotations;
using static FitnessPlatform.GCommon.ValidationConstraints.SubscriptionPlanConst;

namespace FitnessPlatform.Data.Models
{
    public class SubscriptionPlan
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        public int DurationInDays { get; set; }  // напр. 30, 90 и т.н.

        [Required]
        public decimal Price { get; set; }

        [MaxLength(DescriptionMaxLength)]
        public string? Description { get; set; }

        public ICollection<UserGymSubscription> Subscriptions { get; set; } = new List<UserGymSubscription>();
    }
}
