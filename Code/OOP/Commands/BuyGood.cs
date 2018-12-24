using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP
{
    class BuyGood : ICommand
    {
        private WendingMachine _machine;
        private IOrder _order;

        public BuyGood(WendingMachine machine, IOrder order)
        {
            _machine = machine;
            _order = order;
        }

        public void Execute()
        {
            _machine.TryProcessOrder(_order);
        }
    }
}
