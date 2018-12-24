using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicProgramming
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] values = { 1, 5, 10, 15, 17, 20 };
            int[] counts = { 5, 1, 1, 1, 1, 1 };
            int change = 34;

            Console.WriteLine(GetChangeTabulization(values, change, new int[change + 1]));
            Console.WriteLine(GetChangeSimple(values, change));

            foreach (var coin in GetChangeCoins(values, counts, change))
            {
                Console.WriteLine(coin);
            }
        }

        static int GetChangeRecursive(int[] values, int change)
        {
            int minCoins = change;

            if (values.Contains(minCoins))
            {
                return 1;
            }
            else
            {
                foreach (var value in values.Where(x => x <= change))
                {
                    int numCoins = 1 + GetChangeRecursive(values, change - value);
                    if (numCoins < minCoins)
                    {
                        minCoins = numCoins;
                    }
                }

                return minCoins;
            }
        }

        static int GetChangeMemoization(int[] values, int change, int[] getChangeResults)   
        {
            int minCoins = change;

            if (values.Contains(minCoins))
            {
                getChangeResults[change] = 1;
                return 1;
            }
            else if (getChangeResults[change] != 0)
            {
                return getChangeResults[change];
            }
            else
            {
                foreach (var value in values.Where(x => x <= change))
                {
                    int numCoins = 1 + GetChangeMemoization(values, change - value, getChangeResults);
                    if (numCoins < minCoins)
                    {
                        minCoins = numCoins;
                        getChangeResults[change] = minCoins;
                    }
                }
            }
            return minCoins;
        }

        static int GetChangeTabulization(int[] values, int change, int[] getChangeResults)
        {
            for (int cents = 0; cents < change+1; cents++)
            {
                int coinsCounts = cents;
                foreach (var value in values.Where(value => value <= cents))
                {
                    if(getChangeResults[cents-value] + 1 < coinsCounts)
                    {
                        coinsCounts = getChangeResults[cents - value] + 1;
                    }
                }
                getChangeResults[cents] = coinsCounts; 
            }

            return getChangeResults[change];
        }

        static int GetChangeSimple(int[] values, int change)
        {
            int count = 0;
            foreach(int value in values.Distinct().OrderByDescending(x => x))
            {
                count += change / value;
                change = change % value;
                if (change == 0) return count;
            }

            return 0;
        }

        static int GetChangeWithCoinsTabulization(int[] values, int[] counts, int change, int[] getChangeResults, int[] coins)
        {
            for (int cents = 0; cents < change + 1; cents++)
            {
                int coinsCounts = cents;
                int newCoin = 1;

                foreach (var value in values)
                {
                    if (counts[GetIndex(values, value)] <= 0) continue;
                    if (value > cents) continue;

                    if (getChangeResults[cents - value] + 1 < coinsCounts)
                    {
                        coinsCounts = getChangeResults[cents - value] + 1;
                        newCoin = value;
                    }
                }

                coins[cents] = newCoin; 
                getChangeResults[cents] = coinsCounts;

                counts[GetIndex(values, newCoin)] -= 1;
            }

            return getChangeResults[change];
        }

        static IEnumerable<int> GetChangeCoins(int[] values, int[] counts, int change)
        {
            int[] getChangeResults = new int[change + 1];
            int[] coins = new int[change + 1];

            GetChangeWithCoinsTabulization(values, counts, change, getChangeResults, coins);

            int coin = change;

            while(coin > 0)
            {
                int thisCoin = coins[coin];
                coin -= thisCoin;

                yield return thisCoin;
            }
        }

        static int GetIndex(int[] array, int element)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == element)
                {
                    return i;
                }
            }

            throw new ArgumentOutOfRangeException();
        }
    }
}
