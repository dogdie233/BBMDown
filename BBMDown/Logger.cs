using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBMDown
{
    public class Logger
    {
        private readonly IConsole _console;

        internal Logger(IConsole console)
        {
            _console = console;
        }

        public void Info(string message)
        {
            _console.WriteLine($"[{DateTime.Now}][Info] {message}");
        }

        public void Warn(string message)
        {
            _console.WriteLine($"[{DateTime.Now}][Warn] {message}");
        }

        public void Exception(Exception exception, string message)
        {
            _console.WriteLine($"[{DateTime.Now}][Exception] 发生了一个异常: {message}");
            _console.WriteLine(exception.ToString());
        }
    }
}
