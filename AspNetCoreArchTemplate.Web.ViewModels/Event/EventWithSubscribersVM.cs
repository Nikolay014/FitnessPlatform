using FitnessPlatform.Web.ViewModels.Gym;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Web.ViewModels.Event
{
    public class EventWithSubscribersVM
    {
        public string EvenName { get; set; }
        public List<SubscribedEventUserVM> Users { get; set; }
    }
}
