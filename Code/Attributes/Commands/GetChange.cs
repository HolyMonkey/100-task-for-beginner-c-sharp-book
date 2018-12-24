using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandsSystem
{
    class GetChange : ICommand
    {
        private WendingMachine _machine;

        public GetChange(WendingMachine machine)
        {
            _machine = machine;
        }

        public void Execute()
        {
            _machine.DiscardBalance(_machine.Balance);
        }
    }
}
