using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Web.ViewModels.Event
{
    public class DeleteEventVM
    {

        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public bool IsAdmin { get; set; }
    }
}
