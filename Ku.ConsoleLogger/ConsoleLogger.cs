using Ku.Main.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ku.ConsoleLogger
{
    public class ConsoleLogger : BaseLogger
    {
        private Type _type;
        private string[] _pars;

        public ConsoleLogger(Type type, string[] pars)
        {
            // TODO: Complete member initialization
            this._type = type;
            this._pars = pars;
        }

        protected override void Write(int level, string format, params object[] pars)
        {
            Console.WriteLine(DateTime.Now.ToString() + " L: " + level + " " + string.Format(format, pars));
        }

        protected override void Write(int level, Exception ex, string format = "", params object[] pars)
        {
            Console.WriteLine(DateTime.Now.ToString() + " L: " + level + " " + string.Format(format, pars) + " Exception: " + ex.Message);
        }
    }
}
