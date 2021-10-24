namespace CardMon.Core.Interfaces.Services
{
    public interface ILoggerManager
    {
        void LogDebug(string message);
        void LogInfo(string message);
        void LogError(string message);
        void LogWarning(string message);
        void LogStackTrace(string message);
    }
}
