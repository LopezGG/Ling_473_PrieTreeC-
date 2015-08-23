using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ling473_Proj4
{
    class DictionaryNode
    {
        public Dictionary<Char, DictionaryNode> Children;
        public string Payload;

        public DictionaryNode ()
        {
            Children = new Dictionary<char, DictionaryNode>();
        }
        public DictionaryNode (string payload)
        {
            Payload = payload;
            Children = new Dictionary<char, DictionaryNode>();
        }
    }
}
