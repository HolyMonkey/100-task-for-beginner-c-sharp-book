using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandsSystem
{
    class AddMoney : ICommand
    {
        private WendingMachine _machine;
        private int _money;

        public AddMoney(WendingMachine machine, int money)
        {
            _machine = machine;
            _money = money;
        }

        public void Execute()
        {
            _machine.AddBalance(_money);
        }
    }
}
