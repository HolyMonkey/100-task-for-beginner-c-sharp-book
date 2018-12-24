using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandsSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            WendingMachine machine = new WendingMachine(balance: 0,
                    goods: new Good[]{
                        new Good("Шоколадка", price: 70, count: 5),
                        new Good("Газировка", price: 60, count: 2)
                    }
            );
            ICommandInput input = new ConsoleCommandInput(new Router(machine));

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Баланс {machine.Balance}");

                var command = input.GetCommand();
                if (command == null)
                {
                    Console.WriteLine("Команда не распознана");
                }
                else
                {
                    command.Execute();
                }
                Console.ReadKey();
            }
        }
    }
}
