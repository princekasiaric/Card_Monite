﻿using CardMon.Core.Interfaces.Services;
using NLog;

namespace CardMon.Core.Services
{
    public class LoggerManager : ILoggerManager
    {
        private static ILogger _logger = LogManager.GetCurrentClassLogger();
        public LoggerManager() { }

        public void LogDebug(string message) =>
            _logger.Debug(message);

        public void LogError(string message) =>
            _logger.Error(message);

        public void LogInfo(string message) =>
            _logger.Info(message);

        public void LogWarning(string message) =>
            _logger.Warn(message);
        public void LogStackTrace(string message) =>
            _logger.Trace(message);
    }
}
