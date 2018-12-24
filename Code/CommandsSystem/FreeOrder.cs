using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandsSystem
{
    class FreeOrder : Order
    {
        public FreeOrder(Good good, int count) : base(good, count)
        {
        }

        public override int GetTotalPrice()
        {
            return 0;
        }
    }
}
