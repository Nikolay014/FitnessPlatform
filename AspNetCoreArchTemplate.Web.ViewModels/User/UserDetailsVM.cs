using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Web.ViewModels.User
{
    public class UserDetailsVM
    {
        public string UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }

        public string PhoneNumber { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int HeightCm { get; set; }

        public int WeightKg { get; set; }

        public double? BMI {             get
            {
                if (HeightCm == 0 || WeightKg == 0)
                {
                    return null;
                }
                return Math.Round(WeightKg / Math.Pow(HeightCm / 100.0, 2), 2);
            }
        }


    }
}
