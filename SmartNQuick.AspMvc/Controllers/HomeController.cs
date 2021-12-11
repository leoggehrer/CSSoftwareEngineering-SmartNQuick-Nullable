using Microsoft.AspNetCore.Mvc;
using SmartNQuick.AspMvc.Models;
using System.Diagnostics;

namespace SmartNQuick.AspMvc.Controllers
{
    public partial class HomeController : Controller
	{
		static HomeController()
		{
			ClassConstructing();
			ClassConstructed();
		}
		static partial void ClassConstructing();
		static partial void ClassConstructed();

		//private readonly ILogger<HomeController> _logger;

		//public HomeController(ILogger<HomeController> logger)
		//{
		//	_logger = logger;
		//}

		public HomeController()
		{
			Constructing();
			Constructed();
		}
		partial void Constructing();
		partial void Constructed();

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		public IActionResult Contact()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		public IActionResult ErrorList()
        {
			return View();
        }
    }
}
