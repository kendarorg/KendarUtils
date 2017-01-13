using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ku.Main.Web
{
    public interface ICorsEnabler : IService
    {
        bool Application_BeginRequest(object sender, EventArgs e);
    }
}
