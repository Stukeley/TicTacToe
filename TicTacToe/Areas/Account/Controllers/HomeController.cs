﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TicTacToe.Services;

namespace TicTacToe.Areas.Account.Controllers
{
	[Area("Account")]
	public class HomeController : Controller
	{
		private IUserService _userService;
		public HomeController(IUserService userService)
		{
			_userService = userService;
		}

		[Authorize]
		public async Task<IActionResult> Index()
		{
			var user = await _userService.GetUserByEmail(User.Identity.Name);
			return View(user);
		}

		[Authorize]
		public IActionResult EnableTwoFactor()
		{
			_userService.EnableTwoFactor(User.Identity.Name, true);
			return RedirectToAction("Index");
		}

		[Authorize]
		public IActionResult DisableTwoFactor()
		{
			_userService.EnableTwoFactor(User.Identity.Name, false);
			return RedirectToAction("Index");
		}
	}
}