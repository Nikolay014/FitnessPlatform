using FitnessPlatform.Data.Models;
using FitnessPlatform.Services.Core;
using FitnessPlatform.Services.Core.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessPlatform.Web.Controllers
{
    public class DailyLogController : BaseController
    {
        private readonly IDailyLogService dailyLogService;

        public DailyLogController(IDailyLogService dailyLogService)
        {
            this.dailyLogService = dailyLogService ?? throw new ArgumentNullException(nameof(dailyLogService));
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> MyLog(DateTime? date,string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                id = GetUserId();
            }
             // текущият потребител

            var logDate = date ?? DateTime.Today;

            var vm = await dailyLogService.GetUserLogAsync(id, logDate);
            return View(vm); ;
        }
    }
}
