using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FamilyHubs.Idam.Api.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("/")]
        public string Index()
        {
            return ReturnInfo();
        }

        [HttpGet]
        [Route("/api/info")]
        public string Info()
        {
            return ReturnInfo();
        }

        private string ReturnInfo() 
        {
            var assembly = typeof(HomeController).Assembly;
            var version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

            return $"Version: {version}";

        }
    }
}
