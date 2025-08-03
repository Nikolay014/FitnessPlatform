using FitnessPlatform.Web.ViewModels.User;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Web.ViewModels.Event
{
    public class PaginatedEventsVM
    {
        public IEnumerable<EventVM> Events { get; set; } = new List<EventVM>();

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public int? SelectedGymId { get; set; }

        public IEnumerable<SelectListItem> Gyms { get; set; } = new List<SelectListItem>();
    }
}
