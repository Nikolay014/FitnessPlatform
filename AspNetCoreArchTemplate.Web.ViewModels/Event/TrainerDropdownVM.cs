using System.ComponentModel.DataAnnotations;

namespace FitnessPlatform.Web.Views.Event
{
    public class TrainerDropdownVM
    {
        [Required]
        public int Id { get; set; } // Това ще е Trainer.Id
        public string FullName { get; set; } // Това ще е User.FirstName + User.LastName
    }
}
