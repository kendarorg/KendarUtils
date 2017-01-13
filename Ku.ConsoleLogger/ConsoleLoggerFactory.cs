using Ku.Main.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ku.ConsoleLogger
{
    public class ConsoleLoggerFactory : ILoggerFactory
    {
        public ILogger CreateLogger(Type type, string[] pars)
        {
            return new ConsoleLogger(type, pars);
        }
    }
}
