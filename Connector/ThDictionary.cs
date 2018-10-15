using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectorDB
{
    public class ThDictionary : Dictionary<string, object>
    {
        private readonly Dictionary<string, object> _dictionary;

        public ThDictionary()
        {
            _dictionary = new Dictionary<string, object>();
        }
        public new void Add(string key, object value)
        {
            lock (_dictionary)
            {
                _dictionary.Add(key, value);
            }
        }
        public new void Clear()
        {
            lock (_dictionary)
            {
                _dictionary.Clear();
            }
        }
        public new bool TryGetValue(string thekey, out object result)
        {
            lock (_dictionary) { return _dictionary.TryGetValue(thekey, out result); }
        }
        public Dictionary<string, object> GetValues()
        {
            lock (_dictionary)
            {
                return _dictionary;
            }
        }
    }
}
