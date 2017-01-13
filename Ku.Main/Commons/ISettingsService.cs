using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ku.Main.Commons
{
    public interface ISettingsService : IService
    {
        bool ContainsKey(string key);
        string GetString(string key);
        void SetString(string key, string value);
        IEnumerable<string> GetStrings(string key);
        void SetStrings(string key, IEnumerable<string> values);
        int GetInt(string key);
        void SetInt(string key, int value);

    }
}
