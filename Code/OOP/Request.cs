using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP
{
    class Request
    {
        public Request(string command, int[] values)
        {
            Command = command;
            Values = values;
        }

        public string Command { get; set; }
        public int[] Values { get; set; }

        public bool IsIncorectValuesCount(int correct)
        {
            return correct != Values.Length;
        }
    }
}
