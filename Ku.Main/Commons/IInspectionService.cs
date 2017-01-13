using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ku.Main.Commons
{
    public enum InspectedType
    {
        Concrete = 1,
        Abstract = 2,
        Interface = 4,
        All = Concrete | Abstract | Interface,
        AllNotConcrete = Abstract | Interface
    }
    public interface IInspectionService : IService
    {
        IEnumerable<Type> GetTypes<T>(InspectedType inspectedType = InspectedType.Concrete);
    }
}
