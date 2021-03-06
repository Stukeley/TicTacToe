﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace TicTacToe.Controllers
{
	public class HomeController : Controller
	{
		public async Task<IActionResult> Index()
		{
			return await Task.Run(() =>
			{
				var culture = Request.HttpContext.Session.GetString("culture");
				ViewBag.Language = culture;
				return View();
			});
		}

		public async Task<IActionResult> SetCulture(string culture)
		{
			return await Task.Run(() =>
			{
				Request.HttpContext.Session.SetString("culture", culture);
				return RedirectToAction("Index");
			});
		}

		//[Authorize]
		//[Authorize(Roles = "Administrator")]
		[Authorize(Policy = "AdministratorAccessLevelPolicy")]
		public async Task<IActionResult> SecuredPage()
		{
			return await Task.Run(() =>
			{
				ViewBag.SecureWord = "Zabezpieczona strona";
				return View("SecuredPage");
			});
		}
	}
}