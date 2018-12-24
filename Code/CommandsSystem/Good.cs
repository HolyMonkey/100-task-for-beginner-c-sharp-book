using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandsSystem
{
    class Good
    {
        public Good(string name, int price, int count)
        {
            Name = name;
            Price = price;
            Count = count;
        }

        public string Name { get; private set; }
        public int Price { get; private set; }
        public int Count { get; set; }
    }
}
