using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TicTacToe.Services;

namespace TicTacToe.Controllers
{
	public class GameSessionController : Controller
	{
		private IGameSessionService _gameSessionService;

		public GameSessionController(IGameSessionService gameSessionService)
		{
			_gameSessionService = gameSessionService;
		}

		public async Task<IActionResult> Index(Guid id)
		{
			var session = await _gameSessionService.GetGameSession(id);

			if (session == null)
			{
				var gameInvitationService = Request.HttpContext.RequestServices.GetService<IGameInvitationService>();
				var invitation = await gameInvitationService.Get(id);
				session = await _gameSessionService.CreateGameSession(invitation.Id, invitation.InvitedBy, invitation.EmailTo);
			}

			return View(session);
		}
	}
}