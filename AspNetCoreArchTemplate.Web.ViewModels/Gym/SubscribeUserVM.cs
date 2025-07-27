using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Web.ViewModels.Gym
{
    public class SubscribeUserVM
    {
        public string Id { get; set; }

        

        public string Image { get; set; }

        public string FullName { get; set; }

        public string Gender { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime SubscriptionEndDate { get; set; }
    }
}
