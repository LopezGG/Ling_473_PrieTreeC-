using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ling473_Proj4
{
    class DictionaryNode
    {
        public Char Nucleotide;
        public Dictionary<Char, DictionaryNode> Children;
        public string Payload;

        public DictionaryNode ()
        {
            Nucleotide =' ';
            Children = new Dictionary<char, DictionaryNode>();
        }
        public DictionaryNode (char nucleotide, string payload)
        {
            Nucleotide = nucleotide;
            Payload = payload;
            Children = new Dictionary<char, DictionaryNode>();
        }
    }
}
