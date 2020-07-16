using System;
using System.Collections.Generic;
using System.Linq;

namespace 拥有最多糖果的孩子
{
    class Program
    {
        static void Main(string[] args)
        {
            var candies =new int[]{ 2, 3, 5, 1, 3 };
            var extraCandies = 3;
            var result=KidsWithCandies(candies, extraCandies);
            foreach (var b in result)
            {
                Console.WriteLine(b.ToString());
            }
            Console.ReadKey();
        }
        public static IEnumerable<bool> KidsWithCandies(int[] candies, int extraCandies)
        {
            foreach (var candy in candies)
            {
                var currentvalue = candy + extraCandies;

                var Maxds = candies.Max();

                if (currentvalue >= Maxds)
                    yield return true;
                else
                {
                    yield return false;
                }

            }
        }
    }
}
