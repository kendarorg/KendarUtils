using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ku.Main.Commons
{
    public static class LoggerManager
    {
        private static ILoggerFactory _loggerFactory;
        public static void Initialize(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public static ILogger CreateLogger<T>(params string[] pars)
        {
            return _loggerFactory.CreateLogger(typeof(T),pars);
        }
    }
}
