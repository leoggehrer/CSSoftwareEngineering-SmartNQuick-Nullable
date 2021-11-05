using Microsoft.AspNetCore.Mvc;
using SmartNQuick.AspMvc.Models;
using SmartNQuick.AspMvc.Modules.Language;
using System.Diagnostics;

namespace SmartNQuick.AspMvc.Controllers
{
    public class HomeController : Controller
	{
		//private readonly ILogger<HomeController> _logger;

		//public HomeController(ILogger<HomeController> logger)
		//{
		//	_logger = logger;
		//}

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

		public IActionResult ReloadTranslation()
        {
			Translator.Instance.ReloadTranslation();

			return RedirectToAction("Index");
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
