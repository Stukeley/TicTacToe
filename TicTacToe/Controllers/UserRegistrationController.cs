using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Threading.Tasks;
using TicTacToe.Models;
using TicTacToe.Services;
using Microsoft.Extensions.Logging;

namespace TicTacToe.Controllers
{
	public class UserRegistrationController : Controller
	{
		private readonly IUserService _userService;
		private readonly IEmailService _emailService;
		private readonly ILogger<UserRegistrationController> _logger;

		public UserRegistrationController(IUserService userService, IEmailService emailService, ILogger<UserRegistrationController> logger)
		{
			_userService = userService;
			_emailService = emailService;
			_logger = logger;
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
		public async Task<IActionResult> EmailConfirmation(string email)
		{
			_logger.LogInformation($"##Start## Proces potwierdzenia adresu {email}");

			var user = await _userService.GetUserByEmail(email);

			var urlAction = new UrlActionContext
			{
				Action = "ConfirmEmail",
				Controller = "UserRegistration",
				Values = new { email },
				Protocol = Request.Scheme,
				Host = Request.Host.ToString()
			};

			var message = $"Dziękujemy za rejestrację na naszej stronie. Aby potwierdzić adres email, proszę kliknąć tutaj " + $"{Url.Action(urlAction)}";

			try
			{
				_emailService.SendEmail(email, "Potwierdzenie adresu e-mail w grze Kółko i krzyżyk", message);
			}
			catch (Exception e)
			{
			}

			if (user?.IsEmailConfirmed == true)
			{
				return RedirectToAction("Index", "GameInvitation", new { Email = email });
			}

			ViewBag.Email = email;
			return View();
		}

		public IActionResult Index()
		{
			return View();
		}
	}
}