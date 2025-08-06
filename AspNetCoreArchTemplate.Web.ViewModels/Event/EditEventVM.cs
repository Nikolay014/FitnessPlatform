using FitnessPlatform.Web.Views.Event;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FitnessPlatform.GCommon.ValidationConstraints.EventConst;

namespace FitnessPlatform.Web.ViewModels.Event
{
    public class EditEventVM:IValidatableObject
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(TitleMaxLength, ErrorMessage = "Name must be between 3 and 100 characters.")]
        [MinLength(TitleMinLength, ErrorMessage = "Name must be between 3 and 100 characters.")]
        public string Title { get; set; }

        [Required, MaxLength(ImageURLMaxLength)]
        public string Image { get; set; }
        [Required]
        public int GymId { get; set; }

        public string? Description { get; set; }


        [Required]
        public int TrainerId { get; set; }

        public string StartDate { get; set; }
        public string StartTime { get; set; }

        public string EndDate { get; set; }
        public string EndTime { get; set; }

        public List<FitnessPlatform.Data.Models.Gym> Gyms { get; set; } = new List<FitnessPlatform.Data.Models.Gym>();

        public List<TrainerDropdownVM> TrainersDropdown { get; set; } = new List<TrainerDropdownVM>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            DateTime startDateTime, endDateTime;

            bool isStartParsed = DateTime.TryParse($"{StartDate} {StartTime}", out startDateTime);
            bool isEndParsed = DateTime.TryParse($"{EndDate} {EndTime}", out endDateTime);

            if (isStartParsed && isEndParsed)
            {
                if (endDateTime <= startDateTime)
                {
                    yield return new ValidationResult(
                        "End date and time must be after start date and time.",
                        new[] { nameof(EndDate), nameof(EndTime) }
                    );
                }
            }
            else
            {
                if (!isStartParsed)
                {
                    yield return new ValidationResult("Invalid start date/time format.");
                }

                if (!isEndParsed)
                {
                    yield return new ValidationResult("Invalid end date/time format.");
                }
            }
        }


    }

    
}
