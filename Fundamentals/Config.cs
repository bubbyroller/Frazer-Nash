using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSandwich.Fundamentals
{
    public class Config
    {
        public int id { get; set; }
        public string file_version { get; set; }    
        public Dictionary<string, string> sandwiches { get; set; }
    }
}
