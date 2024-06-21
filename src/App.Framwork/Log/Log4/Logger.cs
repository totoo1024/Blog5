using log4net;
using log4net.Config;
using log4net.Repository;
using System;
using System.IO;
using App.Framwork.DependencyInjection;

namespace App.Framwork.Log
{
    public class Logger : ILogger, ISingletonDependency
    {
        private ILog _logger;
        private ILoggerRepository _loggerRepository;
        public Logger()
        {
            _loggerRepository = LogManager.CreateRepository("NETCoreLog4netRepository");
            var file = new FileInfo(Path.Combine(AppContext.BaseDirectory, "log4net.config"));
            XmlConfigurator.Configure(_loggerRepository, file);
            _logger = LogManager.GetLogger("NETCoreLog4netRepository", "loginfo");
        }
        public void Info(string message, Exception exception)
        {
            if (exception == null)
            {
                _logger.Info(message);
            }
            else
            {
                _logger.Info(message, exception);
            }
        }

        public void Warn(string message, Exception exception)
        {
            if (exception == null)
            {
                _logger.Warn(message);
            }
            else
            {
                _logger.Warn(message, exception);
            }
        }

        public void Debug(string message, Exception exception)
        {
            if (exception == null)
            {
                _logger.Debug(message);
            }
            else
            {
                _logger.Debug(message, exception);
            }
        }

        public void Error(string message, Exception exception)
        {
            if (exception == null)
            {
                _logger.Error(message);
            }
            else
            {
                _logger.Error(message, exception);
            }
        }
    }
}