using FitnessPlatform.Web.ViewModels.DailyLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPlatform.Services.Core.Contracts
{
    public interface IDailyLogService
    {
        Task<DailyLogVM> GetUserLogAsync(string userId, DateTime date);
    }
}
