using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FitnessPlatform.GCommon.ValidationConstraints.ApplicationUserConst;

namespace FitnessPlatform.Data.Models
{
    [Comment("Extended Identity user with fitness and event tracking data")]
    public class ApplicationUser : IdentityUser
    {
        [Comment("First name of the user")]
        [Required]
        [MaxLength(FirstNameMaxLength)]
        public string FirstName { get; set; }

        [Comment("Last name of the user")]
        [Required]
        [MaxLength(LastNameMaxLength)]
        public string LastName { get; set; }

        [Comment("Biological sex of the user: Male, Female, Other")]
        [Required]
        [MaxLength(GenderMaxLength)]
        
        public string Gender { get; set; }

        [Comment("Phone number of the current person")]
        [Required]
        [MaxLength(PhoneNumberMaxLength)]
        public string PhoneNumber { get; set; } = null!;

        [Comment("Profile image URL of the user")]
        [MaxLength(ImageURLMaxLength)]
        public string? ImageUrl { get; set; }

        [Comment("Date of birth")]
        [Required]
        public DateTime DateOfBirth { get; set; }

        [Comment("User's height in cm")]
        [Required]
        [Range(HaightMin, HaightMax)]
        public int HeightCm { get; set; }

        [Comment("User's weight in kg")]
        [Required]
        [Range(WeightMin, WeightMax)]
        public int WeightKg { get; set; }

        [NotMapped]
        public double? BMI
        {
            get
            {
                if (HeightCm == null || WeightKg == null || HeightCm == 0)
                {
                    return null;
                }

                double heightInMeters = HeightCm / 100.0;
                return Math.Round(WeightKg / (heightInMeters * heightInMeters), 2);
            }
        }

        [Comment("Trainers assigned to this user (if they are a client)")]
        public virtual ICollection<TrainerClient> AssignedTrainers { get; set; } = new List<TrainerClient>();

        [Comment("Gyms the user is subscribed to")]
        public virtual ICollection<UserGymSubscription> GymSubscriptions { get; set; } = new List<UserGymSubscription>();

        [Comment("Event registrations the user has made")]
        public virtual ICollection<EventRegistration> EventRegistrations { get; set; } = new List<EventRegistration>();

        [Comment("Daily logs including food, water, and workouts")]
        public virtual ICollection<DailyLog> DailyLogs { get; set; } = new List<DailyLog>();
    }
}
