using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ku.Main.Ioc
{
    public interface IIocService 
    {
        T ResolveProperties<T>(T target);
        T Resolve<T>();
        HashSet<Type> PotentiallyMisconfigured { get; }
        String BinPath { get; }
    }
}
