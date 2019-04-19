using TicTacToe.Options;

namespace TicTacToe.Extensions
{
	public static class ConfigureLoggingExtension
	{
		public static ILoggingBuilder AddLoggingConfiguration(this ILoggingBuilder loggingBuilder, IConfiguration configuration)
		{
			var loggingOptions = new LoggingOptions();
			configuration.GetSection("Logging").Bind(loggingOptions);

			foreach (var provider in loggingOptions.Providers)
			{
				switch (provider.Name.ToLower())
				{
					case "console":
						loggingBuilder.AddConsole();
						break;

					case "file":
						string filePath = Path.Combine(Directory.GetCurrentDirectory(), "logs", $"TicTacToe_{DateTime.Now.ToString("ddMMyyHHmm")}.log");
						loggingBuilder.AddFile(filePath, (LogLevel)provider.LogLevel);
						break;

					default:
						break;
				}
			}

			return loggingBuilder;
		}
	}
}