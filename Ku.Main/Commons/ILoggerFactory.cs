using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ku.Main.Commons
{
    public interface ILoggerFactory : IService
    {
        ILogger CreateLogger(Type type, string[] pars);
    }
}
