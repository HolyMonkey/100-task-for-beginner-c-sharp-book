using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace CommandsSystem
{
    class Router
    {
        private readonly Type _commandBaseType = typeof(ICommand);

        private WendingMachine _machine;
        private RouterState _state;
        private Dictionary<Type, object> _dependencies;

        public Router(WendingMachine machine)
        {
            _machine = machine;
            _state = new DefaultState(this);

            _dependencies = new Dictionary<Type, object>()
                            {
                                { typeof(WendingMachine), _machine},
                                { typeof(Router), this }
                            };
        }

        public ICommand CreateCommand(Request request)
        {
            var commandType = GetCommandTypeByName(request.Command);
            if (commandType != null)
            {
                var instance = CreateInstance(commandType, request);
                return instance;
            }
            else
            {
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

        public IEnumerable<string> GetCommands()
        {
            return GetCommandsTypes()
                    .SelectMany(type => type.GetCustomAttributes<CommandAttribute>())
                    .Select(attribute => attribute.CommandName);
        }

        private object[] ResolveDependenciesAndMerge(ConstructorInfo constructor, Request request)
        {
            List<object> args = new List<object>();
            Queue<int> requestArgs = new Queue<int>(request.Values);

            foreach (var parameter in constructor.GetParameters())
            {
                if (_dependencies.TryGetValue(parameter.ParameterType, out object value))
                {
                    args.Add(value);
                }
                else
                {
                    if (requestArgs.Count == 0) return null;

                    args.Add(requestArgs.Dequeue());
                }
            }

            if (args.Count == constructor.GetParameters().Length)
            {
                return args.ToArray();
            }
            else
            {
                return null;
            }
        }

        private IEnumerable<Type> GetCommandsTypes()
        {
            return AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => _commandBaseType.IsAssignableFrom(type))
                .Where(type => IsRealClass(type));
        }

        private Type GetCommandTypeByName(string name)
        {
            return GetCommandsTypes()
                .Where(type => type.GetCustomAttributes<CommandAttribute>()
                                .Any(attribute => attribute.CommandName == name))
                .FirstOrDefault();
        }

        private bool IsRealClass(Type testType)
        {
            return testType.IsAbstract == false
                    && testType.IsGenericTypeDefinition == false
                    && testType.IsInterface == false;
        }

        private ICommand CreateInstance(Type type, Request request)
        {
            ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);

            foreach (var ctor in constructors)
            {
                var args = ResolveDependenciesAndMerge(ctor, request);
                if (args != null)
                {
                    return (ICommand)ctor.Invoke(args);
                }
            }

            return null;
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
                return new PayableOrder(Router._machine.GetFromId(request.Values[0]), request.Values[1]);     
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
