using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System;
using System.Threading.Tasks;

namespace TicTacToe.Services
{
	//3 methods of getting user's culture
	public class CultureProviderResolverService : RequestCultureProvider
	{
		private static readonly char[] _cookieSeparator = new[] { '|' };
		private static readonly string _culturePrefix = "c=";
		private static readonly string _uiCulturePrefix = "uic=";

		public static string ParseCookieValue(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return null;
			}

			var parts = value.Split(_cookieSeparator, StringSplitOptions.RemoveEmptyEntries);
			if (parts.Length != 2)
			{
				return null;
			}

			var potentialCultureName = parts[0];
			var potentialUICultureName = parts[1];

			if (!potentialCultureName.StartsWith(_culturePrefix) || !potentialUICultureName.StartsWith(_uiCulturePrefix))
			{
				return null;
			}

			var cultureName = potentialCultureName.Substring(_culturePrefix.Length);
			var uiCultureName = potentialUICultureName.Substring(_uiCulturePrefix.Length);

			if (cultureName == null && uiCultureName == null)
			{
				return null;
			}

			if (cultureName != null && uiCultureName == null)
			{
				uiCultureName = cultureName;
			}

			if (cultureName == null && uiCultureName != null)
			{
				cultureName = uiCultureName;
			}

			return cultureName;
		}

		public override async Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext context)
		{
			if (GetCultureFromQueryString(context, out string culture))
			{
				return new ProviderCultureResult(culture, culture);
			}
			else if (GetCultureFromCookies(context, out culture))
			{
				return new ProviderCultureResult(culture, culture);
			}
			else if (GetCultureFromSession(context, out culture))
			{
				return new ProviderCultureResult(culture, culture);
			}

			return await NullProviderCultureResult;
		}

		private bool GetCultureFromQueryString(HttpContext context, out string culture)
		{
			if (context == null)
			{
				throw new ArgumentNullException(nameof(context));
			}

			var request = context.Request;
			if (!request.QueryString.HasValue)
			{
				culture = null;
				return false;
			}

			culture = request.Query["culture"];
			return true;
		}

		private bool GetCultureFromCookies(HttpContext context, out string culture)
		{
			if (context == null)
			{
				throw new ArgumentNullException(nameof(context));
			}

			var cookie = context.Request.Cookies["culture"];
			if (string.IsNullOrEmpty(cookie))
			{
				culture = null;
				return false;
			}

			culture = ParseCookieValue(cookie);
			return !string.IsNullOrEmpty(culture);
		}

		private bool GetCultureFromSession(HttpContext context, out string culture)
		{
			culture = context.Session.GetString("culture");
			return !string.IsNullOrEmpty(culture);
		}
	}
}