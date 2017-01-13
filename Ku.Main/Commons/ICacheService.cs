using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ku.Main.Commons
{
    public interface ICacheService : IService
    {
        T GetOrAdd<T>(string key, Func<T> retrieveData, string region = "");
        void Invalidate(string region = "");
        void Remove(string key, string region = "");
    }
}
