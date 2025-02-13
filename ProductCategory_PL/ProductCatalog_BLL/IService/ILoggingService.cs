namespace ProductCatalog_BLL.IService
{
    public interface ILoggingService
    {
        void LogInformation(string message);
        void LogWarning(string message);
        void LogError(string message, Exception ex);
    }
}
