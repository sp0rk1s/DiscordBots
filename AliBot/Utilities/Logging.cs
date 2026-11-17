using Discord;

namespace AliBot.Utilities;
static class Logging {
    public static Task Log(LogMessage message) {
        string time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        switch (message.Severity) {
            case LogSeverity.Critical or LogSeverity.Error:
                Console.WriteLine($"{time} \u001b[32m!\u001b[0m {message}");
                break;
            case LogSeverity.Warning or LogSeverity.Debug:
                Console.WriteLine($"{time} \u001b[34m?\u001b[0m {message}");
                break;
            case LogSeverity.Verbose:
                Console.WriteLine($"{time} \u001b[35m.\u001b[0m {message}");
                break;
            case LogSeverity.Info:
                Console.WriteLine($"{time}   {message}");
                break;
        }
        return Task.CompletedTask;
    }

    public static void Log(string message) {
        string time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        Console.WriteLine($"{time}   {message}");
    }

    public static void Note(string message) {
        string time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        Console.WriteLine($"{time} \u001b[35m.\u001b[0m {message}");
    }

    public static void Warn(string message) {
        string time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        Console.WriteLine($"{time} \u001b[34m?\u001b[0m {message}");
    }

    public static void Error(string message) {
        string time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        Console.WriteLine($"{time} \u001b[32m!\u001b[0m {message}");
    }
}
