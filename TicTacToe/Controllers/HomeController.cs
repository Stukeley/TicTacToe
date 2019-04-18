using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TicTacToe.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult SetCulture(string culture)
		{
			Request.HttpContext.Session.SetString("culture", culture);
			return RedirectToAction("Index");
		}

		public IActionResult Index()
		{
			var culture = Request.HttpContext.Session.GetString("culture");
			ViewBag.Language = culture;
			return View();
		}
	}
}