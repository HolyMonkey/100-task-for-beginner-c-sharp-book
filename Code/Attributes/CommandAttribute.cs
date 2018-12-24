using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandsSystem
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    class CommandAttribute : Attribute
    {
        public string CommandName;

        public CommandAttribute(string commandName)
        {
            CommandName = commandName;
        }
    }
}
