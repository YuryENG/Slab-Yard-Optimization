using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScatterSearch
{
    public class reference
    {
        public char[] phrase { get; set; }
        public double fitness { get; set; }

        public reference()
        { }

        public reference(string secret_phrase)
        {
            phrase = new char[secret_phrase.Length];
        }
    }
}
