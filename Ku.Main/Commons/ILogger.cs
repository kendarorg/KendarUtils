using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ku.Main.Commons
{
    public interface ILogger
    {
        void Trace(string format, params object[] pars);
        void Trace(Exception ex, string format = null, params object[] pars);
        void Debug(string format, params object[] pars);
        void Debug(Exception ex, string format = null, params object[] pars);
        void Warn(string format, params object[] pars);
        void Warn(Exception ex, string format = null, params object[] pars);
        void Error(string format, params object[] pars);
        void Error(Exception ex, string format = null, params object[] pars);
        void Fatal(string format, params object[] pars);
        void Fatal(Exception ex, string format = null, params object[] pars);
        void Info(string format, params object[] pars);
        
    }
}
