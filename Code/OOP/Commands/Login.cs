using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP
{
    class Login : ICommand
    {
        private Router _router;

        public Login(Router router)
        {
            _router = router;
        }

        public void Execute()
        {
            _router.Login();
        }
    }
}
