using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandsSystem
{
    [Command("ShowCommands")]
    class ShowCommands : ICommand
    {
        private Router _router;

        public ShowCommands(Router router)
        {
            _router = router;
        }

        public void Execute()
        {
            foreach(var command in _router.GetCommands())
            {
                Console.WriteLine(command);
            }
        }
    }
}
