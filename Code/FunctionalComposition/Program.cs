using System;

namespace FunctionalComposition
{
    class Program
    {
        private static int balance = 0;
        private static int[] coinsQuantity = { 0, 0, 0, 0 }; //1, 2, 5, 10
        private static int[] coinsValues = { 1, 2, 5, 10 };
        private static string[] names = { "Шоколадка", "Газировка" };
        private static int[] prices = { 70, 60 };
        private static int[] availableQuantity = { 5, 2 };
        private static PaymentType payment = PaymentType.Card;

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Баланс {balance}");

                string command = ReadCommand();
                ExecuteCommand(command);

                Console.ReadKey();
            }
        }

        private static string ReadCommand()
        {
            Console.WriteLine("Введите команду:");
            return Console.ReadLine();
        }

        private static void ExecuteCommand(string command)
        {
            if (command == "AddMoney")
            {
                switch (payment)
                {
                    case PaymentType.Coins:
                        for (int i = 0; i < coinsValues.Length; i++)
                        {
                            Console.WriteLine($"Сколько монет номиналом {coinsValues[i]} вы хотите внести?");
                            int count = ReadInt();
                            coinsQuantity[i] += count;
                            balance += count * coinsValues[i];
                        }
                        break;
                    case PaymentType.Card:
                        Console.WriteLine("Сколько снять с вашей карты?");
                        int balanceDelta = ReadInt();
                        balance += balanceDelta;
                        Console.WriteLine("Баланс успешно пополнен");
                        break;
                    default:
                        break;
                }
            }
            else if (command == "GetChange")
            {
                balance = 0;
            }
            else if (command.StartsWith("BuyGood"))
            {
                //Разбиение строки на единицы данных
                string[] rawData = command.Substring("BuyGood ".Length).Split(' ');

                //Сопоставление этих данных с переменными (и их типами) 
                if (rawData.Length != 2)
                {
                    Console.WriteLine("Неправильные аргументы команды");
                    return;
                }

                int id = 0;
                if (!MapParameter(rawData, out id, BuyGoodParameter.Id))
                {
                    return;
                }

                int count = 0;
                if (!MapParameter(rawData, out count, BuyGoodParameter.Count))
                {
                    return;
                }

                //Проверка корректности этих данных на основе текущего состояния модели.
                if (!Exist(id))
                {
                    Console.WriteLine("Такого товара нет");
                    return;
                }

                if (!IsAvailableInQuantity(id, count))
                {
                    Console.WriteLine("Нет такого количества");
                    return;
                }

                //Выполнение
                int totalPrice = GetTotalPrice(GetPrice(id), count);

                if (balance >= totalPrice)
                {
                    balance -= totalPrice;
                    availableQuantity[id] -= count;
                }
            }
            else
            {
                Console.WriteLine("Команда не определена");
            }

        }

        private static bool Exist(int id)
        {
            return id > 0 && id < names.Length;
        }

        private static void ValidateId(int id)
        {
            if (!Exist(id))
            {
                throw new ArgumentOutOfRangeException("id");
            }
        }

        private static string GetName(int id)
        {
            ValidateId(id);
            return names[id];
        }

        private static int GetPrice(int id)
        {
            ValidateId(id);
            return prices[id];
        }

        private static int GetAvailableQuantity(int id)
        {
            ValidateId(id);
            return availableQuantity[id];
        }

        private static bool IsAvailableInQuantity(int id, int count)
        {
            return count < 0 || count > GetAvailableQuantity(id);
        }

        private static int GetTotalPrice(int price, int count)
        {
            return price * count;
        }

        private static int ReadInt()
        {
            int result = 0;

            while (!int.TryParse(Console.ReadLine(),
                        out result))
            {
                Console.WriteLine("Вы ввели не число!");
            }

            return result;
        }

        private static bool MapParameter(string[] rawParams, out int containter, BuyGoodParameter parameter)
        {
            int index = (int)parameter;
            string name = Enum.GetName(typeof(BuyGoodParameter), parameter);

            if (!int.TryParse(rawParams[index], out containter))
            {
                Console.WriteLine($"Ошибка в параметре {name}, он должен быть числом");
                return false;
            }

            return true;
        }

    }

    enum BuyGoodParameter
    {
        Id = 0, 
        Count = 1
    }

    enum PaymentType
    {
        Coins,
        Card
    }
}
