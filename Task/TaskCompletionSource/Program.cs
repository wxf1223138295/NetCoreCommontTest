using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskCompletionSource
{

    class Test
    {
        public volatile int a;
    }
    class Program
    {
        static async Task Main(string[] args)
        {

            bool complete = false;
            new Thread(delegate ()
            {
                bool toggle = false;
                Console.Write("4");
                while (!complete)
                {
                    Console.Write("4");
                    Thread.Sleep(1000);
                    Console.Write("1");
                    toggle = !toggle;
                }
                  
            }).Start();

            Thread.Sleep(1000);


            complete = true;



            Console.WriteLine("ss");
            Console.ReadKey();
        }
    }

    public class EventClass
    {
        public Action<string> Done = (args) => { };

        public void Do()
        {
            Done("Done");
        }
    }
}
