using Microsoft.AspNetCore.Builder;
using TicTacToe.Middlewares;

namespace TicTacToe.Extensions
{
	public static class CommuncationMiddlewareExtension
	{
		public static IApplicationBuilder UseCommuncationMiddleware(this IApplicationBuilder app)
		{
			return app.UseMiddleware<CommunicationMiddleware>();
		}
	}
}