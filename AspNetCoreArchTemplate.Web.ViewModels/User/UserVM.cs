using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Web.ViewModels.User
{
    public class UserVM
    {
        public string Id { get; set; }

        public string Image { get; set; }

        public  string FullName { get; set; }

        public string Gender { get; set; }

        public string PhoneNumber { get; set; }

        public string Role { get; set; }

        public bool IsAdmin { get; set; }
    }
}
