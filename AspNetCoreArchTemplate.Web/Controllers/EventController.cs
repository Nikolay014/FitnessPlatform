using FitnessPlatform.Services.Core;
using FitnessPlatform.Services.Core.Contracts;
using FitnessPlatform.Web.ViewModels.Event;
using FitnessPlatform.Web.ViewModels.Gym;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessPlatform.Web.Controllers
{
    public class EventController : BaseController
    {
        private readonly IEventService eventService;

        public EventController(IEventService eventService)
        {
            this.eventService = eventService ?? throw new ArgumentNullException(nameof(eventService));
        }
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]

        public async Task<IActionResult> AllEvents()
        {
            string? userId = GetUserId();
            IEnumerable<EventVM> events = await eventService.GetEventAsync(userId);
            return View(events);

        }
        [Authorize(Roles = "Admin,Trainer")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            CreateEventVM createEventVM = await eventService.GetGymsAndTrainersAsync();
            return View(createEventVM);
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Trainer")]
        public async Task<IActionResult> Create(CreateEventVM createEventVM)
         {
            if (!ModelState.IsValid)
            {
                return View(createEventVM);
            }
            await eventService.CreateEventAsync(createEventVM);

            return RedirectToAction("AllEvents", "Event");
        }
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return BadRequest("Event ID cannot be null or empty.");
            }

            string? userId = GetUserId();
            bool isAdmin = User.IsInRole("Admin");
            EventDetailsVM eventDetails = await eventService.GetEventDetailsAsync(id, userId,isAdmin);
            if (eventDetails == null)
            {
                return NotFound("Event not found.");
            }

            return View(eventDetails);
        }
    }
}
