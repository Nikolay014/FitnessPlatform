using FitnessPlatform.Services.Core;
using FitnessPlatform.Services.Core.Contracts;
using FitnessPlatform.Web.ViewModels.Event;
using FitnessPlatform.Web.ViewModels.Gym;
using FitnessPlatform.Web.ViewModels.User;
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

        public async Task<IActionResult> AllEvents(int? gymId, int page = 1)
        {
            string? userId = GetUserId();
            PaginatedEventsVM events = await eventService.GetEventAsync(gymId,page, userId);
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

            //// Парсваме датата и часа от string към DateTime
            //if (!DateTime.TryParse($"{createEventVM.StartDate} {createEventVM.StartTime}", out var startDateTime))
            //{
            //    ModelState.AddModelError(string.Empty, "Invalid start date or time.");
            //    return View(createEventVM);
            //}

            //if (startDateTime < DateTime.Now)
            //{
            //    ModelState.AddModelError(string.Empty, "Start time must be in the future.");
            //    return View(createEventVM);
            //}

            //// Същото може да се направи и за EndDate + EndTime, ако искаш:
            //if (!DateTime.TryParse($"{createEventVM.EndDate} {createEventVM.EndTime}", out var endDateTime))
            //{
            //    ModelState.AddModelError(string.Empty, "Invalid end date or time.");
            //    return View(createEventVM);
            //}

            //if (endDateTime <= startDateTime)
            //{
            //    ModelState.AddModelError(string.Empty, "End time must be after start time.");
            //    return View(createEventVM);
            //}

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
        [Authorize(Roles = "Admin,Trainer")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return BadRequest("Event ID cannot be null or empty.");
            }

            EditEventVM editEventVM = await eventService.GetEventForEditAsync(id);
            if (editEventVM == null)
            {
                return NotFound("Event not found.");
            }
            return View(editEventVM);
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Trainer")]
        public async Task<IActionResult> Edit(EditEventVM editEventVM)
        {
            if (!ModelState.IsValid)
            {
                return View(editEventVM);
            }
            await eventService.EditEventAsync(editEventVM);

            return RedirectToAction("AllEvents", "Event");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            string? userId = GetUserId();
            bool isAdmin = User.IsInRole("Admin");
            DeleteEventVM deleteGymVM = await eventService.GetEventForDeleteAsync(id, userId, isAdmin);
            if (deleteGymVM == null)
            {
                return RedirectToAction("AllEvents", "Event");
            }
            return View(deleteGymVM);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            string? userId = GetUserId();
            bool isAdmin = User.IsInRole("Admin");

            if (!isAdmin || userId == null)
            {
                return Unauthorized();
            }
            await eventService.DeleteEventAsync(id);
            return RedirectToAction("AllEvents", "Event");
        }
        [Authorize(Roles = "User")]
        public async Task <IActionResult> Subscribe(int id)
        {
            string userId = GetUserId();

            if(userId == null)
            {
                return Unauthorized();
            }
            await eventService.SubscribeEventAsync(id, userId);
            return RedirectToAction("AllEvents", "Event");
        }
        [Authorize(Roles = "User")]
        public async Task<IActionResult> RemoveSubscription(int id)
        {
            string userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }
            await eventService.RemoveSubscriptionAsync(id,userId);
            return RedirectToAction("AllEvents", "Event");

        }
        [Authorize(Roles = "Admin,Trainer")]
        public async Task<IActionResult> GetSubcribedUsers(int id)
        {
            string? userId = GetUserId();
            EventWithSubscribersVM users = await eventService.GetSubscribedUsersAsync(id,userId);
            return View(users);
        }
        


    }
}
