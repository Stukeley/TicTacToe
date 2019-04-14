﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TicTacToe.Models;
using TicTacToe.Services;

namespace TicTacToe.Controllers
{
	public class UserRegistrationController : Controller
	{
		private IUserService _userService;

		public UserRegistrationController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpPost]
		public async Task<IActionResult> Index(UserModel userModel)
		{
			await _userService.RegisterUser(userModel);
			return Content($"Użytkownik {userModel.FirstName} {userModel.LastName} został pomyślnie zarejestrowany!");
		}

		public IActionResult Index()
		{
			return View();
		}
	}
}