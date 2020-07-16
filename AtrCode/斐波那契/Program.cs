using System;
using System.Collections.Generic;

namespace 斐波那契
{
    class Program
    {
        static void Main(string[] args)
        {
            var t = V3(5);

            for (int i = 0; i < 5; i++)
            {
                var r = feibonaqie(i+1);
                Console.WriteLine(r);
            }
             
            //foreach (var VARIABLE in t)
            //{
            //    Console.WriteLine(VARIABLE);
            //}
            
           
            Console.ReadKey();
        }

        private static int feibonaqie(int n)
        {
            if (n == 1)
            {
                return 1;
            }

            if (n == 2)
            {
                return 1;
            }

           var result = feibonaqie(n - 1) + feibonaqie(n - 2);


           return result;
        }


        private static IEnumerable<int> V3(int number)
        {
            Console.WriteLine("V3");
            int a = 0, b = 1, c = 0;
            for (int i = 0; i < number; i++)
            {
                yield return b;
                c = a + b;
                a = b;
                b = c;
            }
        }
    }
}
