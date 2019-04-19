using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using TicTacToe.Extensions;

namespace TicTacToe
{
	public class Program
	{
		public static void Main(string[] args)
		{
			BuildWebHost(args).Run();
		}

		public static IWebHost BuildWebHost(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.CaptureStartupErrors(true)
				.UseStartup<Startup>()
				.PreferHostingUrls(true)
				.UseUrls("http://localhost:5000")
				.UseApplicationInsights()
				.ConfigureLogging((hostingcontext, logging) =>
				{
					logging.AddLoggingConfiguration(hostingcontext.Configuration);
				})
				.Build();
	}
}