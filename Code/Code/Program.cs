using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code
{
    class Program
    {
        static void Main(string[] args)
        {
            int balance = 0;
            int[] coinsQuantity = { 0, 0, 0, 0}; //1, 2, 5, 10
            int[] coinsValues  = { 1, 2, 5, 10}; 
            string[] names = { "Шоколадка", "Газировка" };
            int[] prices = { 70, 60 };
            int[] availableQuantity = { 5, 2 };
            PaymentType payment = PaymentType.Card;

            string command = "";
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Баланс {balance}");
                Console.WriteLine("Введите команду:");
                command = Console.ReadLine();

                if(command == "AddMoney")
                {
                    switch (payment)
                    {
                        case PaymentType.Coins:
                            for(int i = 0; i < coinsValues.Length; i++)
                            {
                                Console.WriteLine($"Сколько монет номиналом {coinsValues[i]} вы хотите внести?");
                                int count = 0;
                                while (!int.TryParse(Console.ReadLine(),
                                                        out count))
                                {
                                    Console.WriteLine("Вы ввели не число!");
                                }
                                coinsQuantity[i] += count;
                                balance += count * coinsValues[i];
                            }
                            break;
                        case PaymentType.Card:
                            Console.WriteLine("Сколько снять с вашей карты?");
                            int balanceDelta = 0;
                            while (!int.TryParse(Console.ReadLine(),
                                    out balanceDelta)) 
                            {
                                Console.WriteLine("Вы ввели не число!");
                            }
                            balance += balanceDelta;
                            Console.WriteLine("Баланс успешно пополнен");
                            break;
                        default:
                            break;
                    }
                }
                else if(command == "GetChange")
                {
                    balance = 0;
                }
                else if (command.StartsWith("BuyGood")) 
                {
                    //Разбиение строки на единицы данных
                    string[] rawData = command.Substring("BuyGood ".Length).Split(' ');

                    //Сопоставление этих данных с переменными (и их типами) 
                    if(rawData.Length != 2)
                    {
                        Console.WriteLine("Неправильные аргументы команды");
                        break;
                    }

                    int id = Convert.ToInt32(rawData[0]);
                    int count = Convert.ToInt32(rawData[1]);

                    //Проверка корректности этих данных на основе текущего состояния модели.
                    if(id < 0 || id >= names.Length)
                    {
                        Console.WriteLine("Такого товара нет");
                        break;
                    }

                    if(count < 0 || count > availableQuantity[id])
                    {
                        Console.WriteLine("Нет такого количества");
                        break;
                    }

                    //Выполнение
                    if(balance >= prices[id] * count)
                    {
                        balance -= prices[id] * count;
                        availableQuantity[id] -= count;
                    }
                }
                else
                {
                    Console.WriteLine("Команда не определена");
                }

                Console.ReadKey();
            }
        }
    }

    enum PaymentType
    {
        Coins,
        Card
    }
}
