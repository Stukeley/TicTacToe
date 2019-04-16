using Microsoft.AspNetCore.Mvc;
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
			if (ModelState.IsValid)
			{
				await _userService.RegisterUser(userModel);
				return RedirectToAction(nameof(EmailConfirmation), new { userModel.Email });
			}
			else
			{
				return View(userModel);
			}
		}

		[HttpGet]
		public IActionResult EmailConfirmation(string email)
		{
			ViewBag.Email = email;
			return View();
		}

		public IActionResult Index()
		{
			return View();
		}
	}
}