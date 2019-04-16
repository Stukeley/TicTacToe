﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TicTacToe.Models;
using TicTacToe.Services;

namespace TicTacToe.Controllers
{
	public class GameInvitationController : Controller
	{
		private IUserService _userService;

		public GameInvitationController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpGet]
		public async Task<IActionResult> Index(string email)
		{
			var gameInvitationModel = new GameInvitationModel { InvitedBy = email };
			return View(gameInvitationModel);
		}
	}
}