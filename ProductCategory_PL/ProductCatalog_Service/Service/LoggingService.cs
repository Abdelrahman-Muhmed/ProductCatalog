using Microsoft.Extensions.Logging;
using ProductCatalog_BLL.IService;

namespace ProductCatalog_Service.ServiceRepo
{

    public class LoggingService : ILoggingService
    {
        private readonly ILogger<LoggingService> _logger;
        public LoggingService(ILogger<LoggingService> logger)
        {
            _logger = logger;
        }
        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }

        public void LogWarning(string message)
        {
            _logger.LogWarning(message);
        }

        public void LogError(string message, Exception ex)
        {
            _logger.LogError(ex, message);
        }
    }
}
