using Luo.Core.LayuiAdmin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Luo.Core.LayuiAdmin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IServices.DataInitService _dataInit;
        public HomeController(ILogger<HomeController> logger, IServices.DataInitService dataInit)
        {
            _logger = logger;
            _dataInit = dataInit;
        }

        public IActionResult Main()
        {
            _dataInit.InitUser();
            return View();
        }

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