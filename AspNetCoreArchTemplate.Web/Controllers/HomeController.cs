namespace AspNetCoreArchTemplate.Web.Controllers
{
    using System.Diagnostics;

    using ViewModels;

    using Microsoft.AspNetCore.Mvc;
    using FitnessPlatform.Web.Controllers;
    using Microsoft.AspNetCore.Authorization;

    public class HomeController : BaseController
    {
        public HomeController(ILogger<HomeController> logger)
        {

        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
