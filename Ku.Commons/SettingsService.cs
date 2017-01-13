using Ku.Main.Commons;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ku.Commons
{
    public class SettingsService : ISettingsService
    {
        private static ConcurrentDictionary<string, string> _settings = new ConcurrentDictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

        public SettingsService()
        {
            foreach (var key in ConfigurationManager.AppSettings.AllKeys)
            {
                _settings[key] = ConfigurationManager.AppSettings[key];
            }
            foreach (ConnectionStringSettings connectionString in ConfigurationManager.ConnectionStrings)
            {
                _settings[connectionString.Name] = connectionString.ConnectionString;
            }
        }

        public bool ContainsKey(string key)
        {
            return _settings.ContainsKey(key);
        }

        public string GetString(string key)
        {
            return _settings[key];
        }

        public void SetString(string key, string value)
        {
            _settings[key] = value;
        }

        public IEnumerable<string> GetStrings(string key)
        {
            var res = GetString(key);
            return res.Split(';');
        }

        public void SetStrings(string key, IEnumerable<string> values)
        {
            SetString(key, string.Join(";", values));
        }

        public int GetInt(string key)
        {
            var res = GetString(key);
            return int.Parse(res);
        }

        public void SetInt(string key, int value)
        {
            SetString(key, value.ToString());
        }
    }
}
