using System.CommandLine;

namespace BBMDown
{
    public class Logger
    {
        private readonly IConsole _console;

        internal Logger(IConsole console)
        {
            _console = console;
        }

        public void Info(string? message)
        {
            if (message == null) return;
            _console.WriteLine($"[{DateTime.Now}][Info] {message}");
        }

        public void Warn(string? message)
        {
            if (message == null) return;
            _console.WriteLine($"[{DateTime.Now}][Warn] {message}");
        }

        public void Error(string? message)
        {
            if (message == null) return;
            _console.WriteLine($"[{DateTime.Now}][Error] {message}");
        }

        public void Exception(Exception exception, string? message)
        {
            _console.WriteLine($"[{DateTime.Now}][Exception] 发生了一个异常{(message != null ? ": " + message : "")}");
            _console.WriteLine(exception.ToString());
        }
    }
}
