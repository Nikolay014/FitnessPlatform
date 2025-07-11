using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessPlatform.Web.Controllers
{
    [Authorize]
    public abstract class BaseController : Controller
    {
        protected bool IsAuthenticated()
        {
            return User.Identity?.IsAuthenticated ?? false;
        }
        protected string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }
    }
}
