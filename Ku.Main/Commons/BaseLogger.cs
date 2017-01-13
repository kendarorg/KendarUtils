using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ku.Main.Commons
{
    public abstract class BaseLogger : ILogger
    {
        protected abstract void Write(int level, string format, params object[] pars);
        protected abstract void Write(int level, Exception ex, string format = "", params object[] pars);

        public virtual void Trace(string format, params object[] pars)
        {
            Write(6, format, pars);
        }

        public virtual void Trace(Exception ex, string format = null, params object[] pars)
        {
            Write(6, ex, format, pars);
        }

        public virtual void Debug(string format, params object[] pars)
        {
            Write(5, format, pars);
        }

        public virtual void Debug(Exception ex, string format = null, params object[] pars)
        {
            Write(5, ex, format, pars);
        }

        public virtual void Warn(string format, params object[] pars)
        {
            Write(4, format, pars);
        }

        public virtual void Warn(Exception ex, string format = null, params object[] pars)
        {
            Write(4, ex, format, pars);
        }

        public virtual void Error(string format, params object[] pars)
        {
            Write(3, format, pars);
        }

        public virtual void Error(Exception ex, string format = null, params object[] pars)
        {
            Write(3, ex, format, pars);
        }

        public virtual void Fatal(string format, params object[] pars)
        {
            Write(2, format, pars);
        }

        public virtual void Fatal(Exception ex, string format = null, params object[] pars)
        {
            Write(2, ex, format, pars);
        }

        public virtual void Info(string format, params object[] pars)
        {
            Write(1, format, pars);
        }
    }
}
