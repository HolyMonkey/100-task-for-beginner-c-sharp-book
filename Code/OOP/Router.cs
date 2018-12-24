using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP
{
    class Router
    {
        private WendingMachine _machine;
        private RouterState _state;

        public Router(WendingMachine machine)
        {
            _machine = machine;
            _state = new DefaultState(this);
        }

        public ICommand CreateCommand(Request request)
        {
            switch (request.Command)
            {
                case "AddMoney":
                    if (request.IsIncorectValuesCount(1)) return null;

                    return new AddMoney(_machine, request.Values[0]);
                case "GetChange":
                    if (request.IsIncorectValuesCount(0)) return null;

                    return new GetChange(_machine);
                case "BuyGood":
                    if (request.IsIncorectValuesCount(2)) return null;

                    return new BuyGood(_machine, _state.MakeOrder(request));
                case "ShowCommands":
                    if (request.IsIncorectValuesCount(0)) return null;

                    return new ShowCommands("AddMoney", "GetChange", "BuyGood", "ShowCommands");
                case "Login":
                    if (request.IsIncorectValuesCount(0)) return null;

                    return new Login(this);
                default:
                    return null;
            }
        }

        public void Login()
        {
            _state = new AdminState(this);
        }

        public void Logout()
        {
            _state = new DefaultState(this);
        }

        abstract class RouterState
        {
            protected readonly Router Router;

            public RouterState(Router router)
            {
                Router = router;
            }

            public abstract IOrder MakeOrder(Request request);
        }

        class DefaultState : RouterState
        {
            public DefaultState(Router router) : base(router)
            {
            }

            public override IOrder MakeOrder(Request request)
            {
                return new Order(Router._machine.GetFromId(request.Values[0]), request.Values[1]);
            }
        }

        class AdminState : RouterState
        {
            public AdminState(Router router) : base(router)
            {
            }

            public override IOrder MakeOrder(Request request)
            {
                return new FreeOrder(Router._machine.GetFromId(request.Values[0]), request.Values[1]);
            }
        }
    }
}
