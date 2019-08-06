using Microsoft.AspNetCore.Builder;
using TicTacToe.Middlewares;

namespace TicTacToe.Extensions
{
	public static class CommuncationMiddlewareExtension
	{
		public static IApplicationBuilder UseCommunicationMiddleware(this IApplicationBuilder app)
		{
			return app.UseMiddleware<CommunicationMiddleware>();
		}
	}
}