﻿using Microsoft.Extensions.Logging;
using System;

namespace TicTacToe.Logging
{
	internal class FileLoggerProvider : ILoggerProvider
	{
		private readonly Func<string, LogLevel, bool> _filter;
		private string _fileName;

		public FileLoggerProvider(Func<string, LogLevel, bool> filter, string fileName)
		{
			_filter = filter;
			_fileName = fileName;
		}

		public ILogger CreateLogger(string categoryName)
		{
			return new FileLogger(categoryName, _filter, _fileName);
		}

		public void Dispose()
		{
		}
	}
}